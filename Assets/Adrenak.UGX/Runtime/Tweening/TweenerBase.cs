using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for creating tweening behaviour.
    /// </summary>
    public abstract class TweenerBase : UGXBehaviour {
        // Ambient context dependency pattern. Set driver
        // implementation before ever using it. Best to set 
        // it at the start of the application.
        static ITweenDriver driver;

        public static bool IsDriverLocked => driver != null;

        /// <summary>
        /// The <see cref="ITweenDriver"/> implementation to use used.
        /// Defaults to <see cref="SurgeTweenDriver"/>
        /// </summary>
        public static ITweenDriver Driver {
            get {
                if (driver == null)
                    driver = new SurgeTweenDriver();
                return driver;
            }
            set {
                if (driver != null){
                    var msg = "Driver can only be set once and " +
                    "before any get calls";
                    Debug.LogError(msg);
                }

                if (value == null)
                    Debug.LogError("Cannot set driver to null!");

                driver = value;
            }
        }

        float lastProgress = -1;

        /// <summary>
        /// The tween progress (from 0 to 1)
        /// </summary>
        public float progress;

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
        async public void TweenIn() {
            if (Application.isPlaying)
                await TweenInAsync();
            else
                SetProgress(1);
        }

        /// <summary>
        /// Tweens out. When not in play mode, instantly jumps to progress 0
        /// </summary>
        async public void TweenOut() {
            if (Application.isPlaying)
                await TweenOutAsync();
            else
                SetProgress(0);
        }

        /// <summary>
        /// Awaitable Tween in from 0 to 1 
        /// </summary>
        public abstract UniTask TweenInAsync();

        /// <summary>
        /// Awaitable Tween out from 1 to 0
        /// </summary>
        public abstract UniTask TweenOutAsync();

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
    }
}
