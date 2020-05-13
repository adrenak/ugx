﻿using System;
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
            RTLocalPosition = inPosition;
        }

        public Vector3 RTLocalPosition {
            get => RT.localPosition;
            set => RT.localPosition = value;
        }

        public float CGAlpha {
            get => CG.alpha;
            set => CG.alpha = value;
        }

        // OPACITY TWEENING
        // Quick
        public void FadeInAndForget(){
            FadeIn();
        }

        public Task FadeIn() {
            var source = new TaskCompletionSource<bool>();
            FadeIn(() => source.SetResult(true));
            return source.Task;
        }

        public void FadeIn(Action onComplete) {
            CGAlpha = 0;
            TweenOpacity(1, defaultOpacityTween, "FadeIn", onComplete);
        }

        public void FadeOutAndForget(){
            FadeOut();
        }

        public Task FadeOut() {
            var source = new TaskCompletionSource<bool>();
            FadeOut(() => source.SetResult(true));
            return source.Task;
        }

        public void FadeOut(Action onComplete) {
            CGAlpha = 1;
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
        public void MoveInAndForget(){
            MoveIn();
        }

        public Task MoveIn() {
            var source = new TaskCompletionSource<bool>();
            MoveIn(() => source.SetResult(true));
            return source.Task;
        }

        public void MoveIn(Action onComplete = null) {
            RTLocalPosition = DefaultEnter;
            TweenPosition(inPosition, defaultPositionTween, "MoveIn", onComplete);
        }

        public void MoveOutAndForget(){
            MoveOut();
        }

        public Task MoveOut() {
            var source = new TaskCompletionSource<bool>();
            MoveOut(() => source.SetResult(true));
            return source.Task;
        }

        public void MoveOut(Action onComplete) {
            RTLocalPosition = InPosition;
            TweenPosition(DefaultExit, defaultPositionTween, "MoveOut", () => {
                RTLocalPosition = AwayPosition;
                onComplete?.Invoke();
            });
        }

        // Custom
        public Task TweenPosition(Vector3 endValue, PositionTween tween, object args = null) {
            var source = new TaskCompletionSource<bool>();
            TweenPosition(endValue, tween, args, () => source.SetResult(true));
            return source.Task;
        }

        public void TweenPosition(Vector3 endValue, PositionTween tween, object args, Action onComplete = null){
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
                case Position.Left: return LeftExit;
                case Position.Right: return RightExit;
                case Position.Top: return TopExit;
                case Position.Bottom: return BottomExit;
                default: return Vector3.zero;
            }
        }

        public Vector3 DefaultExit {
            get => GetPositionVector3(defaultPositionForExit);
        }

        public Vector3 DefaultEnter {
            get => GetPositionVector3(defaultPositionForEnter);
        }

        // Computed
        public Vector3 RightExit {
            get {
                var x = (parentRect.width / 2) -
                    (inPosition.x + RT.rect.width / 2) +
                    RT.rect.width;
                return new Vector3(x, inPosition.y, RT.localPosition.z);
            }
        }

        public Vector3 TopExit {
            get {
                var y = (parentRect.height / 2) -
                    inPosition.y +
                    RT.rect.height;
                return new Vector3(inPosition.x, y, RT.localPosition.z);
            }
        }

        public Vector3 LeftExit {
            get {
                var x = -(parentRect.width / 2) +
                    (inPosition.x + RT.rect.width / 2) -
                    RT.rect.width;
                return new Vector3(x, inPosition.y, RT.localPosition.z);
            }
        }

        public Vector3 BottomExit {
            get {
                var y = -(parentRect.height / 2) +
                    inPosition.y -
                    RT.rect.height;
                return new Vector3(inPosition.x, y, RT.localPosition.z);
            }
        }

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
