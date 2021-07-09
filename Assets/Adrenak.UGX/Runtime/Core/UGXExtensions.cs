using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Adrenak.UGX {
    public static class UGXExtensions {
        /// <summary>
        /// Returns the minimum distance RectTransform should move to the left
        /// to get out of the bounds of the parent RectTransform.
        /// </summary>
        public static float GetLeftExit(this RectTransform rt) {
            var parentRect = rt.parent.GetComponent<RectTransform>().rect;
            return -parentRect.width / 2 - rt.rect.width / 2;
        }

        /// <summary>
        /// Returns the minimum distance RectTransform should move to the right
        /// to get out of the bounds of the parent RectTransform.
        /// </summary>
        public static float GetRightExit(this RectTransform rt) {
            var parentRect = rt.parent.GetComponent<RectTransform>().rect;
            return parentRect.width / 2 + rt.rect.width / 2;
        }

        /// <summary>
        /// Returns the minimum distance RectTransform should move to the top
        /// to get out of the bounds of the parent RectTransform.
        /// </summary>
        public static float GetTopExit(this RectTransform rt) {
            var parentRect = rt.parent.GetComponent<RectTransform>().rect;
            return parentRect.height / 2 + rt.rect.height / 2;
        }

        /// <summary>
        /// Returns the minimum distance RectTransform should move to the bottom
        /// to get out of the bounds of the parent RectTransform.
        /// </summary>
        public static float GetBottomExit(this RectTransform rt, bool screen = false) {
            Rect parentRect = rt.parent.GetComponent<RectTransform>().rect;
            return -parentRect.width / 2 - rt.rect.height / 2;
        }

        /// <summary>
        /// Returns the x coordinate of the left edge of the RectTransform
        /// </summary>
        public static float GetLeft(this RectTransform rt) {
            return rt.position.x - rt.rect.width * rt.lossyScale.x / 2;
        }

        /// <summary>
        /// Returns the x coordinate of the right edge of the RectTransform
        /// </summary>
        public static float GetRight(this RectTransform rt) {
            return rt.position.x + rt.rect.width * rt.lossyScale.x / 2;
        }

        /// <summary>
        /// Returns the y coordinate of the top edge of the RectTransform
        /// </summary>
        public static float GetTop(this RectTransform rt) {
            return rt.position.y + rt.rect.height * rt.lossyScale.y / 2;
        }

        /// <summary>
        /// Returns the y coordinate of the left edge of the RectTransform
        /// </summary>
        public static float GetBottom(this RectTransform rt) {
            return rt.position.y - rt.rect.height * rt.lossyScale.y / 2;
        }

        /// <summary>
        /// Returns the coordinate of the top left corner of the RectTransform
        /// </summary>
        public static Vector2 GetTopLeft(this RectTransform rt) {
            var left = rt.GetLeft();
            var top = rt.GetTop();
            return new Vector2(left, top);
        }

        /// <summary>
        /// Returns the coordinate of the top right corner of the RectTransform
        /// </summary>
        public static Vector2 GetTopRight(this RectTransform rt) {
            var right = rt.GetRight();
            var top = rt.GetTop();
            return new Vector2(right, top);
        }

        /// <summary>
        /// Returns the coordinate of the bottom left corner of the RectTransform
        /// </summary>
        public static Vector2 GetBottomLeft(this RectTransform rt) {
            var left = rt.GetLeft();
            var bottom = rt.GetBottom();
            return new Vector2(left, bottom);
        }

        /// <summary>
        /// Returns the coordinate of the bottom right corner of the RectTransform
        /// </summary>
        public static Vector2 GetBottomRight(this RectTransform rt) {
            var right = rt.GetRight();
            var bottom = rt.GetBottom();
            return new Vector2(right, bottom);
        }

        /// <summary>
        /// Returns the visibility of the RectTransform
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        public static Visibility GetVisibility(this RectTransform rt) {
            var result = rt.IsVisible(out bool? partially);

            if (!partially.Value)
                return result ? Visibility.Full : Visibility.None;
            else
                return Visibility.Partial;
        }

        /// <summary>
        /// Returns if the RectTransform is visible 
        /// </summary>
        /// <param name="partially">If true, the RT is partially visible</param>
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

            if (count == 4) {
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

        /// <summary>
        /// Set a KeyValuePair entry in the Dictionary. If the key exists, 
        /// the value is change. If not, the pair is added.
        /// </summary>
        public static void SetPair<K, V>(this IDictionary<K, V> dict, K k, V v) {
            if (!dict.ContainsKey(k))
                dict.Add(k, v);
            else
                dict[k] = v;
        }

        /// <summary>
        /// Ensures the key is present in the dictionary. If it isn't, the provided
        /// value is added along with the key.
        /// </summary>
        /// <returns>If the key was already present</returns>
        public static bool EnsureKey<T, K>(this IDictionary<T, K> dict, T t, K k) {
            if (!dict.ContainsKey(t)) {
                dict.Add(t, k);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ensures the list contains the element.
        /// </summary>
        /// <returns>If the element was already present</returns>
        public static bool EnsureContains<T>(this List<T> list, T t) {
            if (!list.Contains(t)){
                list.Add(t);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ensures that the list doesn't contain the element
        /// </summary>
        /// <returns>Returns true if the list DID contain the element</returns>
        public static bool EnsureDoesntContain<T>(this List<T> list, T t) {
            if (list.Contains(t)){
                list.Remove(t);
                return true;
            }
            return false;
        }
    }
}