using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for popups with no return type or state type.
    /// Inherit from this class to implement different kinds of popups.
    /// </summary>
    [RequireComponent(typeof(Window))]
    public class StaticPopup : Popup<State> {
        async protected override UniTask<UniTask> GetResponse() {
            responded = false;
            while (!responded)
                await UniTask.Delay(100);
            return UniTask.CompletedTask;
        }

        bool responded;
        public void Close() => responded = true;

        protected override void OnInitializeView() { }
        protected override void OnUpdateView() { }
    }
}
