using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public interface ITweener {
        Task TweenPosition(RectTransform rt, Vector3 endValue, PositionTween tween);
        void TweenPosition(RectTransform rt, Vector3 endValue, PositionTween tween, Action onComplete);
        Task TweenOpacity(CanvasGroup group, float endValue, OpacityTween tween);
        void TweenOpacity(CanvasGroup group, float endValue, OpacityTween tween, Action onComplete);
    }
}
