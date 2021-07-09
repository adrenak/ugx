using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(Window))]
    public class Popup : Popup<ViewModel> {
        async protected override UniTask<UniTask> GetResponse() {
            responded = false;
            while (!responded)
                await UniTask.Delay(100);
            return UniTask.CompletedTask;
        }

        bool responded;
        public void Close() => responded = true;

        protected override void OnViewModelUpdate() { }
    }
}
