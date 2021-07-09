using UnityEngine;
using System;

namespace Adrenak.UGX {
    /// <summary>
    /// The fundamental class used to define anything that is visible
    /// to the user. 
    /// </summary>
    [DisallowMultipleComponent]
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UGXBehaviour { }
}