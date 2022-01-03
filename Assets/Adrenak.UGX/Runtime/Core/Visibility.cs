using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Used to represent visibility of a <see cref="RectTransform"/> 
    /// within its UI canvas
    /// </summary>
    public enum Visibility {
        /// <summary>
        /// The entire <see cref="RectTransform"/> is visible on the canvas
        /// </summary>
        Full,

        /// <summary>
        /// The <see cref="RectTransform"/> is partially visible on the canvas
        /// </summary>
        Partial,

        /// <summary>
        /// No part of the <see cref="RectTransform"/> is visible on the canvas
        /// </summary>
        None
    }
}