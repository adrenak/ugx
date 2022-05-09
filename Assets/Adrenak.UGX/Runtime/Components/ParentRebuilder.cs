using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    /// <summary>
    /// Every few frames check if the sum of the scale of children has changed 
    /// and if so, rebuild this <see cref="RectTransform"/>
    /// 
    /// The main purpose is to rebuild layout groups when children are
    /// scaled at runtime. Unity often doesn't rebuild the layout correctly and
    /// we need to force a manual update.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ParentRebuilder : MonoBehaviour {
        public float frameStep = 2;
        public Vector2 threshold = new Vector2(1, 1);
        RectTransform RT;

        void Start() => RT = GetComponent<RectTransform>();
        void Update() {
            TryResize();
        }

        int lastHeight, lastWidth, height, width;
        RectTransform childRT;
        void TryResize() {
            if (frameStep <= 0 || Time.frameCount % frameStep != 0) return;

            height = width = 0;
            foreach (Transform child in transform) {
                childRT = child.GetComponent<RectTransform>();
                height += (int)childRT.sizeDelta.y;
                width += (int)childRT.sizeDelta.x;
            }

            if (Mathf.Abs(height - lastHeight) > threshold.y 
            || (Mathf.Abs(width - lastWidth) > threshold.x))
                LayoutRebuilder.MarkLayoutForRebuild(RT);

            lastHeight = height;
            lastWidth = width;
        }
    }
}
