using System.Drawing;

namespace LambdaEngine.PlatformSystem.RenderSystem;

public interface IRenderSystem {
    /// <summary>
    /// The background color of the screen.
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// Initializes the render system.
    /// </summary>
    /// <param name="platformSystem"></param>
    public void Initialize(IPlatformSystem platformSystem);

    /// <summary>
    /// Renders all existing sprites to the screen.
    /// </summary>
    public void Render();
}