using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Adrenak.UPF {
    public static class Extensions {
        public static void AddRange<T>(this IList<T> destination, IList<T> source) {
            foreach (var element in source)
                destination.Add(element);
        }

        public static float GetLeft(this RectTransform rt) {
            return rt.position.x - rt.rect.width * rt.lossyScale.x / 2;
        }

        public static float GetRight(this RectTransform rt) {
            return rt.position.x + rt.rect.width * rt.lossyScale.x / 2;
        }

        public static float GetTop(this RectTransform rt) {
            return rt.position.y + rt.rect.height * rt.lossyScale.y / 2;
        }

        public static float GetBottom(this RectTransform rt) {
            return rt.position.y + rt.rect.height * rt.lossyScale.y / 2;
        }

        public static Vector2 GetTopLeft(this RectTransform rt) {
            var left = rt.GetLeft();
            var top = rt.GetTop();
            return new Vector2(left, top);
        }

        public static Vector2 GetTopRight(this RectTransform rt) {
            var right = rt.GetRight();
            var top = rt.GetTop();
            return new Vector2(right, top);
        }

        public static Vector2 GetBottomLeft(this RectTransform rt) {
            var left = rt.GetLeft();
            var bottom = rt.GetBottom();
            return new Vector2(left, bottom);
        }

        public static Vector2 GetBottomRight(this RectTransform rt) {
            var right = rt.GetRight();
            var bottom = rt.GetBottom();
            return new Vector2(right, bottom);
        }
    }
}