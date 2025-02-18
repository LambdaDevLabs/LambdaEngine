using System.Drawing;
using System.Numerics;
using SDL3;

namespace LambdaEngine.PlatformSystem.RenderSystem;

public class Sprite : ISprite {
    private readonly int id;
    
    public bool IsDestroyed { get; set; }

    public int TextureWidth {
        get => SpriteManager.Get(id).TextureWidth;
    }

    public int TextureHeight {
        get => SpriteManager.Get(id).TextureHeight;
    }

    public Vector2 Position {
        get => SpriteManager.Get(id).position;
        set => SpriteManager.Get(id).position = value;
    }

    public Vector2 Scale {
        get => SpriteManager.Get(id).scale;
        set => SpriteManager.Get(id).scale = value;
    }

    public Color Color {
        get => SpriteManager.Get(id).color;
        set => SpriteManager.Get(id).color = value;
    }

    public IntPtr TextureHandle {
        get => SpriteManager.Get(id).TextureHandle;
    }

    private Sprite(int id) {
        this.id = id;
    }

    public void Destroy() {
        if (IsDestroyed) {
            return;
        }

        SpriteManager.DestroySprite(id);
        IsDestroyed = true;
    }
    
    public static Sprite FromFile(string path) {
        return new Sprite(SpriteManager.CreateSpriteFromFile(path));
    }

    public static unsafe Sprite FromSprite(Sprite sprite) {
        return new Sprite(SpriteManager.CreateSpriteFromTexture((SDL.SDL_Texture*)sprite.TextureHandle));
    }
}