using LambdaEngine.PlatformSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the PlatformSystem.
/// </summary>
public static class Platform {
    private static IPlatformSystem platformSystem;

    public static int WindowWidth {
        get => platformSystem.WindowWidth;
    }

    public static int WindowHeight {
        get => platformSystem.WindowHeight;
    }

    /// <summary>
    /// Initializes the PlatformSystem wrapper.
    /// </summary>
    /// <param name="platformSystem"></param>
    public static void Connect(IPlatformSystem platformSystem) {
        Platform.platformSystem = platformSystem;
    }
}