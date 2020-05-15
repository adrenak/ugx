using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [ExecuteAlways]
    public class UITweener : MonoBehaviour {
        public event Action<object> OnBeginPositionTweening;
        public event Action<object> OnEndPositionTweening;
        public event Action<object> OnBeginOpacityTweening;
        public event Action<object> OnEndOpacityTweening;

        // MEMBERS
        [ReadOnly] [SerializeField] Rect parentRect;

        [ReadOnly] [SerializeField] Vector3 inPosition;
        public Vector3 InPosition { get => inPosition; }

        [ReadOnly] [SerializeField] Vector3 awayPosition;
        public Vector3 AwayPosition { get => awayPosition; }

        [BoxGroup("Default Position Tween")] public Position defaultPositionForEnter = Position.Left;
        [BoxGroup("Default Position Tween")] public Position defaultPositionForExit = Position.Right;
        [BoxGroup("Default Position Tween")] public PositionTween defaultPositionTween = null;
        [BoxGroup("Default Opacity Tween")] public OpacityTween defaultOpacityTween = null;

        ITweener tweener = new SurgeTweener();
        Transform prevParent;
        Rect screenRect;

        [Button("Capture In Position")]
        public void CaptureInPosition() {
            inPosition = GetComponent<RectTransform>().localPosition;
        }

        [Button("Goto In Position")]
        public void GotoInPosition() {
            RT.localPosition = inPosition;
        }

        // OPACITY TWEENING
        // Quick
        public void FadeInAndForget() {
            FadeIn();
        }

        public Task FadeIn() {
            var source = new TaskCompletionSource<bool>();
            FadeIn(() => source.SetResult(true));
            return source.Task;
        }

        public void FadeIn(Action onComplete) {
            CG.alpha = 0;
            TweenOpacity(1, defaultOpacityTween, "FadeIn", onComplete);
        }

        public void FadeOutAndForget() {
            FadeOut();
        }

        public Task FadeOut() {
            var source = new TaskCompletionSource<bool>();
            FadeOut(() => source.SetResult(true));
            return source.Task;
        }

        public void FadeOut(Action onComplete) {
            CG.alpha = 1;
            TweenOpacity(0, defaultOpacityTween, "FadeOut", onComplete);
        }

        // Custom
        public Task TweenOpacity(float endValue, OpacityTween tween, object args = null) {
            var source = new TaskCompletionSource<bool>();
            TweenOpacity(endValue, tween, args, () => source.SetResult(true));
            return source.Task;
        }

        public void TweenOpacity(float endValue, OpacityTween tween, object args, Action onComplete = null) {
            OnBeginOpacityTweening?.Invoke(args);
            TweenOpacity(endValue, tween, () => {
                OnEndOpacityTweening?.Invoke(args);
                onComplete?.Invoke();
            });
        }

        public void TweenOpacity(float endValue, OpacityTween tween, Action onComplete = null) {
            tweener.TweenOpacity(CG, endValue, tween, onComplete);
        }

        // POSITION TWEENING
        // Quick
        public void MoveInAndForget() {
            MoveIn();
        }

        public Task MoveIn() {
            var source = new TaskCompletionSource<bool>();
            MoveIn(() => source.SetResult(true));
            return source.Task;
        }

        public void MoveIn(Action onComplete = null) {
            RT.localPosition = DefaultEnterCordinates;
            TweenPosition(inPosition, defaultPositionTween, "MoveIn", onComplete);
        }

        public void MoveOutAndForget() {
            MoveOut();
        }

        public Task MoveOut() {
            var source = new TaskCompletionSource<bool>();
            MoveOut(() => source.SetResult(true));
            return source.Task;
        }

        public void MoveOut(Action onComplete) {
            RT.localPosition = InPosition;
            TweenPosition(DefaultExitCordinates, defaultPositionTween, "MoveOut", () => {
                RT.localPosition = AwayPosition;
                onComplete?.Invoke();
            });
        }

        // Custom
        public Task TweenPosition(Vector3 endValue, PositionTween tween, object args = null) {
            var source = new TaskCompletionSource<bool>();
            TweenPosition(endValue, tween, args, () => source.SetResult(true));
            return source.Task;
        }

        public void TweenPosition(Vector3 endValue, PositionTween tween, object args, Action onComplete = null) {
            OnBeginPositionTweening?.Invoke(args);
            TweenPosition(endValue, tween, () => {
                OnEndPositionTweening?.Invoke(args);
                onComplete?.Invoke();
            });
        }

        public void TweenPosition(Vector3 endValue, PositionTween tween, Action onComplete = null) {
            tweener.TweenPosition(RT, endValue, tween, onComplete);
        }

        // UNITY INVOCATION
        void Awake() {
            awayPosition = RT.localPosition;
            screenRect = new Rect(0, 0, ScreenX.Width, ScreenX.Height);
        }

        void Update() {
            UpdateParentRect();
        }

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

        // POSITION VECTOR3 ACCESSORS
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

        // COMPONENT CACHING
        CanvasGroup cg;
        public CanvasGroup CG {
            get {
                if (cg == null)
                    cg = GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = gameObject.AddComponent<CanvasGroup>();
                return cg;
            }
        }

        RectTransform rt;
        public RectTransform RT {
            get { return rt = rt ?? GetComponent<RectTransform>(); }
        }
    }
}
