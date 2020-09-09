using Adrenak.Unex;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class ToastPopup : Window {
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
                await Task.Delay((int)(_delay * 1000), source.Token);
                source = null;
                onWindowClose?.Invoke();
            }
            catch { }
        }
    }
}
