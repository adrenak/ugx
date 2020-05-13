using UnityEngine;

namespace Adrenak.UPF {
    [System.Serializable]
    public class PositionTween {
        [SerializeField] float duration;
        public float Duration {
            get => duration;
            private set => duration = value;
        }

        [SerializeField] float delay;
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
        
        public PositionTween(float duration) {
            Duration = duration;
        }

        public PositionTween SetDelay(float delay) {
            Delay = delay;
            return this;
        }

        public PositionTween SetCurve(CurveType curve){
            Curve = curve;
            return this;
        }

        public PositionTween SetLoop(LoopType loop){
            Loop = loop;
            return this;
        }
    }
}

