using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Navigation.Sample
{
    public class Node : INode
    {
        internal List<string> connectedNodeIds = new List<string>(); 

        private Vector3 _position;
        public Vector3 Position => _position;

        private TransitionType _transitionType;
        public TransitionType TransitionType => _transitionType;

        private List<ILink> _links;
        public IReadOnlyList<ILink> Links => _links;

        private List<INode> _connectedNode;
        public IEnumerable<INode> ConnectedNode => _connectedNode;

        private string _id;
        public string ID => _id;

        private string _name;
        public string Name => _name;

        public Node(Vector3 position, TransitionType transitionType, string id, string name)
        {
            _position = position;
            _transitionType = transitionType;
            _id = id;
            _name = name;
        }

        public void AddLink(ILink link)
        {
            if (_links == null)
            {
                _links = new List<ILink>();
            }
            _links.Add(link);
        }

        public void AddConnectedNode(INode node)
        {
            if (_connectedNode == null)
            {
                _connectedNode = new List<INode>();
            }
            _connectedNode.Add(node);
        }
    }

    public class Link : ILink
    {
        private INode _from;
        public INode From => _from;

        private INode _to;
        public INode To => _to;

        private LinkDirection _direction;
        public LinkDirection Direction => _direction;

        public float Distance => Vector3.Distance(_from.Position, _to.Position);

        private bool _isTransitionLink;
        public bool IsTransitionLink => _isTransitionLink;

        private string _id;
        public string ID => _id;

        private string _name;
        public string Name => _name;

        public Link(INode from, INode to, LinkDirection direction, bool isTransitionLink, string id, string name)
        {
            _from = from;
            _to = to;
            _direction = direction;
            _isTransitionLink = isTransitionLink;
            _id = id;
            _name = name;
        }

        public void Reverse()
        {
            INode temp = _from;
            _from = _to;
            _to = temp;
        }
    }
}