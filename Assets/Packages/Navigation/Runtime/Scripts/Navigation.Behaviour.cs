using Kunnymann.Navigation.Data;
using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Navigation behaviour base class.
    /// </summary>
    public class NavigationBehaviour : MonoBehaviour, INavigationEventHandler
    {
        /// <summary>
        /// Navigation module.
        /// </summary>
        public NavigationModule Navigation { get; set; }

        public virtual void OnReadyNavigationSession() { }
        public virtual void OnStartNavigationSession(Path path) { }
        public virtual void OnEndNavigationSession(SessionEndType end) { }
        public virtual void OnSessionStateChanged(NavigationState state) { }
        public virtual void OnRecoveryPath(Path path) { }
        public virtual void OnUpdatePath(Path path) { }
        public virtual void OnEnterTransition(INode node) { }
        public virtual void OnExitTransition(INode node) { }
    }
}