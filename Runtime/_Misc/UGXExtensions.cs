using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Adrenak.UGX {
    public static class UGXExtensions {
        public static void SetColor(this Image image, Color color){
            image.color = color;
        }

        public static void AddRange<T>(this IList<T> destination, IList<T> source) {
            foreach (var element in source)
                destination.Add(element);
        }

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
            return rt.position.y - rt.rect.height * rt.lossyScale.y / 2;
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

        public static bool IsVisible(this RectTransform rt, out bool? partially) {
            var points = new Vector2[]{
                rt.GetTopLeft(),
                rt.GetTopRight(),
                rt.GetBottomRight(),
                rt.GetBottomLeft()
            };

            // If every point is within the screen bounds,
            // we return true and false for partially.
            // Which basically means full visibility
            int count = 0;
            foreach (var point in points)
				if (point.y >= -1
                    && point.y <= Screen.height + 1
                    && point.x >= -1
                    && point.x <= Screen.width + 1
                )
					count++;

            if(count == 4) {
                partially = false;
                return true;
			}

            // Check if every point is either above, below,
            // left of, or right of the screen. In which case
            // we return invisible and false for partially.
            // Which basically means full invisibility

            // ABOVE
            count = 0;
			foreach (var point in points)
				count += point.y > Screen.height + 1 ? 1 : 0;
			if (count == 4) {
				partially = false;
				return false;
			}

            // BELOW
            count = 0;
            foreach (var point in points)
                count += point.y < -1 ? 1 : 0;
            if (count == 4) {
                partially = false;
                return false;
            }

            // RIGHT OF
            count = 0;
            foreach (var point in points)
                count += point.x > Screen.width + 1 ? 1 : 0;
            if (count == 4) {
                partially = false;
                return false;
            }

            // LEFT OF
            count = 0;
            foreach (var point in points)
                count += point.x < -1 ? 1 : 0;
            if (count == 4) {
                partially = false;
                return false;
            }

            // Else we return true for both.
            // Means partial visibility/invisibility
            partially = true;
            return true;
        }

        public static void EnsureContains<T, K>(this IDictionary<T, K> dict, T t, K k){
            if (!dict.ContainsKey(t))
                dict.Add(t, k);
        }

        public static void EnsureContains<T>(this List<T> list, T t){
            if (!list.Contains(t))
                list.Add(t);
        }

        public static void EnsureDoesntContain<T>(this List<T> list, T t){
            if (list.Contains(t))
                list.Remove(t);
        }
    }
}