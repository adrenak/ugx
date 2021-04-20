using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    public class TweenArgs {
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

        public TweenArgs(float duration) {
            Duration = duration;
        }

        public TweenArgs SetDelay(float delay) {
            Delay = delay;
            return this;
        }

        public TweenArgs SetCurve(CurveType curve) {
            Curve = curve;
            return this;
        }

        public TweenArgs SetLoop(LoopType loop) {
            Loop = loop;
            return this;
        }
    }
}

