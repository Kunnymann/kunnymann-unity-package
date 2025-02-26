using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Navigation
{
    public class CurrentPoint : INode
    {
        public float Distance
        {
            get; internal set;
        }

        public Vector3 Position
        {
            get; internal set;
        }

        public Vector3 Direction
        {
            get; internal set;
        }

        public string ID => NavigationConst.CurrentPointID;

        public string Name => NavigationConst.CurrentPointName;

        public TransitionType TransitionType => TransitionType.Path;
        
        [Obsolete("Not implemented")]
        public IReadOnlyList<ILink> Links => throw new System.NotImplementedException();
        
        [Obsolete("Not implemented")]
        public IEnumerable<INode> ConnectedNode => throw new System.NotImplementedException();

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