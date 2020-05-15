using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [ExecuteInEditMode]
    public class UITweener : MonoBehaviour {
#pragma warning disable 0649
        [ReadOnly] [SerializeField] Rect parentRect;
        [ReadOnly] [SerializeField] CanvasGroup canvasGroup;
        [ReadOnly] [SerializeField] RectTransform rectTransform;
        [ReadOnly] [SerializeField] Vector3 inPosition;
        [ReadOnly] [SerializeField] Vector3 awayPosition;

        public Vector3 InPosition => inPosition;
        public Vector3 AwayPosition => awayPosition;
        public RectTransform RT {
            get {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }
        public CanvasGroup CG {
            get {
                if (canvasGroup == null)
                    canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = gameObject.GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }

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
        async public void FadeIn() {
            CG.alpha = 0;
            await TweenOpacity(1, defaultOpacityTween);
            CG.alpha = 1;
        }

        async public void FadeOut() {
            CG.alpha = 1;
            await TweenOpacity(0, defaultOpacityTween);
            CG.alpha = 0;
        }

        async public Task TweenOpacity(float endValue, OpacityTween tween)
            => await tweener.TweenOpacity(CG, endValue, tween);

        // POSITION TWEENING
        async public void MoveIn() {
            RT.localPosition = DefaultEnterCordinates;
            await TweenPosition(inPosition, defaultPositionTween);
            RT.localPosition = InPosition;
        }

        async public void MoveOut() {
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

        [SerializeField] Transform prevParent;
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
