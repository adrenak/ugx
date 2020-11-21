using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    public interface ITransitioner {
        UniTask TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransitionArgs tween);
        void TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransitionArgs tween, Action onComplete);
        UniTask TransitionOpacity(CanvasGroup group, float endValue, OpacityTransitionArgs tween);
        void TransitionOpacity(CanvasGroup group, float endValue, OpacityTransitionArgs tween, Action onComplete);
    }
}