using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Tweens the position of a UI element using <see cref="RectTransform"/>
    /// </summary>
    [DisallowMultipleComponent]
    public class RectTransformPositionTweener : TweenerBase {
        /// <summary>
        /// The edge of a UI element
        /// </summary>
        public enum Edge {
            Top,
            Bottom,
            Left,
            Right
        }

        [SerializeField] Vector3 inPosition;
        /// <summary>
        /// The position at which the UI element is considered to be "in"
        /// </summary>
        public Vector3 InPosition => inPosition;

        [SerializeField] Vector3 outPosition;
        /// <summary>
        /// The position at which the UI element is considers to be "out"
        /// </summary>
        public Vector3 OutPosition => outPosition;

        /// <summary>
        /// The edge from which the UI element should tween in
        /// </summary>
        public Edge enterEdge = Edge.Left;

        /// <summary>
        /// The edge to which the UI eement should tween out
        /// </summary>
        public Edge exitEdge = Edge.Right;

        /// <summary>
        /// Sets the current local position of the <see cref="RectTransform"/>
        /// as the <see cref="inPosition"/>
        /// </summary>
        public void CaptureInPosition() => inPosition = RectTransform.localPosition;

        /// <summary>
        /// Sets the current local position of the <see cref="RectTransform"/>
        /// as the <see cref="OutPosition"/>
        /// </summary>
        public void CaptureOutPosition() => outPosition = RectTransform.localPosition;

        /// <summary>
        /// Sets the tweening progress to a value between 0 and 1. 
        /// Use this to have manual control over the position.
        /// </summary>
        /// <param name="value"></param>
        protected override void SetProgress(float value) {
            RectTransform.localPosition = Vector3.Lerp(OutPosition, InPosition, value);
        }

        /// <summary>
        /// Gets the nearest local position for this UI element to be outside 
        /// of the screen at the given edge of the screen.
        /// </summary>
        public Vector2 GetExitCoordinates(Edge edge) {
            switch (edge) {
                case Edge.Left: return LeftExitCordinates;
                case Edge.Right: return RightExitCordinates;
                case Edge.Top: return TopExitCordinates;
                case Edge.Bottom: return BottomExitCordinates;
                default: return Vector2.zero;
            }
        }

        /// <summary>
        /// The exit coordinates towards the right
        /// </summary>
        public Vector2 RightExitCordinates
            => new Vector2(RectTransform.GetRightExit(), inPosition.y);

        /// <summary>
        /// The exit coordinates towards the top
        /// </summary>
        public Vector2 TopExitCordinates
            => new Vector2(inPosition.x, RectTransform.GetTopExit());

        /// <summary>
        /// /// The exit coordinates towards the left
        /// </summary>
        public Vector2 LeftExitCordinates
            => new Vector2(RectTransform.GetLeftExit(), inPosition.y);

        /// <summary>
        /// /// The exit coordinates towards the bottom
        /// </summary>
        public Vector2 BottomExitCordinates
            => new Vector2(inPosition.x, RectTransform.GetBottomExit());
    }
}