using Cysharp.Threading.Tasks;

using UnityEngine;

#if UGX_NAUGHTY_AVAILABLE
using NaughtyAttributes;
#endif

namespace Adrenak.UGX {
    [DisallowMultipleComponent]
    public class PositionTweener : TweenerBase {
        public enum PositionType {
            Top,
            Bottom,
            Left,
            Right
        }

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Positions")] [ReadOnly]
#endif
        [SerializeField] Vector3 inPosition;
        public Vector3 InPosition => inPosition;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Positions")] [ReadOnly]
#endif
        [SerializeField] Vector3 outPosition;
        public Vector3 OutPosition => outPosition;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Config")]
#endif
        public PositionType enterPosition = PositionType.Left;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Config")]
#endif
        public PositionType exitPosition = PositionType.Right;

#if UGX_NAUGHTY_AVAILABLE
        [Button("Set As In")]
#endif
        public void CaptureInPosition() => inPosition = RT.localPosition;

#if UGX_NAUGHTY_AVAILABLE
        [Button("Set As Out")]
#endif
        public void CaptureOutPosition() => outPosition = RT.localPosition;

        override async public UniTask TransitionInAsync() {
            if (!Application.isPlaying) {
                RT.localPosition = InPosition;
                return;
            }
            RT.localPosition = DefaultEnterCordinates;
            if (useSameArgsForInAndOut)
                await TweenPosition(inPosition, args);
            else
                await TweenPosition(inPosition, inArgs);
            RT.localPosition = InPosition;
        }

        override async public UniTask TransitionOutAsync() {
            if (!Application.isPlaying) {
                RT.localPosition = OutPosition;
                return;
            }
            RT.localPosition = InPosition;
            if(useSameArgsForInAndOut)
                await TweenPosition(DefaultExitCordinates, args);
            else
                await TweenPosition(DefaultExitCordinates, outArgs);
            RT.localPosition = OutPosition;
        }

        async public UniTask TweenPosition(Vector3 endValue, TweenArgs tween)
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

        protected override void OnProgressChanged(float value) {
            RT.localPosition = Vector3.Lerp(OutPosition, InPosition, value);
            if (value == 0f)
                RT.localPosition = OutPosition;
            else if (value == 1f)
                RT.localPosition = InPosition;
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