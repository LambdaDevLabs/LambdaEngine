using LambdaEngine.AssetManagementSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the AssetManagementSystem.
/// </summary>
public static class AssetManagement {
    private static IAssetManagementSystem assetManagementSystem;

    /// <summary>
    /// Initializes the AssetManagementSystem wrapper.
    /// </summary>
    /// <param name="assetManagementSystem"></param>
    public static void Connect(IAssetManagementSystem assetManagementSystem) {
        AssetManagement.assetManagementSystem = assetManagementSystem;
    }
}