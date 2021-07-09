using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    public class SafeArea : MonoBehaviour {
        public static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();

        private static bool screenChangeVarsInitialized = false;
        private static ScreenOrientation lastOrientation = ScreenOrientation.Landscape;
        private static Vector2 lastResolution = Vector2.zero;
        private static Rect lastSafeArea = Rect.zero;

        private Canvas canvas;
        private RectTransform RT;

        public void Start() {
            canvas = GetComponentInParent<Canvas>();

            RT = GetComponent<RectTransform>();

            if (!screenChangeVarsInitialized) {
                lastOrientation = Screen.orientation;
                lastResolution.x = Screen.width;
                lastResolution.y = Screen.height;
                lastSafeArea = Screen.safeArea;

                screenChangeVarsInitialized = true;
            }

            ApplySafeArea();
        }

        void Update() {
            if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
                OrientationChanged();

            if (Screen.safeArea != lastSafeArea)
                SafeAreaChanged();

            if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
                ResolutionChanged();
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

        private static void OrientationChanged() {
            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            OnResolutionOrOrientationChanged.Invoke();
        }

        private static void ResolutionChanged() {
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            OnResolutionOrOrientationChanged.Invoke();
        }

        private void SafeAreaChanged() {
            lastSafeArea = Screen.safeArea;

            ApplySafeArea();
        }
    }
}