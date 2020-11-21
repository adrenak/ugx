using System.Threading;

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class NotificationPopup : Window {
#pragma warning disable 0649
        [SerializeField] Text title;
        [SerializeField] Text description;
        [SerializeField] float delay;
#pragma warning restore 0649

        CancellationTokenSource source;

        public async void Show(string _title, string _description, float _delay = -1) {
            if (source != null) source.Cancel();

            onWindowOpen?.Invoke();
            title.text = _title;
            description.text = _description;

            source = new CancellationTokenSource();
            if (_delay == -1)
                _delay = delay;

            try {
                await UniTask.Delay((int)(_delay * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, source.Token);
                source = null;
                onWindowClose?.Invoke();
            }
            catch { }
        }
    }
}