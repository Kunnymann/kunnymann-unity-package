using Kunnymann.Base;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Navigation module
    /// </summary>
    [Serializable]
    public class NavigationModule : ModuleBase
    {
        private bool _initialized = false;
        private float _distanceFromCurrentNode = 0;
        private Transform _target;
        private NavigationConfig _configuration;
        private List<INavigationEventHandler> _behaviour = new List<INavigationEventHandler>();
        
        /// <summary>
        /// Return Node container
        /// </summary>
        public List<INode> Nodes => _configuration.GetNodeContainer();

        /// <summary>
        /// Return Link container
        /// </summary>
        public List<ILink> Links => _configuration.GetLinkContainer();

        private Vector3 _currentPose;
        /// <summary>
        /// Return current pose
        /// </summary>
        public Vector3 CurrentPose => _currentPose;

        private Path _path;
        /// <summary>
        /// Return current path
        /// </summary>
        public Path ActivePath => _path;

        private NavigationState _state = NavigationState.None;
        /// <summary>
        /// Return current navigation state
        /// </summary>
        public NavigationState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    _behaviour?.ForEach(behaviour => behaviour.OnSessionStateChanged(_state));
                }
            }
        }

        private TransitionType _currentTransitType = TransitionType.Path;
        /// <summary>
        /// Return current transition type
        /// </summary>
        public TransitionType CurrentTransitType => _currentTransitType;

        /// <summary>
        /// Initialize navigation module
        /// </summary>
        /// <param name="config">Navigation configuration</param>
        /// <param name="setTarget">Target to track</param>
        public void Ready(NavigationConfig config, Transform setTarget)
        {
            _target = setTarget;
            _currentPose = _target.position;

            _configuration = config;
            _configuration.Initialize();
            
            if (Nodes == null || Nodes.Count == 0)
            {
                Debug.LogError("Node is not set");
                return;
            }

            if (Links == null || Links.Count == 0)
            {
                Debug.LogError("Link is not set");
                return;
            }

            _initialized = true;

            State = NavigationState.Ready;
            _behaviour?.ForEach(behaviour => behaviour.OnReadyNavigationSession());

            Observable.EveryUpdate().Subscribe(_ => Update());
        }

        private void Update()
        {
            _currentPose = _target.position;

            if (!_initialized)
            {
                return;
            }

            OnPoseUpdated(_currentPose);
        }

        /// <summary>
        /// Register NavigationEventHandler
        /// </summary>
        /// <param name="behavour">NavigationEventHandler</param>
        public void RegisterBehavour(INavigationEventHandler behavour)
        {
            _behaviour.Add(behavour);
        }

        /// <summary>
        /// Unregister NavigationEventHandler
        /// </summary>
        /// <param name="behavour">NavigationEventHandler</param>
        public void UnregisterBehavour(INavigationEventHandler behavour)
        {
            _behaviour.Remove(behavour);
        }

        /// <summary>
        /// Return path from start node to end node
        /// </summary>
        /// <param name="from">Start node</param>
        /// <param name="to">End node</param>
        /// <param name="transitionType">Transition method</param>
        /// <returns>Path</returns>
        public Path GetPath(INode from, INode to, TransitionType transitionType)
        {
            return AStarPathGenerator.FindPath(from, to, transitionType);
        }

        /// <summary>
        /// Return path from Vector3 position to end node
        /// </summary>
        /// <param name="from">Vector3 position</param>
        /// <param name="to">End node</param>
        /// <param name="transitionType">Transition method</param>
        /// <returns>Path</returns>
        public Path GetPath(Vector3 from, INode to, TransitionType transitionType)
        {
            return AStarPathGenerator.FindPath(from, to, transitionType);
        }

        /// <summary>
        /// Return path from current position to end node
        /// </summary>
        /// <param name="destination">End node</param>
        /// <param name="transition">Transition method</param>
        public void SetupPath(INode destination, TransitionType transition)
        {
            _path = AStarPathGenerator.FindPath(Vector3Utility.XZProject(_currentPose), destination, transition);
            _distanceFromCurrentNode = Vector3Utility.GetXZDistance(_currentPose, _path.StartNode.Position);
        }

        /// <summary>
        /// Navigation recovery path
        /// </summary>
        /// <returns>Recovery success</returns>
        public bool RecoveryPath()
        {
            if (_path != null)
            {
                if (State == NavigationState.Navigation)
                {
                    State = NavigationState.Wrong;
                }

                INode destination = _path.EndNode;
                _path = AStarPathGenerator.FindPath(Vector3Utility.XZProject(_currentPose), destination, _currentTransitType);
                
                if (_path == null)
                {
                    return false;
                }
                _distanceFromCurrentNode = Vector3Utility.GetXZDistance(_currentPose, _path.StartNode.Position);

                if (State == NavigationState.Wrong)
                {
                    State = NavigationState.Recovery;
                    _behaviour?.ForEach(behaviour => behaviour.OnRecoveryPath(_path));
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Start navigation
        /// </summary>
        public void Start()
        {
            if (State != NavigationState.Ready)
            {
                Cancel();
            }

            if (_path == null)
            {
                Debug.LogError("Path is not set");
                return;
            }
            else
            {
                State = NavigationState.Navigation;
                _behaviour?.ForEach(behaviour => behaviour.OnStartNavigationSession(_path));
            }
        }

        /// <summary>
        /// Cancel navigation
        /// </summary>
        public void Cancel()
        {
            if (State != NavigationState.Ready)
            {
                _behaviour?.ForEach(behaviour => behaviour.OnEndNavigationSession(SessionEndType.Canceled));
                EndNavigationSession();
            }
        }

        private void OnPoseUpdated(Vector3 pose)
        {
            _currentPose = pose;

            // Navigation arrival process
            if (State == NavigationState.Navigation)
            {
                // Check if arrived
                if (IsArrivedDestination())
                {
                    State = NavigationState.End;
                    _behaviour?.ForEach(behaviour => behaviour.OnEndNavigationSession(SessionEndType.Arrived));
                    EndNavigationSession();
                    return;
                }

                // Check transition event
                if (IsEnteredTransition())
                {
                    _behaviour?.ForEach(behaviour => behaviour.OnEnterTransition(_path.CurrentTransitionNode));
                }
            }

            // Navigation common process
            if (_path != null)
            {
                var recoveryResult = _path.ShouldRecoveryPath(Vector3Utility.XZProject(_currentPose));

                if (recoveryResult.isNeeded || recoveryResult.weight >= _distanceFromCurrentNode + NavigationConst.RecoveryThreshold)
                {
                    // if algorithm can't find path, end navigation session and invoke error
                    if (!RecoveryPath())
                    {
                        _behaviour?.ForEach(behaviour => behaviour.OnEndNavigationSession(SessionEndType.Error));
                        EndNavigationSession();
                        return;
                    }

                    if (State == NavigationState.Recovery)
                    {
                        State = NavigationState.Navigation;
                    }
                    return;
                }
                else if (recoveryResult.weight < _distanceFromCurrentNode)
                {
                    _distanceFromCurrentNode = recoveryResult.weight;
                }

                _behaviour?.ForEach(behaviour => behaviour.OnUpdatePath(_path));
            }
        }

        /// <summary>
        /// Check arrived destination
        /// </summary>
        /// <returns>arrived</returns>
        private bool IsArrivedDestination()
        {
            if (Vector3Utility.GetXZDistance(_currentPose, _path.EndNode.Position) < NavigationConst.ArrivalThreshold)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check entered transition
        /// </summary>
        /// <returns>entered</returns>
        private bool IsEnteredTransition()
        {
            // 만약 현재 Transition Node가 없다면 종료
            if (_path.CurrentTransitionNode == null)
            {
                return false;
            }

            // enteredTransition이 false이고, 현재 위치와 현재 Transition Node가 근접하면 Enter 이벤트 발행
            if (Vector3Utility.GetXZDistance(_currentPose, _path.CurrentTransitionNode.Position) < NavigationConst.EnterTransitionThreshold)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// End navigation session
        /// </summary>
        private void EndNavigationSession()
        {
            _currentTransitType = TransitionType.Path;
            _path = null;
            State = NavigationState.Ready;
        }
    }
}
