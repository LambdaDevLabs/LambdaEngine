using LambdaEngine.PlatformSystem.AudioSystem;
using LambdaEngine.PlatformSystem.InputSystem;
using LambdaEngine.PlatformSystem.RenderSystem;

namespace LambdaEngine.PlatformSystem;

/// <summary>
/// Interface for the platform system.
/// </summary>
public interface IPlatformSystem {
    public IRenderSystem RenderSystem { get; }
    public IInputSystem InputSystem { get; }
    public IAudioSystem AudioSystem { get; }
}