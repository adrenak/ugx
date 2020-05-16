using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class PairFlow : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] protected PageView first;
        [SerializeField] protected PageView second;

        [SerializeField] bool showSecondOnStart;
        [ReadOnly] [SerializeField] bool isShowingSecond;
#pragma warning restore 0649

        public PageView First => first;
        public PageView Second => second;
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

        void Appear(PageView view) {
            if (view.IsOpen) return;
            view.OpenPage();
        }

        void Disappear(PageView view) {
            if (!view.IsOpen) return;
            view.ClosePage();
        }
    }
}
