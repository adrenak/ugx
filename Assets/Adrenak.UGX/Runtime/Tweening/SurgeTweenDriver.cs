using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Adrenak.UGX {
    public class SurgeTweenDriver : ITweenDriver {
        public UniTask TransitionPosition(RectTransform rt, Vector3 endValue, TweenArgs tween) {
            var pSource = new UniTaskCompletionSource<bool>();
            TransitionPosition(rt, endValue, tween, () => pSource.TrySetResult(true));
            return pSource.Task;
        }

        public void TransitionPosition(RectTransform rt, Vector3 endValue, TweenArgs tween, Action onComplete) {
            Tween.Value(
                rt.localPosition,
                endValue,
                v => rt.localPosition = v,
                Mathf.Clamp(tween.Duration, 1f / int.MaxValue, Mathf.Infinity), // Make sure duration is not 0 else tweener jumps to final value
                tween.Delay,
                loop: Convert(tween.Loop),
                easeCurve: Convert(tween.Curve),
                completeCallback: () => onComplete?.Invoke()
            );
        }

        public UniTask TransitionOpacity(CanvasGroup group, float endValue, TweenArgs tween) {
            var oSource = new UniTaskCompletionSource<bool>();
            TransitionOpacity(group, endValue, tween, () => oSource.TrySetResult(true));
            return oSource.Task;
        }

        public void TransitionOpacity(CanvasGroup group, float endValue, TweenArgs tween, Action onComplete) {
            Tween.CanvasGroupAlpha(
                group,
                endValue,
                Mathf.Clamp(tween.Duration, 1f / int.MaxValue, Mathf.Infinity), // Make sure duration is not 0 else tweener jumps to final value
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
