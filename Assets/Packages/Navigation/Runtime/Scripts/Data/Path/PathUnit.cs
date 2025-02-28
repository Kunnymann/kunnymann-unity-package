using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Navigation.Data
{
    /// <summary>
    /// Units that make up the navigation path
    /// </summary>
    public interface PathUnit
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// Node interface
    /// </summary>
    public interface INode : PathUnit
    {
        /// <summary>
        /// Unity position
        /// </summary>
        public Vector3 Position { get; }
        /// <summary>
        /// Transition type
        /// </summary>
        public TransitionType TransitionType { get; }
        /// <summary>
        /// Link data that connects to node
        /// </summary>
        public IReadOnlyList<ILink> Links { get; }
        /// <summary>
        /// Connected node
        /// </summary>
        public IEnumerable<INode> ConnectedNode { get; }
        /// <summary>
        /// Add link
        /// </summary>
        /// <param name="link">Link instance</param>
        public void AddLink(ILink link);
        /// <summary>
        /// Add connected node
        /// </summary>
        /// <param name="node">Node instance</param>
        public void AddConnectedNode(INode node);
    }

    /// <summary>
    /// Link interface
    /// </summary>
    public interface ILink : PathUnit
    {
        /// <summary>
        /// From node
        /// </summary>
        public INode From { get; }
        /// <summary>
        /// To node
        /// </summary>
        public INode To { get; }
        /// <summary>
        /// Connected direction
        /// </summary>
        /// <remarks>
        /// Forward: From -> To, Backward : From <- To, Bidirection : From <-> To
        /// </remarks>
        public LinkDirection Direction { get; }
        /// <summary>
        /// Distance
        /// </summary>
        public float Distance { get; }
        /// <summary>
        /// Is transition link
        /// </summary>
        public bool IsTransitionLink { get; }
        /// <summary>
        /// Reverse link data
        /// </summary>
        public void Reverse();
    }
}