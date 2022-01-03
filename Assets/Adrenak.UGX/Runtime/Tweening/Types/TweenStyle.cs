using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    public class TweenStyle {
        [SerializeField] float duration;
        public float Duration {
            get => duration;
            private set => duration = value;
        }

        [SerializeField] float delay = 0;
        public float Delay {
            get => delay;
            private set => delay = value;
        }

        [SerializeField] CurveType curve = CurveType.None;
        public CurveType Curve {
            get => curve;
            private set => curve = value;
        }

        [SerializeField] LoopType loop = LoopType.None;
        public LoopType Loop {
            get => loop;
            private set => loop = value;
        }
    }
}

