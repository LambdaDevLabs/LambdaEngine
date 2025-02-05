using System.Drawing;
using LambdaEngine.DebugSystem;
using SDL3;

namespace LambdaEngine.PlatformSystem.RenderSystem;

/// <summary>
/// Provides rendering functionality by directly interfacing with SDL3.
/// </summary>
public class DefaultRenderSystem : IRenderSystem {
    private IntPtr rendererHandle;
    
    public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

    public void Initialize(IPlatformSystem platformSystem) {
        rendererHandle = platformSystem.RendererHandle;
        
        TexturePool.Initialize(rendererHandle, 256);
        SpriteManager.Initialize(512);
        
        Debug.Log($"DefaultRenderSystem initialized.", LogLevel.INFO);
    }

    public ISprite CreateSprite(string path) {
        return Sprite.FromFile(path);
    }

    public void Render() {
        SDL.SDL_SetRenderDrawColor(rendererHandle,
            BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
        
        SDL.SDL_RenderClear(rendererHandle);

        foreach (SpriteObject sprite in SpriteManager.AsSpan()) {
            SDL.SDL_FRect srcRect = new() {
                x = 0,
                y = 0,
                w = sprite.TextureWidth,
                h = sprite.TextureHeight
            };
            
            float scaledWidth = sprite.TextureWidth * sprite.scale.X;
            float scaledHeight = sprite.TextureHeight * sprite.scale.Y;
            
            SDL.SDL_FRect dstRect = new() {
                x = sprite.position.X - scaledWidth * 0.5f,
                y = sprite.position.Y - scaledHeight * 0.5f,
                w = scaledWidth,
                h = scaledHeight
            };

            SDL.SDL_SetTextureColorMod(sprite.TextureHandle, sprite.color.R, sprite.color.G, sprite.color.B);
            SDL.SDL_SetTextureAlphaMod(sprite.TextureHandle, sprite.color.A);
            SDL.SDL_RenderTexture(rendererHandle, sprite.TextureHandle, ref srcRect, ref dstRect);
        }
        
        SDL.SDL_RenderPresent(rendererHandle);
    }
}