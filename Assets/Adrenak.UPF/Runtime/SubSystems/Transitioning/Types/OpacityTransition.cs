using UnityEngine;

namespace Adrenak.UPF {
    [System.Serializable]
    public class OpacityTransition {
        [SerializeField] float duration = 1;
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

        public OpacityTransition(float duration){
            Duration = duration;
        }

        public OpacityTransition SetDelay(float delay){
            Delay = delay;
            return this;
        }

        public OpacityTransition SetCurve(CurveType curve) {
            Curve = curve;
            return this;
        }

        public OpacityTransition SetLoop(LoopType loop){
            Loop = loop;
            return this;
        }
    }
}
