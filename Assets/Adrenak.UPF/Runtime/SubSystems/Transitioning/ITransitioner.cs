using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public interface ITransitioner {
        Task TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransition tween);
        void TransitionPosition(RectTransform rt, Vector3 endValue, PositionTransition tween, Action onComplete);
        Task TransitionOpacity(CanvasGroup group, float endValue, OpacityTransition tween);
        void TransitionOpacity(CanvasGroup group, float endValue, OpacityTransition tween, Action onComplete);
    }
}