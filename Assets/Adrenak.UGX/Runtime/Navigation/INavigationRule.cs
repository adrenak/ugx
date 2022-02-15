namespace Adrenak.UGX {
    /// <summary>
    /// Allows implementation of navigation rule. Use this to implement
    /// things like navigation history.
    /// </summary>
    public interface INavigationRule {
        /// <summary>
        /// Process a push. Provided is a unique identifier for
        /// the <see cref="Window"/> that the <see cref="Navigator"/> wants
        /// to push. Access to <see cref="Window"/> directly isn't provided
        /// to avoid interfering with opening and closing.
        /// </summary>
        void Push(int id);

        /// <summary>
        /// Process a pop. Called by <see cref="Navigator"/> when a pop needs
        /// to occur. Return a previously received ID received in 
        /// <see cref="Push(int)"/> to specify which window should be popped.
        /// If no window is to be popped, return null.
        /// </summary>
        /// <returns></returns>
        int? Pop();
    }
}
