using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Adrenak.UGX {
    public class PositionTransitioner : TransitionerBase {
        [BoxGroup("Positions")] [ReadOnly] [SerializeField] Vector3 inPosition;
        public Vector3 InPosition => inPosition;

        [BoxGroup("Positions")] [ReadOnly] [SerializeField] Vector3 outPosition;
        public Vector3 OutPosition => outPosition;

        [BoxGroup("Config")] public PositionTransitionArgs args;
        [BoxGroup("Config")] public PositionType enterPosition = PositionType.Left;
        [BoxGroup("Config")] public PositionType exitPosition = PositionType.Right;

        [Button("Set As In")]
        public void CaptureInPosition() => inPosition = RT.localPosition;

        [Button("Set As Out")]
        public void CaptureOutPosition() => outPosition = RT.localPosition;

        override async public UniTask TransitionInAsync() {
            if (!Application.isPlaying) {
                RT.localPosition = InPosition;
                return;
            }
            RT.localPosition = DefaultEnterCordinates;
            await TweenPosition(inPosition, args);
            RT.localPosition = InPosition;
        }

        override async public UniTask TransitionOutAsync() {
            if (!Application.isPlaying) {
                RT.localPosition = OutPosition;
                return;
            }
            RT.localPosition = InPosition;
            await TweenPosition(DefaultExitCordinates, args);
            RT.localPosition = OutPosition;
        }

        async public UniTask TweenPosition(Vector3 endValue, PositionTransitionArgs tween)
            => await Driver.TransitionPosition(RT, endValue, tween);

        public Vector3 GetPositionVector3(PositionType position) {
            switch (position) {
                case PositionType.Left: return LeftExitCordinates;
                case PositionType.Right: return RightExitCordinates;
                case PositionType.Top: return TopExitCordinates;
                case PositionType.Bottom: return BottomExitCordinates;
                default: return Vector3.zero;
            }
        }

        public Vector3 DefaultExitCordinates
            => GetPositionVector3(exitPosition);

        public Vector3 DefaultEnterCordinates
            => GetPositionVector3(enterPosition);

        public Vector3 RightExitCordinates
            => new Vector3(RT.GetRightExit(), inPosition.y, RT.localPosition.z);

        public Vector3 TopExitCordinates
            => new Vector3(inPosition.x, RT.GetTopExit(), RT.localPosition.z);

        public Vector3 LeftExitCordinates
            => new Vector3(RT.GetLeftExit(), inPosition.y, RT.localPosition.z);

        public Vector3 BottomExitCordinates
            => new Vector3(inPosition.x, RT.GetBottomExit(), RT.localPosition.z);
    }
}