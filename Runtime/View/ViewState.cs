using System;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for View state definitions
    /// </summary>
    [Serializable]
    public abstract class ViewState {
        /// <summary>
        /// An ID that can be used to identify it. Optional.
        /// </summary>
        public string ID;
    }
}