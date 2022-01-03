using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    public interface ITweenDriver {
        UniTask TweenToPosition(RectTransform rt, Vector3 to, TweenStyle tween);
        void TweenToPosition(RectTransform rt, Vector3 to, TweenStyle tween, Action callback);

        UniTask TweenToOpacity(CanvasGroup cg, float to, TweenStyle tween);
        void TweenToOpacity(CanvasGroup cg, float to, TweenStyle tween, Action callback);
    }
}