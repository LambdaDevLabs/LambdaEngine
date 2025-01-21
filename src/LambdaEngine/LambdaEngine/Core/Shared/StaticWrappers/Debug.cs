using LambdaEngine.DebugSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the DebugSystem.
/// </summary>
public static class Debug {
    private static IDebugSystem debugSystem;

    /// <summary>
    /// Initializes the DebugSystem wrapper.
    /// </summary>
    /// <param name="debugSystem"></param>
    public static void Initialize(IDebugSystem debugSystem) {
        Debug.debugSystem = debugSystem;
    }
    
    /// <summary>
    /// Starts the debugger.
    /// </summary>
    public static void Start() {
        debugSystem.Start();
    }

    /// <summary>
    /// Stops the debugger.
    /// </summary>
    public static void Stop() {
        debugSystem.Stop();
    }

    /// <summary>
    /// Queue a log message for the debugger.
    /// </summary>
    /// <param name="message"></param>
    public static void Log(string message) {
        debugSystem.Log(message);
    }

    /// <summary>
    /// Queue a log message for the debugger.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="loglevel"></param>
    public static void Log(string message, LogLevel loglevel) {
        debugSystem.Log(message, loglevel);
    }
}