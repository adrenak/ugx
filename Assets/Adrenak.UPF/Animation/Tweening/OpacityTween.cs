using UnityEngine;

namespace Adrenak.UPF{
    [System.Serializable]
    public class OpacityTween {
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

        public OpacityTween(float duration){
            Duration = duration;
        }

        public OpacityTween SetDelay(float delay){
            Delay = delay;
            return this;
        }

        public OpacityTween SetCurve(CurveType curve) {
            Curve = curve;
            return this;
        }

        public OpacityTween SetLoop(LoopType loop){
            Loop = loop;
            return this;
        }
    }
}
