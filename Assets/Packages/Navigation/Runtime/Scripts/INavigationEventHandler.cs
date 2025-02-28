using Kunnymann.Navigation.Data;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Navigation event handler interface.
    /// </summary>
    public interface INavigationEventHandler
    {
        /// <summary>
        /// Called when the navigation session is ready.
        /// </summary>
        public void OnReadyNavigationSession();
        /// <summary>
        /// Called when the navigation session is started.
        /// </summary>
        /// <param name="path">Current path</param>
        public void OnStartNavigationSession(Path path);
        /// <summary>
        /// Called when the navigation session is ended.
        /// </summary>
        /// <param name="end">Session end type</param>
        public void OnEndNavigationSession(SessionEndType end);
        /// <summary>
        /// Called when the navigation session state is changed.
        /// </summary>
        /// <param name="state">Navigation session state</param>
        public void OnSessionStateChanged(NavigationState state);
        /// <summary>
        /// Called when the navigation path is updated.
        /// </summary>
        /// <param name="path"></param>
        public void OnUpdatePath(Path path);
        /// <summary>
        /// Called when the navigation path is recovered.
        /// </summary>
        /// <param name="path"></param>
        public void OnRecoveryPath(Path path);
        /// <summary>
        /// Called when the target is entered transition zone.
        /// </summary>
        /// <param name="node">transition zone node</param>
        public void OnEnterTransition(INode node);
        /// <summary>
        /// Called when the target is exited transition zone.
        /// </summary>
        /// <param name="node"></param>
        public void OnExitTransition(INode node);
    }
}