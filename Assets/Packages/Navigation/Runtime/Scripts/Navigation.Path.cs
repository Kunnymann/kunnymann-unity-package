using Kunnymann.Base;
using Kunnymann.Navigation.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Node와 Link로 구성된 Path
    /// </summary>
    public class Path : IEnumerable<PathUnit>
    {
        private List<PathUnit> _pathNodeLinkCollection = new List<PathUnit>();
        private CurrentPoint _currentPoint;

        /// <summary>
        /// Start node of path
        /// </summary>
        public INode StartNode => _pathNodeLinkCollection.First() as INode;
        /// <summary>
        /// End node of path
        /// </summary>
        public INode EndNode => _pathNodeLinkCollection.Last() as INode;
        /// <summary>
        /// Current transition node
        /// </summary>
        public INode CurrentTransitionNode => GetLinkFromCurrentToEnd().OfType<ILink>().Where(link => link.IsTransitionLink == true).Select(link => link.From).FirstOrDefault();
        /// <summary>
        /// All transition node
        /// </summary>
        public List<INode> PathTransitionNodes => _pathNodeLinkCollection.OfType<ILink>().Where(link => link.IsTransitionLink == true).Select(link => link.From).ToList();
        /// <summary>
        /// PathUnit count
        /// </summary>
        public int Count => this.Count();
        /// <summary>
        /// Node count
        /// </summary>
        public int NodeCount => _pathNodeLinkCollection.Count(pathUnit => pathUnit is INode);
        /// <summary>
        /// Link count
        /// </summary>
        public int LinkCount => _pathNodeLinkCollection.Count(pathUnit => pathUnit is ILink);
        /// <summary>
        /// Path distance
        /// </summary>
        public float AllDistance => _pathNodeLinkCollection.Sum(pathUnit => (pathUnit as ILink)?.Distance ?? 0f);
        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator<PathUnit> GetEnumerator() => GetPathNodeEnumerator().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <summary>
        /// Return path node enumerator
        /// </summary>
        /// <returns>Enumerable PathUnit</returns>
        internal IEnumerable<PathUnit> GetPathNodeEnumerator()
        {
            foreach (var item in _pathNodeLinkCollection)
            {
                if (item as INode != null)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Path string info
        /// </summary>
        /// <returns>Info</returns>
        public override string ToString()
        {
            return $"Path, start node : {StartNode.ID}, end node : {EndNode.ID}, distance : {AllDistance}";
        }

        /// <summary>
        /// Add PathUnit
        /// </summary>
        /// <param name="pathUnit">PathUnit (Node or Link)</param>
        internal void Add(PathUnit pathUnit)
        {
            if (pathUnit is ILink)
            {
                ILink link = pathUnit as ILink;

                // 추가된 Link가 양방향성에 From 노드가 Path의 마지막 노드와 연결되어있지 않다면, Link를 Reverse하여, To 노드와 연결을 시도
                if (link.Direction == LinkDirection.Bidirection)
                {
                    INode lastNode = _pathNodeLinkCollection.Last() as INode;
                    if (link.From != lastNode)
                    {
                        link.Reverse();
                    }
                }
            }
            
            _pathNodeLinkCollection.Add(pathUnit);
        }

        /// <summary>
        /// Return Link is nearest from position
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Tuple (Need recovery, Link, Link index, Link weight, Positin in Link)</returns>
        internal (bool refind, ILink link, int collectionIndex, float weight, Vector3 positionInLink) GetNearestInLink(Vector3 position)
        {
            (bool refind, ILink link, int collectionIndex, float weight, Vector3 positionInLink) result = default;
            result.refind = false;
            result.weight = float.MaxValue;
            result.positionInLink = position;
            result.positionInLink.y = 0f;           // 맵 상 노드 데이터와 동일하게 height값을 0으로 설정

            // if only one node in path
            if (_pathNodeLinkCollection.Count(unit => unit is ILink) == 0)
            {
                if (_pathNodeLinkCollection.Count() == 0)
                {
                    result.weight = 0f;
                    result.refind = true;
                }
                else
                {
                    INode node = _pathNodeLinkCollection.FirstOrDefault() as INode;
                    result.weight = Vector3.Distance(node.Position, position);
                    result.positionInLink = node.Position;
                }

                return result;
            }

            // if there are more than one node in path
            foreach (ILink link in _pathNodeLinkCollection.Where(unit => unit is ILink))
            {
                var fromUnit = link.From;
                var toUnit = link.To;
                float weight = GetDistanceFromLink(fromUnit.Position, toUnit.Position, position, out Vector3 pointInLink);

                if (result.weight > weight)
                {
                    result.link = link;
                    result.weight = weight;
                    result.positionInLink = pointInLink;
                    result.positionInLink.y = 0f;   // 맵 상 노드 데이터와 동일하게 height값을 0으로 설정
                }
            }
            return result;
        }

        /// <summary>
        /// Return Nodes from current position to end
        /// </summary>
        /// <returns>Node collection</returns>
        public IEnumerable<INode> GetNodeFromCurrentToEnd()
        {
            var currentInLink = GetNearestInLink(GetCurrentPosition());
            ILink link = currentInLink.link;
            if (_currentPoint == null)
            {
                _currentPoint = new CurrentPoint(currentInLink.positionInLink);
            }
            _currentPoint.Position = currentInLink.positionInLink;

            var nodeCollection = _pathNodeLinkCollection.SkipWhile(pathUnit => (pathUnit as ILink) != currentInLink.link)
                                                        .OfType<INode>().ToList();

            INode nextNode = nodeCollection.FirstOrDefault();
            _currentPoint.UpdateValue(nextNode);

            yield return _currentPoint;

            // 가장 인접한 Link로 투영을 시도했을 때, Link가 Joint라면, 안전하게 경로를 렌더링하기 위해, From 노드를 반환
            if (currentInLink.link.IsTransitionLink)
            {
                yield return currentInLink.link.From;
            }

            foreach (var unit in nodeCollection)
            {
                yield return unit;
            }
        }

        /// <summary>
        /// Return Links from current position to end
        /// </summary>
        /// <returns>Link collection</returns>
        public IEnumerable<ILink> GetLinkFromCurrentToEnd()
        {
            var currentInLink = GetNearestInLink(GetCurrentPosition());

            if (_currentPoint == null)
            {
                _currentPoint = new CurrentPoint(currentInLink.positionInLink);
            }
            _currentPoint.Position = currentInLink.positionInLink;

            var linkCollection = _pathNodeLinkCollection.SkipWhile(pathUnit => (pathUnit as ILink) != currentInLink.link)
                                                        .OfType<ILink>();

            foreach (var unit in linkCollection)
            {
                yield return unit;
            }
        }

        private Vector3 GetCurrentPosition()
        {
            return ModuleManager.Instance.GetModule<NavigationModule>().CurrentPose;
        }

        private float GetDistanceFromLink(Vector3 point1_InLink, Vector3 point2_InLink, Vector3 target, out Vector3 projectPoint)
        {
            // Target으로부터 Link에 투영된 Point
            projectPoint = FindProjectedPointInLine(point1_InLink, point2_InLink, target);

            // 해당 Point가 Link 위에 존재하는지 확인
            if (projectPoint.x >= Mathf.Min(point1_InLink.x, point2_InLink.x) && projectPoint.x <= Mathf.Max(point1_InLink.x, point2_InLink.x) &&
                projectPoint.z >= Mathf.Min(point1_InLink.z, point2_InLink.z) && projectPoint.z <= Mathf.Max(point1_InLink.z, point2_InLink.z))
            {
                // Link와의 거리 계산
                return Vector3Utility.GetXZDistance(target, projectPoint);
            }
            else
            {
                // 그게 아니라면, 단순 끝점과의 거리 계산
                float weight1 = Vector3Utility.GetXZDistance(target, point1_InLink);
                float weight2 = Vector3Utility.GetXZDistance(target, point2_InLink);
                if (weight1 < weight2)
                {
                    projectPoint = point1_InLink;
                }
                else
                {
                    projectPoint = point2_InLink;
                }

                return Mathf.Min(weight1, weight2);
            }
        }

        private Vector3 FindProjectedPointInLine(Vector3 pointInLine1, Vector3 pointInLine2, Vector3 target)
        {
            Vector3 p = target;

            float m1, k1, m2, k2;

            if (pointInLine1.x == pointInLine2.x)       // 선분이 x축에 평행한 case
            {
                p.x = pointInLine1.x;
                p.z = target.z;
            }
            else if (pointInLine1.z == pointInLine2.z)  // 선분이 z축에 평행한 case
            {
                p.x = target.x;
                p.z = pointInLine1.z;
            }
            else                                        // 일반적인 case 
            {
                m1 = (pointInLine1.z - pointInLine2.z) / (pointInLine1.x - pointInLine2.x);
                k1 = pointInLine1.z - m1 * pointInLine1.x;

                m2 = -1 / m1;
                k2 = target.z - m2 * target.x;

                p.x = (k2 - k1) / (m1 - m2);
                p.z = m1 * p.x + k1;
            }

            return p;
        }

        /// <summary>
        /// Return tuple is recovery necessity and nearest link weight
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>tuple</returns>
        internal (float weight, bool isNeeded) ShouldRecoveryPath(Vector3 position)
        {
            var currentInLinkUnit = GetNearestInLink(position);

            return (currentInLinkUnit.weight, currentInLinkUnit.refind);
        }
    }
}