using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class OpacityTweener : TweenerBase {
        CanvasGroup cg;
        public CanvasGroup CG => cg == null ? cg = GetComponent<CanvasGroup>() : cg;

        override async public UniTask TransitionInAsync() {
            if (!Application.isPlaying) {
                CG.alpha = 1;
                CG.blocksRaycasts = true;
                CG.interactable = true;
                return;
            }
            CG.alpha = 0;
            if (useSameArgsForInAndOut)
                await TweenOpacity(1, args);
            else
                await TweenOpacity(1, inArgs);
                
            CG.alpha = 1;
            CG.blocksRaycasts = true;
            CG.interactable = true;
        }

        override async public UniTask TransitionOutAsync() {
            if (!Application.isPlaying) {
                CG.alpha = 0;
                CG.blocksRaycasts = false;
                CG.interactable = false;
                return;
            }
            CG.alpha = 1;
            if (useSameArgsForInAndOut)
                await TweenOpacity(0, args);
            else
                await TweenOpacity(0, outArgs);
            CG.alpha = 0;
            CG.blocksRaycasts = false;
            CG.interactable = false;
        }

        async public UniTask TweenOpacity(float endValue, TweenArgs tween)
            => await Driver.TransitionOpacity(CG, endValue, tween);        

        protected override void OnProgressChanged(float value) {
            CG.alpha = value;
            CG.blocksRaycasts = value != 0f;
            CG.interactable = value != 0f;
        }
    }
}
