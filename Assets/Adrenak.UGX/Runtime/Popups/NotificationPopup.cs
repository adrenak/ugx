using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [System.Serializable]
    public class NotificationPopupState : PopupState {
        public string title;
        public string description;
        public int delay = 3;
    }

    public class NotificationPopup : Popup<NotificationPopupState, PopupResult> {
        [SerializeField] Text title = null;
        [SerializeField] Text description = null;

        async public override UniTask<PopupResult> WaitForResponse() {              
            await UniTask.Delay(MyViewState.delay, DelayType.DeltaTime, PlayerLoopTiming.Update);
            return new PopupResult();
        }

        protected override void HandleViewStateSet() {
            title.text = MyViewState.title;
            description.text = MyViewState.description;
        }
    }
}