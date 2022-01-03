using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for popups with no return type.
    /// Inherit from this class to implement different kinds of popups.
    /// </summary>
    [RequireComponent(typeof(Window))]
    public abstract class Popup<T> : Popup<T, UniTask> where T : State { }
}
