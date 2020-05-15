using System;
using System.Threading.Tasks;

using UnityEngine;

using Pixelplacement;

namespace Adrenak.UPF {
    public class SurgeTweener : ITweener {
        public Task TweenPosition(RectTransform rt, Vector3 endValue, PositionTween tween) {
            var source = new TaskCompletionSource<bool>();
            TweenPosition(rt, endValue, tween, () => source.SetResult(true));
            return source.Task;
        }

        public void TweenPosition(RectTransform rt,Vector3 endValue, PositionTween tween, Action onComplete) {
            Tween.Value(
                rt.localPosition,
                endValue,
                v => rt.localPosition = v,
                tween.Duration,
                tween.Delay,
                loop: Convert(tween.Loop),
                easeCurve: Convert(tween.Curve),
                completeCallback: () => onComplete?.Invoke()
            );
        }

        public Task TweenOpacity(CanvasGroup group, float endValue, OpacityTween tween) {
            var source = new TaskCompletionSource<bool>();
            TweenOpacity(group, endValue, tween, () => source.SetResult(true));
            return source.Task;
        }

        public void TweenOpacity(CanvasGroup group, float endValue, OpacityTween tween, Action onComplete) {
            Tween.CanvasGroupAlpha(
                group,
                endValue,
                tween.Duration,
                tween.Delay,
                easeCurve: Convert(tween.Curve),
                loop: Convert(tween.Loop),
                completeCallback: () => onComplete?.Invoke()
            );
        }

        public static Tween.LoopType Convert(LoopType loop) {
            return (Tween.LoopType)(int)loop;
        }

        public static AnimationCurve Convert(CurveType curve) {
            switch (curve) {
                case CurveType.EaseBounce:
                    return Tween.EaseBounce;
                case CurveType.EaseIn:
                    return Tween.EaseIn;
                case CurveType.EaseInBack:
                    return Tween.EaseInBack;
                case CurveType.EaseInOut:
                    return Tween.EaseInOut;
                case CurveType.EaseInOutBack:
                    return Tween.EaseInOutBack;
                case CurveType.EaseInOutStrong:
                    return Tween.EaseInOutStrong;
                case CurveType.EaseInStrong:
                    return Tween.EaseInStrong;
                case CurveType.EaseLinear:
                    return Tween.EaseLinear;
                case CurveType.EaseOut:
                    return Tween.EaseOut;
                case CurveType.EaseOutBack:
                    return Tween.EaseOutBack;
                case CurveType.EaseOutStrong:
                    return Tween.EaseOutStrong;
                case CurveType.EaseSpring:
                    return Tween.EaseSpring;
                case CurveType.EaseWobble:
                    return Tween.EaseWobble;
                default:
                    return null;
            }
        }
    }
}
