using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class RebuildOnResize : MonoBehaviour {
        public float resizingCheckFrameStep = 2;

        void Update() {
            TryResize();
        }

        int lastHeight, lastWidth, height, width;
        RectTransform childRT;
        void TryResize() {
            if (resizingCheckFrameStep <= 0 || Time.frameCount % resizingCheckFrameStep != 0) return;

            height = width = 0;
            foreach (Transform child in transform) {
                childRT = child.GetComponent<RectTransform>();
                height += (int)childRT.sizeDelta.y;
                width += (int)childRT.sizeDelta.x;
            }

            if (height != lastHeight || width != lastWidth)
                LayoutRebuilder.MarkLayoutForRebuild(transform.GetComponent<RectTransform>());

            lastHeight = height;
            lastWidth = width;
        }
    }
}
