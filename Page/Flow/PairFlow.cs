using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public abstract class PairFlow<TMasterPage, TDetailPage> : PairFlow where TMasterPage : PageView where TDetailPage : PageView {
        new public TMasterPage Master => master as TMasterPage;
        new public TDetailPage Detail => detail as TDetailPage;
    }

    public class PairFlow : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] protected PageView master;
        [SerializeField] protected PageView detail;

        [SerializeField] Navigator navigator;
        
        [SerializeField] bool isDetailPageOpenOnStart;
        [ReadOnly] [SerializeField] bool isDetailPageOpen;
#pragma warning restore 0649

        public PageView Master => master;
        public PageView Detail => detail;
        public bool IsDetailPageOpen {
            get => isDetailPageOpen;
            set {
                if (value == IsDetailPageOpen) return;

                isDetailPageOpen = value;
                if (value)
                    navigator.Push(Detail);
                else
                    navigator.Pop();
            }
        }

        void Start() {
            IsDetailPageOpen = isDetailPageOpenOnStart;
        }
    }
}
