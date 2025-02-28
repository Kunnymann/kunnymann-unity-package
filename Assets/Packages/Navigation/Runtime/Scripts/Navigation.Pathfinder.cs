using Kunnymann.Base;
using Kunnymann.Navigation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// A* Node
    /// </summary>
    internal class AStarNode : IEquatable<AStarNode>
    {
        /// <summary>
        /// Node
        /// </summary>
        internal INode Node { get; private set; }
        /// <summary>
        /// FromNode
        /// </summary>
        internal INode FromNode { get; private set; }
        /// <summary>
        /// Link
        /// </summary>
        internal ILink Connected { get; private set; }

        /// <summary>
        /// A* weight G
        /// </summary>
        internal float G { get; set; }
        /// <summary>
        /// A* weight H
        /// </summary>
        internal float H { get; set; }
        /// <summary>
        /// A* weight F
        /// </summary>
        internal float F => G + H;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Target node</param>
        /// <param name="fromNode">From node</param>
        internal AStarNode(INode node, INode fromNode = null)
        {
            this.Node = node;

            if (fromNode == null)
            {
                return;
            }

            this.FromNode = fromNode;
            this.Connected = this.FromNode.Links?.FirstOrDefault(link =>
            {
                return link.From == this.Node || link.To == this.Node;
            });
        }

        public bool Equals(AStarNode other)
        {
            return Node == other.Node;
        }

        public override int GetHashCode()
        {
            return -1993120700 + EqualityComparer<INode>.Default.GetHashCode(Node);
        }

        public override string ToString()
        {
            return $"AStarNode, Node ({Node.ID}), FromNode ({FromNode?.ID})";
        }
    }

    /// <summary>
    /// A* Path Generator
    /// </summary>
    internal class AStarPathGenerator
    {
        /// <summary>
        /// OpenList container
        /// </summary>
        private List<AStarNode> _openList = new List<AStarNode>();
        /// <summary>
        /// CloseList container
        /// </summary>
        private List<AStarNode> _closedList = new List<AStarNode>();

        /// <summary>
        /// Calculate A* path
        /// </summary>
        /// <param name="fromNode">From node</param>
        /// <param name="toNode">To node</param>
        /// <param name="transitionType">Transition type</param>
        private void Calculate(AStarNode fromNode, AStarNode toNode, TransitionType transitionType = TransitionType.Path)
        {
            _closedList.Add(fromNode);
            foreach (var connected in fromNode.Node.ConnectedNode)
            {
                AStarNode connectedAStarNode = new AStarNode(connected, fromNode.Node);
                if (_closedList.Contains(connectedAStarNode))
                {
                    continue;
                }
                
                // Link로 인접한 두 노드 모두 동일한 TransitionType에, Path가 아닌 경우
                if (fromNode.Node.TransitionType == connectedAStarNode.Node.TransitionType && fromNode.Node.TransitionType != TransitionType.Path)
                {
                    // 지정된 TransitionType이 아니라면, 안내하지 않음
                    if (fromNode.Node.TransitionType != transitionType)
                    {
                        // 이 곳으로는 절대로 안내하면 안되니깐 오픈리스트에서 제외
                        _openList.Remove(connectedAStarNode);
                        continue;
                    }
                }

                // Calculate weight
                connectedAStarNode.G = fromNode.G + connectedAStarNode.Connected.Distance;
                connectedAStarNode.H = (toNode.Node.Position - connectedAStarNode.Node.Position).magnitude;

                if (_openList.Contains(connectedAStarNode))
                {
                    var openedAStarNode = _openList.Single(target => target.Node.ID == connectedAStarNode.Node.ID);

                    if (openedAStarNode != null)
                    {
                        // If the calculated weight is greater than the existing weight, skip
                        if (openedAStarNode.F < connectedAStarNode.F)
                        {
                            continue;
                        }
                    }
                    _openList.Remove(connectedAStarNode);
                }
                _openList.Add(connectedAStarNode);
            }
        }

        /// <summary>
        /// Find path
        /// </summary>
        /// <param name="startNode">Start node</param>
        /// <param name="endNode">End node</param>
        /// <param name="transitionType">Transition type</param>
        /// <returns></returns>
        internal static Path FindPath(INode startNode, INode endNode, TransitionType transitionType)
        {
            AStarNode from = new AStarNode(startNode);
            AStarNode to = new AStarNode(endNode);

            AStarNode fromNode = from;
            AStarNode toNode = to;

            AStarPathGenerator generator = new AStarPathGenerator();
            do
            {
                // Path calculation
                generator.Calculate(fromNode, toNode, transitionType);

                fromNode = generator._openList.OrderBy(node => node.F).FirstOrDefault();
                if (fromNode == null)
                {
                    if (generator._openList.Contains(toNode))
                    {
                        break;
                    }

                    return null;
                }
                generator._openList.Remove(fromNode);
            }
            while (!generator._closedList.Contains(toNode));

            AStarNode currentNode = generator._closedList.Last();
            List<AStarNode> retNodes = new List<AStarNode>();

            while (currentNode != from)
            {
                retNodes.Add(currentNode);
                currentNode = generator._closedList.FirstOrDefault(node => node.Node.ID == currentNode.FromNode.ID);
            }
            retNodes.Add(from);
            retNodes.Reverse();

            Path path = new Path();
            foreach (var found in retNodes)
            {
                if (found.Node != null && found.FromNode != null)
                {
                    path.Add(found.Connected);
                }
                path.Add(found.Node);
            }

            return path;
        }

        /// <summary>
        /// Find path
        /// </summary>
        /// <param name="position">Vector3 position</param>
        /// <param name="endNode">End node</param>
        /// <param name="transition">Transition type</param>
        /// <returns>Path</returns>
        public static Path FindPath(Vector3 position, INode endNode, TransitionType transition)
        {
            // 이거 좀 어떻게 해봐... 더러워서 쓰겠나 이거
            INode startNode = ModuleManager.Instance.GetModule<NavigationModule>().Nodes.
                              OrderBy(node => Vector3.Distance(node.Position, position)).First();
            return FindPath(startNode, endNode, transition);
        }
    }
}