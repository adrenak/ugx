using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public static partial class UGX {
        public static class Debug {
            public static bool Enabled { get; set; }
            public static List<LogType> LogTypes { get; set; }

            public static void Log(object message, UnityEngine.Object context = null) {
                if (!LogTypes.Contains(LogType.Log)) return;

                if (context != null)
                    UnityEngine.Debug.Log(message, context);
                else
                    UnityEngine.Debug.Log(message);
            }

            public static void LogWarning(object message, UnityEngine.Object context = null) {
                if (!LogTypes.Contains(LogType.Warning)) return;

                if (context != null)
                    UnityEngine.Debug.LogWarning(message, context);
                else
                    UnityEngine.Debug.LogWarning(message);
            }

            public static void LogException(Exception exception, UnityEngine.Object context = null) {
                if (!LogTypes.Contains(LogType.Exception)) return;

                if (context != null)
                    UnityEngine.Debug.LogException(exception, context);
                else
                    UnityEngine.Debug.LogException(exception);
            }

            public static void LogError(object message, UnityEngine.Object context = null) {
                if (!LogTypes.Contains(LogType.Error)) return;

                if (context != null)
                    UnityEngine.Debug.LogError(message, context);
                else
                    UnityEngine.Debug.LogError(message);
            }
        }
    }
}
