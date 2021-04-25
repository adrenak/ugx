using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    public interface ITweenDriver {
        UniTask TransitionPosition(RectTransform rt, Vector3 endValue, TweenArgs tween);
        void TransitionPosition(RectTransform rt, Vector3 endValue, TweenArgs tween, Action onComplete);

        UniTask TransitionOpacity(CanvasGroup group, float endValue, TweenArgs tween);
        void TransitionOpacity(CanvasGroup group, float endValue, TweenArgs tween, Action onComplete);
    }
}