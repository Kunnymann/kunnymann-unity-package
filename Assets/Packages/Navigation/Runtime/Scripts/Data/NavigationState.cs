namespace Kunnymann.Navigation.Data
{
    /// <summary>
    /// Navigation module state
    /// </summary>
    public enum NavigationState
    {
        /// <summary>
        /// Waiting for initialization
        /// </summary>
        None = 0,
        /// <summary>
        /// Ready
        /// </summary>
        Ready,
        /// <summary>
        /// Navigating
        /// </summary>
        Navigation,
        /// <summary>
        /// Waiting for resume
        /// </summary>
        Wait,
        /// <summary>
        /// Wrong path detected
        /// </summary>
        Wrong,
        /// <summary>
        /// Recovery navigation
        /// </summary>
        Recovery,
        /// <summary>
        /// Navigation end
        /// </summary>
        End,
    }

    public enum SessionEndType
    {
        /// <summary>
        /// Arrived at the destination
        /// </summary>
        Arrived = 0,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Canceled
        /// </summary>
        Canceled,
    }
}