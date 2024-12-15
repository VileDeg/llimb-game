public static class LogUtil
{
    public static void Info(string message)
    {
        string callerInfo = getCallerInfo();
        UnityEngine.Debug.Log($"[{callerInfo}] {message}");
    }

    public static void Warn(string message)
    {
        string callerInfo = getCallerInfo();
        UnityEngine.Debug.LogWarning($"[{callerInfo}] {message}");
    }

    public static void Error(string message)
    {
        string callerInfo = getCallerInfo();
        UnityEngine.Debug.LogError($"[{callerInfo}] {message}");
    }

    private static string getCallerInfo()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(1);
        string callerInfo = $"{stackFrame.GetMethod().DeclaringType}.{stackFrame.GetMethod().Name}";
        return callerInfo;
    }
}
