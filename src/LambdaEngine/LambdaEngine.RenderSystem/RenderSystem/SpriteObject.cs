using System.Drawing;
using System.Numerics;
using SDL3;

namespace LambdaEngine.RenderSystem;

public unsafe struct SpriteObject {
    private readonly SDL.SDL_Texture* textureHandle;
    public Vector2 position;
    public Vector2 scale;
    public Color color;
    public int pixelsPerUnit;
    
    public int TextureWidth {
        get => textureHandle->w;
    }

    public int TextureHeight {
        get => textureHandle->h;
    }

    public IntPtr TextureHandle {
        get => new(textureHandle);
    }

    public SpriteObject(SDL.SDL_Texture* texture) {
        textureHandle = texture;
        
        position = Vector2.Zero;
        scale = Vector2.One;
        color = Color.White;
        
        pixelsPerUnit = 100;
    }
}