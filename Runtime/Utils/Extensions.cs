using Adrenak.Unex;
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

        public static bool IsVisible(this RectTransform rt, out bool? extent) {
            var screen = new Vector2(Screen.width, Screen.height);

            bool IsPointInside(Vector2 point){
                return (point.x > 0 &&
                    point.x < Screen.width &&
                    point.y > 0 &&
                    point.y < Screen.height);
            }

            var points = new Vector2[]{
                rt.GetTopRight(),
                rt.GetTopLeft(),
                rt.GetBottomLeft(),
                rt.GetBottomRight()
            };

            int count = 0;
            foreach(var  point in points)
                count += IsPointInside(point) ? 1 : 0;
            
            if(count == 0){
                extent = null;
                return false;
            }

            extent = count == 4;
            return true;
        }
    }
}