using System;
using System.Threading.Tasks;

using UnityEngine;

using Pixelplacement;

namespace Adrenak.UPF {
    public class SurgeTransitioner : ITransitioner {
        public Task TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransition tween) {
            var source = new TaskCompletionSource<bool>();
            TransitionPosition(rt, endValue, tween, () => source.SetResult(true));
            return source.Task;
        }

        public void TransitionPosition(RectTransform rt,Vector3 endValue, PositionTransition tween, Action onComplete) {
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

        public Task TransitionOpacity(CanvasGroup group, float endValue, OpacityTransition tween) {
            var source = new TaskCompletionSource<bool>();
            TransitionOpacity(group, endValue, tween, () => source.SetResult(true));
            return source.Task;
        }

        public void TransitionOpacity(CanvasGroup group, float endValue, OpacityTransition tween, Action onComplete) {
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

        static Tween.LoopType Convert(LoopType loop) {
            return (Tween.LoopType)(int)loop;
        }

        static AnimationCurve Convert(CurveType curve) {
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
