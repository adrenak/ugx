using System;

using UnityEngine;

namespace Adrenak.UPF {
    public interface ITweener {
        void TweenPosition(RectTransform rt, Vector3 endValue, PositionTween tween, Action onComplete);
        void TweenOpacity(CanvasGroup group, float endValue, OpacityTween tween, Action onComplete);
    }
}
