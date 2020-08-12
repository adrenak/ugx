using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class PairFlow : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] protected View first;
        [SerializeField] protected View second;

        [SerializeField] bool showSecondOnStart;
        [ReadOnly] [SerializeField] bool isShowingSecond;
#pragma warning restore 0649

        public View First => first;
        public View Second => second;
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

        void Appear(View view) {
            if (view.IsOpen) return;
            view.OpenView();
        }

        void Disappear(View view) {
            if (!view.IsOpen) return;
            view.CloseView();
        }
    }
}
