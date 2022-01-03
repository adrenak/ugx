using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [System.Serializable]
    public class NotificationPopupState : State {
        public string title;
        public string body;
        public int delay = 3;
    }

    public class NotificationPopup : Popup<NotificationPopupState> {
        [SerializeField] Text title = null;
        [SerializeField] Text description = null;

        async protected override UniTask<UniTask> GetResponse() {
            await UniTask.Delay(State.delay * 1000, DelayType.DeltaTime, PlayerLoopTiming.Update);
            return UniTask.CompletedTask;
        }

        protected override void OnRefresh() {
            title.text = State.title;
            description.text = State.body;
        }

        protected override void OnStart() { }
    }
}