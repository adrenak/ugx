using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    public class SafeAreaPanel : MonoBehaviour {
        public static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();
         static Rect lastSafeArea = Rect.zero;

         Canvas canvas;
         RectTransform RT;

        void Start() {
            canvas = GetComponentInParent<Canvas>();
            RT = GetComponent<RectTransform>();
            lastSafeArea = Screen.safeArea;

            ApplySafeArea();
        }

        void Update() {
            if (Screen.safeArea != lastSafeArea){
                lastSafeArea = Screen.safeArea;
                ApplySafeArea();
            }
        }

        void ApplySafeArea() {
            if (RT == null)
                return;

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;
            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            RT.anchorMin = anchorMin;
            RT.anchorMax = anchorMax;
        }
    }
}