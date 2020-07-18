using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class PairFlow : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] protected Page first;
        [SerializeField] protected Page second;

        [SerializeField] bool showSecondOnStart;
        [ReadOnly] [SerializeField] bool isShowingSecond;
#pragma warning restore 0649

        public Page First => first;
        public Page Second => second;
        public bool IsShowingSecond {
            get => isShowingSecond;
            set {
                isShowingSecond = value;
                if (value) {
                    Appear(second);
                    Disappear(first);
                }
                else {
                    Appear(first);
                    Disappear(second);
                }
            }
        }

        void Start() {
            IsShowingSecond = showSecondOnStart;
        }

        void Appear(Page view) {
            if (view.IsOpen) return;
            view.OpenPage();
        }

        void Disappear(Page view) {
            if (!view.IsOpen) return;
            view.ClosePage();
        }
    }
}
