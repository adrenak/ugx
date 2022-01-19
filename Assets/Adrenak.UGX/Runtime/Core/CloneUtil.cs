using System;

// Provides cloning for a type. Stores the operation as a delegate 
// for faster operation.
// https://stackoverflow.com/a/52972518
namespace Adrenak.UGX {
    public static class CloneUtil<T> {
        private static readonly Func<T, object> clone;

        static CloneUtil() {
            var cloneMethod = typeof(T).GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            clone = (Func<T, object>)cloneMethod.CreateDelegate(typeof(Func<T, object>));
        }

        public static T ShallowClone(T obj) => (T)clone(obj);
    }

    public static class CloneUtil {
        public static T ShallowClone<T>(this T obj) => CloneUtil<T>.ShallowClone(obj);
    }
}