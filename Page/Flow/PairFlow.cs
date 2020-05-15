using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class PairFlow : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] protected PageView first;
        [SerializeField] protected PageView second;

        [SerializeField] Navigator navigator;

        [SerializeField] bool showSecondOnStart;
        [ReadOnly] [SerializeField] bool isShowingSecond;
#pragma warning restore 0649

        public PageView First => first;
        public PageView Second => second;
        public bool IsShowingSecond {
            get => isShowingSecond;
            set {
                if (value == IsShowingSecond) return;

                isShowingSecond = value;
                if (value)
                    navigator.Push(Second);
                else
                    navigator.Pop();
            }
        }

        void Start() {
            isShowingSecond = showSecondOnStart;
            if (IsShowingSecond)
                navigator.Push(second);
            else
                navigator.Push(first);
        }
    }
}
