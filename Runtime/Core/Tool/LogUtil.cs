namespace LazyPan {
    public static class LogUtil {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(object message) {
            UnityEngine.Debug.Log(message);
        }
        
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogError(object message) {
            UnityEngine.Debug.LogError(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogFormat(string message, params object[] param) {
            UnityEngine.Debug.LogFormat(message, param);
        }
        
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogErrorFormat(string message, params object[] param) {
            UnityEngine.Debug.LogErrorFormat(message, param);
        }
    }
}