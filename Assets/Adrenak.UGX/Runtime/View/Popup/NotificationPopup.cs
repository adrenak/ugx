using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [System.Serializable]
    public class NotificationPopupModel : ViewModel {
        public string title;
        public string body;
        public int delay = 3;
    }

    public class NotificationPopup : Popup<NotificationPopupModel> {
        [SerializeField] Text title = null;
        [SerializeField] Text description = null;

        async protected override UniTask<UniTask> GetResponse() {
            await UniTask.Delay(Model.delay, DelayType.DeltaTime, PlayerLoopTiming.Update);
            return UniTask.CompletedTask;
        }

        protected override void OnViewModelUpdate() {
            title.text = Model.title;
            description.text = Model.body;
        }
    }
}