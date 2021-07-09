using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Adrenak.UGX {
    [DisallowMultipleComponent]
    public class PositionTweener : TweenerBase {
        public enum Edge {
            Top,
            Bottom,
            Left,
            Right
        }

        [SerializeField] Vector2 inPosition;
        public Vector2 InPosition => inPosition;

        [SerializeField] Vector2 outPosition;
        public Vector2 OutPosition => outPosition;

        public Edge enterEdge = Edge.Left;
        public Edge exitEdge = Edge.Right;

        public void CaptureInPosition() => inPosition = RT.anchoredPosition;

        public void CaptureOutPosition() => outPosition = RT.anchoredPosition;

        override async public UniTask TweenInAsync() {
            if (!Application.isPlaying) {
                RT.anchoredPosition = InPosition;
                return;
            }
            RT.anchoredPosition = DefaultEnterCordinates;
            if (useSameArgsForInAndOut)
                await TweenPosition(inPosition, args);
            else
                await TweenPosition(inPosition, inArgs);
            RT.anchoredPosition = InPosition;
        }

        override async public UniTask TweenOutAsync() {
            if (!Application.isPlaying) {
                RT.anchoredPosition = OutPosition;
                return;
            }
            RT.anchoredPosition = InPosition;
            if(useSameArgsForInAndOut)
                await TweenPosition(DefaultExitCordinates, args);
            else
                await TweenPosition(DefaultExitCordinates, outArgs);
            RT.anchoredPosition = OutPosition;
        }

        async public UniTask TweenPosition(Vector2 endValue, TweenArgs tween)
            => await Driver.TransitionPosition(RT, endValue, tween);

        public Vector2 GetPositionVector2(Edge position) {
            switch (position) {
                case Edge.Left: return LeftExitCordinates;
                case Edge.Right: return RightExitCordinates;
                case Edge.Top: return TopExitCordinates;
                case Edge.Bottom: return BottomExitCordinates;
                default: return Vector2.zero;
            }
        }

        protected override void OnProgressChanged(float value) {
            RT.localPosition = Vector2.Lerp(OutPosition, InPosition, value);
            if (value == 0f)
                RT.localPosition = OutPosition;
            else if (value == 1f)
                RT.localPosition = InPosition;
        }

        public Vector2 DefaultExitCordinates
            => GetPositionVector2(exitEdge);

        public Vector2 DefaultEnterCordinates
            => GetPositionVector2(enterEdge);

        public Vector2 RightExitCordinates
            => new Vector2(RT.GetRightExit(), inPosition.y);

        public Vector2 TopExitCordinates
            => new Vector2(inPosition.x, RT.GetTopExit());

        public Vector2 LeftExitCordinates
            => new Vector2(RT.GetLeftExit(), inPosition.y);

        public Vector2 BottomExitCordinates
            => new Vector2(inPosition.x, RT.GetBottomExit());
    }
}