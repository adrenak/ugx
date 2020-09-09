using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    public class PositionTransitionArgs {
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

        public PositionTransitionArgs(float duration) {
            Duration = duration;
        }

        public PositionTransitionArgs SetDelay(float delay) {
            Delay = delay;
            return this;
        }

        public PositionTransitionArgs SetCurve(CurveType curve) {
            Curve = curve;
            return this;
        }

        public PositionTransitionArgs SetLoop(LoopType loop) {
            Loop = loop;
            return this;
        }
    }
}

