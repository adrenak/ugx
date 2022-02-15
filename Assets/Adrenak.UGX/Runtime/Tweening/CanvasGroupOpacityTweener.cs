using Cysharp.Threading.Tasks;
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
        public CanvasGroup CG => cg ?? (cg = GetComponent<CanvasGroup>());

        /// <summary>
        /// Tweens the opacity of <see cref="CanvasGroup"/> from 0 to 1.
        /// is non raycast blocking and non interactable before the tweening
        /// starts until the tweening ends
        /// </summary>
        /// <returns></returns>
        override async public UniTask TweenInAsync() {
            CG.alpha = 0;
            CG.blocksRaycasts = false;
            CG.interactable = false;
            var styleToUse = useSameStyleForInAndOut ? commonStyle : inStyle;
            await TweenToOpacity(1, styleToUse);
            CG.alpha = 1;
            CG.blocksRaycasts = true;
            CG.interactable = true;
        }

        /// <summary>
        /// Tweens the opacity of <see cref="CanvasGroup"/> from 1 to 0.
        /// becomes non raycast blocking and non interactable once called.
        /// </summary>
        /// <returns></returns>
        override async public UniTask TweenOutAsync() {
            CG.alpha = 1;
            CG.blocksRaycasts = false;
            CG.interactable = false;
            var styleToUse = useSameStyleForInAndOut ? commonStyle : outStyle;
            await TweenToOpacity(1, styleToUse);
            CG.alpha = 0;
        }

        /// <summary>
        /// Sets the tweening progress to any value between 0 and 1.
        /// Use this to have manual control over the opacity.
        /// </summary>
        /// <param name="value"></param>
        protected override void SetProgress(float value) {
            CG.alpha = value;
            CG.blocksRaycasts = value != 0f;
            CG.interactable = value != 0f;
        }

        /// <summary>
        /// Tweens the opacity to an end value using the 
        /// provided <see cref="TweenStyle"/>
        /// </summary>
        /// <param name="to">The alpha value to tween to</param>
        /// <param name="style">The tween style to be used</param>
        /// <returns></returns>
        async public UniTask TweenToOpacity(float to, TweenStyle style)
            => await Driver.TweenToOpacity(CG, to, style);
    }
}
