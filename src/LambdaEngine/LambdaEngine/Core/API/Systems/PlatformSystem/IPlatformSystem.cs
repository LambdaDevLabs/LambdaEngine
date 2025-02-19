using LambdaEngine.AudioSystem;
using LambdaEngine.InputSystem;
using LambdaEngine.RenderSystem;

namespace LambdaEngine.PlatformSystem;

/// <summary>
/// Interface for the platform system.
/// </summary>
public interface IPlatformSystem {
    public IRenderSystem RenderSystem { get; }
    public IInputSystem InputSystem { get; }
    public IAudioSystem AudioSystem { get; }
    
    public IntPtr WindowHandle { get; protected set; }
    public IntPtr RendererHandle { get; protected set; }
    
    public int WindowWidth { get; protected set; }
    public int WindowHeight { get; protected set; }

    /// <summary>
    /// Sets the size of the window.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetWindowSize(int width, int height);

    /// <summary>
    /// Creates a new window.
    /// </summary>
    /// <returns></returns>
    public bool CreateWindow();
    
    /// <summary>
    /// Initializes the platform system.
    /// </summary>
    public void Initialize();
}