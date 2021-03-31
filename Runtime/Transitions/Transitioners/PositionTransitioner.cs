using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Adrenak.UGX {
    public class PositionTransitioner : TransitionerBase {
        [ReadOnly] [SerializeField] Vector3 inPosition;
        public Vector3 InPosition => inPosition;

        [ReadOnly] [SerializeField] Vector3 outPosition;
        public Vector3 OutPosition => outPosition;

        public PositionType defaultPositionForEnter = PositionType.Left;
        public PositionType defaultPositionForExit = PositionType.Right;
        public PositionTransitionArgs defaultPositionTween;

        [Button("Set As In")]
        public void CaptureInPosition() => inPosition = RT.localPosition;

        [Button("Set As Out")]
        public void CaptureOutPosition() => outPosition = RT.localPosition;

        [Button("Move In")]
        async public void MoveIn() => await MoveInAwaitable();
        async public UniTask MoveInAwaitable() {
            if (!Application.isPlaying) {
                RT.localPosition = InPosition;
                return;
            }
            RT.localPosition = DefaultEnterCordinates;
            await TweenPosition(inPosition, defaultPositionTween);
            RT.localPosition = InPosition;
        }

        [Button("Move Out")]
        async public void MoveOut() => await MoveOutAwaitable();
        async public UniTask MoveOutAwaitable() {
            if (!Application.isPlaying) {
                RT.localPosition = OutPosition;
                return;
            }
            RT.localPosition = InPosition;
            await TweenPosition(DefaultExitCordinates, defaultPositionTween);
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
            => GetPositionVector3(defaultPositionForExit);

        public Vector3 DefaultEnterCordinates
            => GetPositionVector3(defaultPositionForEnter);

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