﻿using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Adrenak.UPF {
    public class PositionTransitioner : MonoBehaviour {
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

        RectTransform rt;
        public RectTransform RT => rt == null ? rt = GetComponent<RectTransform>() : rt;

        readonly ITransitioner tweener = new SurgeTransitioner();

        [Button("Move In")]
        async public void MoveInAndForget() => await MoveIn();
        async public Task MoveIn() {
            if (!Application.isPlaying) {
                RT.localPosition = InPosition;
                return;
            }
            RT.localPosition = DefaultEnterCordinates;
            await TweenPosition(inPosition, defaultPositionTween);
            RT.localPosition = InPosition;
        }

        [Button("Move Out")]
        async public void MoveOutAndForget() => await MoveOut();
        async public Task MoveOut() {
            if (!Application.isPlaying) {
                RT.localPosition = OutPosition;
                return;
            }
            RT.localPosition = InPosition;
            await TweenPosition(DefaultExitCordinates, defaultPositionTween);
            RT.localPosition = OutPosition;
        }

        async public Task TweenPosition(Vector3 endValue, PositionTransitionArgs tween)
            => await tweener.TransitionPosition(RT, endValue, tween);

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