using System;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for view state structure
    /// </summary>
    [Serializable]
    public abstract class State {
        /// <summary>
        /// An ID that can be used to identify this object
        /// </summary>
        public string ID;
    }
}