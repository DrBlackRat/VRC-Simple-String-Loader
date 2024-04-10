using UnityEngine;

namespace DrBlackRat
{
    public static class SSLDebug
    {
        private const string logPrefix = "[<color=#34cfeb>Simple String Loader</color>]";

        public static void Log(object message)
        {
            Debug.Log($"{logPrefix} {message}");
        }
        public static void LogWarning(object message)
        {
            Debug.LogWarning($"{logPrefix} {message}");
        }
        public static void LogError(object message)
        {
            Debug.LogError($"{logPrefix} {message}");
        }
    }
}
