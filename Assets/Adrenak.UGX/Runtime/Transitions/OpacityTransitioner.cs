using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(CanvasGroup))]
    public class OpacityTransitioner : MonoBehaviour {
        public OpacityTransitionArgs defaultOpacityTween;

        readonly ITransitioner tweener = new SurgeTransitioner();

        RectTransform rt;
        public RectTransform RT => rt == null ? rt = GetComponent<RectTransform>() : rt;

        CanvasGroup cg;
        public CanvasGroup CG => cg == null ? cg = GetComponent<CanvasGroup>() : cg;

        [Button]
        async public void FadeInAndForget() => await FadeIn();
        async public UniTask FadeIn() {
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
        async public void FadeOutAndForget() => await FadeOut();
        async public UniTask FadeOut() {
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
            => await tweener.TransitionOpacity(CG, endValue, tween);
    }
}
