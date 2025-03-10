using System.Drawing;
using System.Numerics;
using LambdaEngine.DebugSystem;
using LambdaEngine.PlatformSystem;
using SDL3;

namespace LambdaEngine.RenderSystem;

/// <summary>
/// Provides rendering functionality by directly interfacing with SDL3.
/// </summary>
public class DefaultRenderSystem : IRenderSystem {
    private IntPtr rendererHandle;
    
    public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

    // ReSharper disable class FieldCanBeMadeReadOnly.Global
    // ReSharper disable class MemberCanBePrivate.Global
    // ReSharper disable class ConvertToConstant.Global
    public int initTextureBufferSize = 256;
    public int initSpriteBufferSize = 512;
    public bool autoIncrementTextureBuffer = true;
    public bool autoIncrementSpriteBufferSize = true;

    public VSyncMode vSyncMode = VSyncMode.NORMAL;

    public void Initialize(IPlatformSystem platformSystem) {
        rendererHandle = platformSystem.RendererHandle;
        
        TexturePool.Initialize(rendererHandle, initTextureBufferSize, autoIncrementTextureBuffer);
        SpriteManager.Initialize(initSpriteBufferSize, autoIncrementSpriteBufferSize);

        SDL.SDL_SetRenderVSync(rendererHandle, (int)vSyncMode);
        
        Debug.Log("DefaultRenderSystem initialized.", LogLevel.INFO);
    }

    public ISprite CreateSprite(string path) {
        return Sprite.FromFile(path);
    }
    
    public void Render() {
        SDL.SDL_SetRenderDrawColor(rendererHandle,
            BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
        
        SDL.SDL_RenderClear(rendererHandle);

        float screenWidth = Platform.WindowWidth;  // Get the window width
        float screenHeight = Platform.WindowHeight; // Get the window height

        float camHeight = Camera.Size * 2;
        float worldToScreenScale = screenHeight / camHeight;
        
        foreach (SpriteObject sprite in SpriteManager.AsSpan()) {
            float screenX = (sprite.position.X - Camera.Position.X) * worldToScreenScale + screenWidth / 2;
            float screenY = screenHeight - ((sprite.position.Y - Camera.Position.Y) * worldToScreenScale + screenHeight / 2);

            float scaledWidth = sprite.TextureWidth * sprite.scale.X * worldToScreenScale / Camera.PPU;
            float scaledHeight = sprite.TextureHeight * sprite.scale.Y * worldToScreenScale / Camera.PPU;
            
            SDL.SDL_FRect srcRect = new() {
                x = 0,
                y = 0,
                w = sprite.TextureWidth,
                h = sprite.TextureHeight
            };

            SDL.SDL_FRect dstRect = new() {
                x = screenX - scaledWidth * 0.5f,
                y = screenY - scaledHeight * 0.5f,
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