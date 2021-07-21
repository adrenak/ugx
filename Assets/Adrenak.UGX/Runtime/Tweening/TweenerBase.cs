using UnityEngine;
using Cysharp.Threading.Tasks;

#if UGX_NAUGHTY_AVAILABLE
using NaughtyAttributes;
#endif

namespace Adrenak.UGX {
    public abstract class TweenerBase : UGXBehaviour {
        static ITweenDriver driver;

        // Ambient context dependency pattern. Set driver
        // implementation before ever using it. Best to set 
        // it at the start of the application.
        public static bool IsDriverLocked => driver != null;
        public static ITweenDriver Driver {
            get {
                if (driver == null)
                    driver = new SurgeTweenDriver();
                return driver;
            }
            set {
                if (driver != null)
                    Debug.LogError("BaseTransitioner.Driver can only be set once and before any get calls");

                if (value == null)
                    Debug.LogError("BaseTransitioner.Driver cannot set Driver to null!");

                driver = value;
            }
        }

        [SerializeField]
#if UGX_NAUGHTY_AVAILABLE
        [ReadOnly]
#endif
        float progress;

        public float Progress {
            get => progress;
            set {
                progress = value;
                OnProgressChanged(value);
            }
        }

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Tweening Args")]
#endif
        [SerializeField]
        public bool useSameArgsForInAndOut = true;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Tweening Args")] [ShowIf("useSameArgsForInAndOut")]
#endif
        public TweenArgs args;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Tweening Args")] [HideIf("useSameArgsForInAndOut")]
#endif
        public TweenArgs inArgs;

#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Tweening Args")] [HideIf("useSameArgsForInAndOut")]
#endif
        public TweenArgs outArgs;

#if UGX_NAUGHTY_AVAILABLE
        [Button]
#endif
        async public void TransitionIn() => await TransitionInAsync();
        public abstract UniTask TransitionInAsync();

        [Button]
        async public void TransitionOut() => await TransitionOutAsync();
        public abstract UniTask TransitionOutAsync();

        protected abstract void OnProgressChanged(float value);
    }
}
