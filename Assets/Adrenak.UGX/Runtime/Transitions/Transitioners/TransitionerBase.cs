using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    public abstract class TransitionerBase : UGXBehaviour {
        static ITransitionDriver driver;

        public static bool DriverLocked => driver != null;
        public static ITransitionDriver Driver {
            get {
                if (driver == null)
                    driver = new SurgeTransitionDriver();
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

        [Button]
        async public void TransitionIn() => await TransitionInAsync();
        public abstract UniTask TransitionInAsync();

        [Button]
        async public void TransitionOut() => await TransitionOutAsync();
        public abstract UniTask TransitionOutAsync();
    }
}
