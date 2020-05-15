using UnityEngine;

public static class TweeningExtensions {
    public static float GetLeftExit(this RectTransform rt) {
        var parentRect = rt.parent.GetComponent<RectTransform>().rect;
        return -parentRect.width / 2 - rt.rect.width / 2;
    }

    public static float GetRightExit(this RectTransform rt) {
        var parentRect = rt.parent.GetComponent<RectTransform>().rect;
        return parentRect.width / 2 + rt.rect.width / 2;
    }

    public static float GetTopExit(this RectTransform rt) {
        var parentRect = rt.parent.GetComponent<RectTransform>().rect;
        return parentRect.height / 2 + rt.rect.height / 2;
    }

    public static float GetBottomExit(this RectTransform rt) {
        var parentRect = rt.parent.GetComponent<RectTransform>().rect;
        return -parentRect.width / 2 - rt.rect.height / 2;
    }
}
