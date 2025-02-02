using LambdaEngine.PlatformSystem.RenderSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the RenderSystem.
/// </summary>
public static class Render {
    private static IRenderSystem renderSystem;

    /// <summary>
    /// Initializes the RenderSystem wrapper.
    /// </summary>
    /// <param name="renderSystem"></param>
    public static void Connect(IRenderSystem renderSystem) {
        Render.renderSystem = renderSystem;
    }
}