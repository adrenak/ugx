using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Tweens the opacity of a UI element using a <see cref="CanvasGroup"/>
    /// component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupOpacityTweener : TweenerBase {
        CanvasGroup cg;
        public CanvasGroup CG {
            get {
                if (cg == null)
                    cg = GetComponent<CanvasGroup>();
                return cg;
            }
        }

        /// <summary>
        /// Sets the tweening progress to any value between 0 and 1.
        /// Use this to have manual control over the opacity.
        /// </summary>
        /// <param name="value"></param>
        protected override void SetProgress(float value) {
            CG.alpha = value;
            CG.blocksRaycasts = !Mathf.Approximately(value, 0f);
            CG.interactable = !Mathf.Approximately(value, 0f);
        }
    }
}
