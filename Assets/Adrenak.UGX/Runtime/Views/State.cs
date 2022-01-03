using System;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for view state structure
    /// </summary>
    [Serializable]
    public abstract class State {
        /// <summary>
        /// An ID that can be used to identify it.
        /// </summary>
        public string ID;
    }
}