using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Pixelplacement;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for creating tweening behaviour.
    /// </summary>
    public abstract class TweenerBase : UGXBehaviour {
        float lastProgress = -1;

        /// <summary>
        /// The tween progress (from 0 to 1)
        /// </summary>
        public float progress;

        /// <summary>
        /// If set to true, while tweening progress will always go
        /// from 0 to 1 for IN and 1 to 0 for OUT regardless of 
        /// current progress value. This will likely cause a disruptive
        /// UI experience, hence set to false.
        /// </summary>
        [Tooltip("If set to true, while tweening progress will always go" +
        "from 0 to 1 for IN and 1 to 0 for OUT regardless of " +
        "current progress value. This will likely cause a disruptive" +
        "UI experience, hence set to false.")]
        public bool tweenEndToEnd = false;

        /// <summary>
        /// Should same tween style be used for in and out tweens.
        /// </summary>
        [Tooltip("Should same tween style be used for in and out tweens.")]
        public bool useSameStyleForInAndOut = true;

        /// <summary>
        /// The tween style to be used for both in and out tweens.
        /// </summary>
        [Tooltip("The tween style to be used for both in and out tweens.")]
        public TweenStyle commonStyle;

        /// <summary>
        /// The tween style to be used for in tweens.
        /// </summary>
        [Tooltip("The tween style to be used for in tweens.")]
        public TweenStyle inStyle;

        /// <summary>
        /// The tween style to be used for out tweens.
        /// </summary>
        [Tooltip("The tween style to be used for out tweens.")]
        public TweenStyle outStyle;

        /// <summary>
        /// Tweens in. When not in play mode, instantly jumps to progress 1
        /// </summary>
        async public void TweenIn(CancellationToken token = default) {
            if (Application.isPlaying)
                await TweenInAsync(token);
            else
                SetProgress(1);
        }

        /// <summary>
        /// Tweens out. When not in play mode, instantly jumps to progress 0
        /// </summary>
        async public void TweenOut(CancellationToken token = default) {
            if (Application.isPlaying)
                await TweenOutAsync(token);
            else
                SetProgress(0);
        }

        /// <summary>
        /// Awaitable Tween in from 0 to 1 
        /// </summary>
        public UniTask TweenInAsync(CancellationToken token = default) {
            var source = new UniTaskCompletionSource();
            var style = useSameStyleForInAndOut ? commonStyle : inStyle;
            var tweenBase = Tween.Value(tweenEndToEnd ? 0 : progress, 1f,
                x => SetProgressInternal(x),
                style.Duration + .001f,
                style.Delay + .001f,
                Convert(style.Curve),
                Convert(style.Loop),
                () => { },
                () => {
                    SetProgressInternal(1);
                    source.TrySetResult();
                }
            );
            if (token != default)
                token.Register(() => tweenBase.Stop());

            return source.Task;
        }

        /// <summary>
        /// Awaitable Tween out from 1 to 0
        /// </summary>
        public UniTask TweenOutAsync(CancellationToken token = default) {
            var source = new UniTaskCompletionSource();
            var style = useSameStyleForInAndOut ? commonStyle : outStyle;
            var tweenBase = Tween.Value(tweenEndToEnd ? 1 : progress, 0f,
                y => SetProgressInternal(y),
                style.Duration + .001f,
                style.Delay + .001f,
                Convert(style.Curve),
                Convert(style.Loop),
                () => { },
                () => {
                    SetProgressInternal(0);
                    source.TrySetResult();
                }
            );
            if(token != default)
                token.Register(() => tweenBase.Stop());
            return source.Task;
        }

        void SetProgressInternal(float value) {
            progress = value;
            SetProgress(value);
        }

        /// <summary>
        /// Implement in derived class for setting 
        /// the tween progress between 0 and 1.
        /// </summary>
        /// <param name="value"></param>
        protected abstract void SetProgress(float value);

        /// <summary>
        /// We check if any change in <see cref="progress"/> has taken place 
        /// and call <see cref="SetProgress(float)"/> if yes.
        /// </summary>
        void Update() {
            progress = Mathf.Clamp01(progress);
            if (lastProgress != progress)
                SetProgress(progress);
            lastProgress = progress;
        }


        public static Tween.LoopType Convert(LoopType loop) {
            return (Tween.LoopType)(int)loop;
        }

        public static AnimationCurve Convert(CurveType curve) {
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
