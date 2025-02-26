using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Navigation configuration
    /// </summary>
    public abstract class NavigationConfig : ScriptableObject
    {
        protected void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize navigation raw data
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Return INode container
        /// </summary>
        /// <returns>INode container</returns>
        public abstract List<INode> GetNodeContainer();
        /// <summary>
        /// Return ILink container
        /// </summary>
        /// <returns>ILink container</returns>
        public abstract List<ILink> GetLinkContainer();
    }
}
