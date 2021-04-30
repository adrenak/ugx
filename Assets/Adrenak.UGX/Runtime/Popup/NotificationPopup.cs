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

        async protected override UniTask<PopupResponse> GetResponse() {              
            await UniTask.Delay(State.delay, DelayType.DeltaTime, PlayerLoopTiming.Update);
            return new PopupResponse();
        }

        protected override void OnStateSet() {
            title.text = State.title;
            description.text = State.description;
        }

        protected override void OnStart() { }
    }
}