using LambdaEngine.AssetManagementSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the AudioSystem.
/// </summary>
public static class AssetManagement {
    private static IAssetManagementSystem assetManagementSystem;

    /// <summary>
    /// Initializes the AudioSystem wrapper.
    /// </summary>
    /// <param name="audioSystem"></param>
    public static void Initialize(IAssetManagementSystem assetManagementSystem) {
        AssetManagement.assetManagementSystem = assetManagementSystem;
    }
}