using LambdaEngine.PlatformSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the PlatformSystem.
/// </summary>
public static class Platform {
    private static IPlatformSystem platformSystem;

    /// <summary>
    /// Initializes the PlatformSystem wrapper.
    /// </summary>
    /// <param name="platformSystem"></param>
    public static void Initialize(IPlatformSystem platformSystem) {
        Platform.platformSystem = platformSystem;
    }
}