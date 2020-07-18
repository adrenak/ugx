using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [ExecuteAlways]
    public class UITweener : MonoBehaviour {
#pragma warning disable 0649
#pragma warning disable 0414
        [SerializeField] bool showReadonlyFields = true;
#pragma warning restore 0414
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] Rect parentRect;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] CanvasGroup canvasGroup;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] RectTransform rectTransform;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] Vector3 inPosition;
        [ShowIf(nameof(showReadonlyFields))] [ReadOnly] [SerializeField] Vector3 awayPosition;

        public Vector3 InPosition => inPosition;
        public Vector3 AwayPosition => awayPosition;
        public RectTransform RT =>
            rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();
        public CanvasGroup CG =>
            canvasGroup = canvasGroup != null ? canvasGroup : GetComponent<CanvasGroup>();

        [BoxGroup("Default Position Tween")] public Position defaultPositionForEnter = Position.Left;
        [BoxGroup("Default Position Tween")] public Position defaultPositionForExit = Position.Right;
        [BoxGroup("Default Position Tween")] public PositionTween defaultPositionTween;
        [BoxGroup("Default Opacity Tween")] public OpacityTween defaultOpacityTween;
#pragma warning restore 0649

        readonly ITweener tweener = new SurgeTweener();

        [Button("Capture In Position")]
        public void CaptureInPosition() => inPosition = RT.localPosition;

        [Button("Capture Away Position")]
        public void CaptureAwayPosition() => awayPosition = RT.localPosition;        

        [Button("Goto In Position")]
        public void GotoInPosition() => RT.localPosition = inPosition;

        [Button("Goto Away Position")]
        public void GotoAwayPosition() => RT.localPosition = awayPosition;

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

        async public Task TweenOpacity(float endValue, OpacityTween tween)
            => await tweener.TweenOpacity(CG, endValue, tween);

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

        async public Task TweenPosition(Vector3 endValue, PositionTween tween)
            => await tweener.TweenPosition(RT, endValue, tween);

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
                parentRect != screenRect
            ) {
                parentRect = screenRect;
                return;
            }

            if (prevParent = null) {
                prevParent = transform.parent;
                parentRect = prevParent.GetComponent<RectTransform>().rect;
                return;
            }
            else if (transform.parent != prevParent) {
                prevParent = transform.parent;
                parentRect = prevParent.GetComponent<RectTransform>().rect;
                return;
            }
        }
        #endregion

        public Vector3 GetPositionVector3(Position position) {
            switch (position) {
                case Position.Left: return LeftExitCordinates;
                case Position.Right: return RightExitCordinates;
                case Position.Top: return TopExitCordinates;
                case Position.Bottom: return BottomExitCordinates;
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
