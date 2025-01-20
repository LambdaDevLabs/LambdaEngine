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
}