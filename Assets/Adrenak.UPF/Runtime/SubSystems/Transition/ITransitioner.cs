using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public interface ITransitioner {
        Task TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransitionArgs tween);
        void TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransitionArgs tween, Action onComplete);
        Task TransitionOpacity(CanvasGroup group, float endValue, OpacityTransitionArgs tween);
        void TransitionOpacity(CanvasGroup group, float endValue, OpacityTransitionArgs tween, Action onComplete);
    }
}