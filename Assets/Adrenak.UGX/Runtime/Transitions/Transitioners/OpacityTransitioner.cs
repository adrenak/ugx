using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(CanvasGroup))]
    public class OpacityTransitioner : TransitionerBase {
        public OpacityTransitionArgs defaultOpacityTween;

        CanvasGroup cg;
        public CanvasGroup CG => cg == null ? cg = GetComponent<CanvasGroup>() : cg;

        [Button]
        async public void FadeIn() => await FadeInAwaitable();
        
        async public UniTask FadeInAwaitable() {
            if (!Application.isPlaying) {
                CG.alpha = 1;
                CG.blocksRaycasts = true;
                CG.interactable = true;
                return;
            }
            CG.alpha = 0;
            await TweenOpacity(1, defaultOpacityTween);
            CG.alpha = 1;
            CG.blocksRaycasts = true;
            CG.interactable = true;
        }

        [Button]
        async public void FadeOut() => await FadeOutAwaitable();

        async public UniTask FadeOutAwaitable() {
            if (!Application.isPlaying) {
                CG.alpha = 0;
                CG.blocksRaycasts = false;
                CG.interactable = false;
                return;
            }
            CG.alpha = 1;
            await TweenOpacity(0, defaultOpacityTween);
            CG.alpha = 0;
            CG.blocksRaycasts = false;
            CG.interactable = false;
        }

        async public UniTask TweenOpacity(float endValue, OpacityTransitionArgs tween)
            => await Driver.TransitionOpacity(CG, endValue, tween);
    }
}
