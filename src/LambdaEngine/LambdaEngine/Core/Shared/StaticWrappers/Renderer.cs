using LambdaEngine.PlatformSystem.RenderSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the RenderSystem.
/// </summary>
public static class Renderer {
    private static IRenderSystem renderSystem;

    /// <summary>
    /// Initializes the RenderSystem wrapper.
    /// </summary>
    /// <param name="renderSystem"></param>
    public static void Connect(IRenderSystem renderSystem) {
        Renderer.renderSystem = renderSystem;
    }

    /// <summary>
    /// Creates a new sprite based on the specified path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static ISprite CreateSprite(string path) {
        return renderSystem.CreateSprite(path);
    }
}