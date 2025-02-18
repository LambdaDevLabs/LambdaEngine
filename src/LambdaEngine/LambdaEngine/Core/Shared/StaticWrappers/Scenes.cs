using LambdaEngine.SceneModule;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the SceneModule.
/// </summary>
public static class Scenes {
    private static ISceneModule sceneModule;
    
    /// <summary>
    /// Initializes the SceneModule wrapper.
    /// </summary>
    /// <param name="sceneModule"></param>
    public static void Connect(ISceneModule sceneModule) {
        Scenes.sceneModule = sceneModule;
    }

    public static void Initialize() {
        sceneModule.Initialize();
    }
}