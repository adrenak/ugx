using UnityEngine;
using Cysharp.Threading.Tasks;

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
                    Debug.LogError("Driver can only be set once and before any get calls");

                if (value == null)
                    Debug.LogError("Cannot set driver to null!");

                driver = value;
            }
        }

        [SerializeField] float progress;
        public float Progress {
            get => progress;
            set {
                progress = value;
                OnProgressChanged(value);
            }
        }

        [SerializeField] public bool useSameArgsForInAndOut = true;
        public TweenArgs args;
        public TweenArgs inArgs;
        public TweenArgs outArgs;

        async public void TweenIn() => await TweenInAsync();
        public abstract UniTask TweenInAsync();

        async public void TweenOut() => await TweenOutAsync();
        public abstract UniTask TweenOutAsync();

        protected abstract void OnProgressChanged(float value);
    }
}
