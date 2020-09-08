using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [ExecuteAlways]
    public class Transitioner : UIBehaviour {
#pragma warning disable 0414
        [SerializeField] bool showReadonlyFields = true;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] Vector3 inPosition;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] Vector3 awayPosition;

        [BoxGroup("Default Position Tween")] public PositionType defaultPositionForEnter = PositionType.Left;
        [BoxGroup("Default Position Tween")] public PositionType defaultPositionForExit = PositionType.Right;
        [BoxGroup("Default Position Tween")] public PositionTransition defaultPositionTween;
        [BoxGroup("Default Opacity Tween")] public OpacityTransition defaultOpacityTween;
#pragma warning restore 0649

        readonly ITransitioner tweener = new SurgeTransitioner();

        [Button("Capture In Position")]
        public void CaptureInPosition() => inPosition = RT.localPosition;

        [Button("Capture Away Position")]
        public void CaptureAwayPosition() => awayPosition = RT.localPosition;        

        [Button("Goto In Position")]
        public void GotoInPosition() => RT.localPosition = inPosition;

        [Button("Goto Away Position")]
        public void GotoAwayPosition() => RT.localPosition = awayPosition;

        Rect parentRT;
        RectTransform rt;
        CanvasGroup cg;
        
        public RectTransform RT => rt ?? (rt = GetComponent<RectTransform>());
        public CanvasGroup CG => cg ?? (cg = GetComponent<CanvasGroup>());
        public Vector3 InPosition => inPosition;
        public Vector3 AwayPosition => awayPosition;

        // ================================================
        #region TWEENING
        // ================================================
        // OPACITY TWEENING
        async public void FadeInAndForget() => await FadeIn();
        async public Task FadeIn() {
            CG.alpha = 0;
            await TweenOpacity(1, defaultOpacityTween);
            CG.alpha = 1;
            CG.blocksRaycasts = true;
            CG.interactable = true;
        }

        async public void FadeOutAndForget() => await FadeOut();
        async public Task FadeOut() {
            CG.alpha = 1;
            await TweenOpacity(0, defaultOpacityTween);
            CG.alpha = 0;
            CG.blocksRaycasts = false;
            CG.interactable = false;
        }

        async public Task TweenOpacity(float endValue, OpacityTransition tween)
            => await tweener.TransitionOpacity(CG, endValue, tween);

        // POSITION TWEENING
        async public void MoveInAndForget() => await MoveIn();
        async public Task MoveIn() {
            RT.localPosition = DefaultEnterCordinates;
            await TweenPosition(inPosition, defaultPositionTween);
            RT.localPosition = InPosition;
        }

        async public void MoveOutAndForget() => await MoveOut();
        async public Task MoveOut() {
            RT.localPosition = InPosition;
            await TweenPosition(DefaultExitCordinates, defaultPositionTween);
            RT.localPosition = AwayPosition;
        }

        async public Task TweenPosition(Vector3 endValue, PositionTransition tween)
            => await tweener.TransitionPosition(RT, endValue, tween);

        #endregion

        // ================================================
        #region UNITY LIFECYCLE
        // ================================================
        Rect screenRect;
        void Awake() {
            screenRect = new Rect(0, 0, ScreenX.Width, ScreenX.Height);
        }

        void Update() {
            UpdateParentRect();
        }

        public Transform prevParent;
        void UpdateParentRect() {
            if ((transform.parent == null ||
                transform.parent.GetComponent<RectTransform>() == null) &&
                parentRT != screenRect
            ) {
                parentRT = screenRect;
                return;
            }

            if (prevParent = null) {
                prevParent = transform.parent;
                parentRT = prevParent.GetComponent<RectTransform>().rect;
                return;
            }
            else if (transform.parent != prevParent) {
                prevParent = transform.parent;
                parentRT = prevParent.GetComponent<RectTransform>().rect;
                return;
            }
        }
        #endregion

        public Vector3 GetPositionVector3(PositionType position) {
            switch (position) {
                case PositionType.Left: return LeftExitCordinates;
                case PositionType.Right: return RightExitCordinates;
                case PositionType.Top: return TopExitCordinates;
                case PositionType.Bottom: return BottomExitCordinates;
                default: return Vector3.zero;
            }
        }

        public Vector3 DefaultExitCordinates
            => GetPositionVector3(defaultPositionForExit);

        public Vector3 DefaultEnterCordinates
            => GetPositionVector3(defaultPositionForEnter);

        public Vector3 RightExitCordinates
            => new Vector3(RT.GetRightExit(), inPosition.y, RT.localPosition.z);

        public Vector3 TopExitCordinates
            => new Vector3(inPosition.x, RT.GetTopExit(), RT.localPosition.z);

        public Vector3 LeftExitCordinates
            => new Vector3(RT.GetLeftExit(), inPosition.y, RT.localPosition.z);

        public Vector3 BottomExitCordinates
            => new Vector3(inPosition.x, RT.GetBottomExit(), RT.localPosition.z);
    }
}
