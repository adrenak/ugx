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

    public class NotificationPopup : Popup<NotificationPopupState, PopupResponse> {
        [SerializeField] Text title = null;
        [SerializeField] Text description = null;

        async public override UniTask<PopupResponse> WaitForResponse() {              
            await UniTask.Delay(MyViewState.delay, DelayType.DeltaTime, PlayerLoopTiming.Update);
            return new PopupResponse();
        }

        protected override void HandleViewStateSet() {
            title.text = MyViewState.title;
            description.text = MyViewState.description;
        }
    }
}