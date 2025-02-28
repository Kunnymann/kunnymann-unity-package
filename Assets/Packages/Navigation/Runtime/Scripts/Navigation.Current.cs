using Kunnymann.Navigation.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Current point in the navigation system.
    /// </summary>
    public class CurrentPoint : INode
    {
        /// <summary>
        /// Distance to the next node.
        /// </summary>
        public float Distance
        {
            get; internal set;
        }

        /// <summary>
        /// Position in current link.
        /// </summary>
        public Vector3 Position
        {
            get; internal set;
        }

        /// <summary>
        /// Direction vector to the next node.
        /// </summary>
        public Vector3 Direction
        {
            get; internal set;
        }

        public string ID => NavigationConst.CurrentPointID;

        public string Name => NavigationConst.CurrentPointName;

        public TransitionType TransitionType => TransitionType.Path;
        
        [Obsolete("Not implemented")]
        public IReadOnlyList<ILink> Links => throw new NotImplementedException();
        
        [Obsolete("Not implemented")]
        public IEnumerable<INode> ConnectedNode => throw new NotImplementedException();

        internal CurrentPoint(Vector3 position)
        {
            Position = position;
        }

        internal void UpdateValue(INode node)
        {
            Distance = Vector3Utility.GetXZDistance(Position, node.Position);
            Direction = (node.Position - Position).normalized;
        }

        [Obsolete("Not implemented")]
        public void AddLink(ILink link)
        {
            throw new System.NotImplementedException();
        }

        [Obsolete("Not implemented")]
        public void AddConnectedNode(INode node)
        {
            throw new NotImplementedException();
        }
    }
}