using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    public interface ITransitionDriver {
        UniTask TransitionPosition(RectTransform rt, Vector3 endValue, TransitionArgs tween);
        void TransitionPosition(RectTransform rt, Vector3 endValue, TransitionArgs tween, Action onComplete);

        UniTask TransitionOpacity(CanvasGroup group, float endValue, TransitionArgs tween);
        void TransitionOpacity(CanvasGroup group, float endValue, TransitionArgs tween, Action onComplete);
    }
}