using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    public class TweenStyle {
        [SerializeField] float duration;
        public float Duration {
            get => duration;
            set => duration = value;
        }

        [SerializeField] float delay = 0;
        public float Delay {
            get => delay;
            set => delay = value;
        }

        [SerializeField] CurveType curve = CurveType.None;
        public CurveType Curve {
            get => curve;
            set => curve = value;
        }

        [SerializeField] LoopType loop = LoopType.None;
        public LoopType Loop {
            get => loop;
            set => loop = value;
        }
    }
}

