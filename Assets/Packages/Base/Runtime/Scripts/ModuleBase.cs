using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈의 공통 베이스
    /// </summary>
    public class ModuleBase : BaseScriptableObject, IDisposable
    {
        /// <summary>
        /// 모듈 상태
        /// </summary>
        public ModuleState CurrentState 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 모듈 FMS
        /// </summary>
        private Dictionary<StateTransition, ModuleState> _transitions;
        /// <summary>
        /// 모듈 FMS 액션
        /// </summary>
        private Dictionary<ModuleState, ModuleLifeCycle> _lifeCycleActions;
        /// <summary>
        /// Main thread 작업
        /// </summary>
        private ConcurrentQueue<Action> _mainThreadWorks = new ConcurrentQueue<Action>();
        
        /// <summary>
        /// 모듈 FMS 액션 델리게이트
        /// </summary>
        private delegate void ModuleLifeCycle();

        /// <summary>
        /// 모듈 상태 전이 구조체
        /// </summary>
        private struct StateTransition
        {
            /// <summary>
            /// 하달 받은 명령
            /// </summary>
            private readonly TransitionCommand CurrentCommand;
            /// <summary>
            /// 현재 상태
            /// </summary>
            private readonly ModuleState CurrentState;

            /// <summary>
            /// 생성자
            /// </summary>
            /// <param name="state">상태</param>
            /// <param name="transition">명령</param>
            public StateTransition(ModuleState state, TransitionCommand transition)
            {
                CurrentState = state;
                CurrentCommand = transition;
            }

            public override int GetHashCode() => 17 + 31 * CurrentState.GetHashCode() + 31 * CurrentCommand.GetHashCode();
            public override bool Equals(object obj) => obj is StateTransition other && this.Equals(other);
            public bool Equals(StateTransition other) => CurrentState == other.CurrentState && CurrentCommand == other.CurrentCommand;
        }

        /// <summary>
        /// 생성자
        /// Default FMS를 구축합니다
        /// </summary>
        public ModuleBase()
        {
            CurrentState = ModuleState.Created;

            _transitions = new Dictionary<StateTransition, ModuleState>();
            _lifeCycleActions = new Dictionary<ModuleState, ModuleLifeCycle>();

            _transitions.Add(new StateTransition(ModuleState.Created, TransitionCommand.Initialize), ModuleState.Inactive);
            _transitions.Add(new StateTransition(ModuleState.Inactive, TransitionCommand.None), ModuleState.Active);
            _transitions.Add(new StateTransition(ModuleState.Active, TransitionCommand.None), ModuleState.Active);
            _transitions.Add(new StateTransition(ModuleState.Inactive, TransitionCommand.Terminate), ModuleState.Terminated);
            _transitions.Add(new StateTransition(ModuleState.Active, TransitionCommand.Terminate), ModuleState.Terminated);

            _lifeCycleActions.Add(ModuleState.Inactive, ModuleInitialize);
            _lifeCycleActions.Add(ModuleState.Active, ModuleUpdate);
            _lifeCycleActions.Add(ModuleState.Terminated, ModuleTerminate);

            Process(TransitionCommand.Initialize);
        }

        /// <summary>
        /// Main thread에 액션을 밀어넣습니다
        /// </summary>
        /// <param name="action"></param>
        public void RunOnMainThread(params Action[] action)
        {
            foreach (var item in action)
            {
                _mainThreadWorks.Enqueue(item);
            }
        }

        /// <summary>
        /// 모듈의 초기화 사이클
        /// </summary>
        public virtual void ModuleInitialize()
        {
            
        }

        /// <summary>
        /// 모듈의 Unity update 사이클
        /// </summary>
        public virtual void ModuleUpdate()
        {
            while (_mainThreadWorks.TryDequeue(out Action action))
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// 모듈의 종료 사이클
        /// </summary>
        public virtual void ModuleTerminate()
        {

        }

        /// <summary>
        /// 모듈 동작을 처리합니다
        /// </summary>
        /// <param name="transition">명령</param>
        /// <exception cref="Exception">비정상적인 프로세스에 대한 Exception</exception>
        internal void Process(TransitionCommand transition = TransitionCommand.None)
        {
            StateTransition stateTransition = new StateTransition(CurrentState, transition);
            ModuleState nextState;

            if (!_transitions.TryGetValue(stateTransition, out nextState))
            {
                throw new Exception("비정상적인 프로세스입니다.");
            }

            if (_lifeCycleActions.ContainsKey(nextState))
            {
                _lifeCycleActions[nextState].Invoke();
            }

            CurrentState = nextState;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (Application.isPlaying)
            {
                Process(TransitionCommand.Terminate);
            }
        }
    }
}