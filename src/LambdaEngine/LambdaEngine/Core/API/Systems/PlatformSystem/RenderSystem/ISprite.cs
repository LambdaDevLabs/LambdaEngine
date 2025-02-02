using System.Drawing;
using System.Numerics;

namespace LambdaEngine.PlatformSystem.RenderSystem;

public interface ISprite {
    public bool IsDestroyed { get; protected set; }
    
    public IntPtr TextureHandle { get; }

    public int TextureWidth { get; }

    public int TextureHeight { get; }

    public Vector2 Position { get; set; }

    public Vector2 Scale { get; set; }

    public Color Color { get; set; }

    public void Destroy();
}