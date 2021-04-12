using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class OpacityTransitioner : TransitionerBase {
        [BoxGroup("Config")] public OpacityTransitionArgs args;

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
            await TweenOpacity(1, args);
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
            await TweenOpacity(0, args);
            CG.alpha = 0;
            CG.blocksRaycasts = false;
            CG.interactable = false;
        }

        async public UniTask TweenOpacity(float endValue, OpacityTransitionArgs tween)
            => await Driver.TransitionOpacity(CG, endValue, tween);
        

        protected override void SetProgress(float value) {
            CG.alpha = value;
            CG.blocksRaycasts = value != 0f;
            CG.interactable = value != 0f;
        }
    }
}
