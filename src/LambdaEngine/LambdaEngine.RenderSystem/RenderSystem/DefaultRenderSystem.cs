using System.Numerics;
using LambdaEngine.DebugSystem;
using LambdaEngine.PlatformSystem;
using SDL3;

namespace LambdaEngine.RenderSystem;

/// <summary>
/// Provides rendering functionality by directly interfacing with SDL3.
/// </summary>
public class DefaultRenderSystem : IRenderSystem {
    private const int TEXT_SIZE = 8;

    private IntPtr rendererHandle;

    private IntPtr[] layerTextures;
    
    public Color BackgroundColor { get; set; } = new(1, 0.392f, 0.584f, 0.929f);

    // ReSharper disable class FieldCanBeMadeReadOnly.Global
    // ReSharper disable class MemberCanBePrivate.Global
    // ReSharper disable class ConvertToConstant.Global
    public int initTextureBufferSize = 256;
    public int initSpriteBufferSize = 512;
    public int initRendererBufferSize = 512;

    public VSyncMode vSyncMode = VSyncMode.NORMAL;

    public void Initialize(IPlatformSystem platformSystem) {
        rendererHandle = platformSystem.RendererHandle;
        
        TexturePool.Initialize(rendererHandle, initTextureBufferSize);
        SpriteManager.Initialize(initSpriteBufferSize);
        RendererManager.Initialize(initRendererBufferSize);

        layerTextures = new IntPtr[256];

        SDL.SDL_SetRenderVSync(rendererHandle, (int)vSyncMode);
        
        Debug.Log("DefaultRenderSystem initialized.", LogLevel.INFO);
    }
    
    public unsafe void Render() {
        SDL.SDL_SetRenderDrawColorFloat(rendererHandle,
            BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, BackgroundColor.a);
        
        SDL.SDL_RenderClear(rendererHandle);

        int screenWidth = Platform.WindowWidth;  // Get the window width
        int screenHeight = Platform.WindowHeight; // Get the window height

        float halfScreenWidth = screenWidth * 0.5f;
        float halfScreenHeight = screenHeight * 0.5f;

        float camHeight = Camera.Size * 2;
        float worldToScreenScale = screenHeight / camHeight;
        
        bool[] clearedLayers = new bool[layerTextures.Length];
        clearedLayers[0] = true;
        
        byte selectedLayer = 0;
        
        foreach (Renderer renderer in RendererManager.AsSpan()) {
            if (layerTextures[renderer.layer] == IntPtr.Zero) {
                layerTextures[renderer.layer] = new IntPtr(SDL.SDL_CreateTexture(rendererHandle,
                    SDL.SDL_PixelFormat.SDL_PIXELFORMAT_RGBA8888,
                    SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, screenWidth, screenHeight));
            }

            if (renderer.layer != selectedLayer) {
                selectedLayer = renderer.layer;
                SDL.SDL_SetRenderTarget(rendererHandle, layerTextures[renderer.layer]);
                SDL.SDL_SetRenderDrawColor(rendererHandle, 0, 0, 0, 0); // Transparent clear

                if (clearedLayers[selectedLayer] == false) {
                    clearedLayers[selectedLayer] = true;
                    SDL.SDL_RenderClear(rendererHandle);
                }
            }
            
            switch (renderer.rendererType) {
                case RendererType.SPRITE: {
                    float screenX = (renderer.position.X - Camera.Position.X) * worldToScreenScale + halfScreenWidth;
                    float screenY = screenHeight -
                                    ((renderer.position.Y - Camera.Position.Y) * worldToScreenScale + halfScreenHeight);

                    ref SpriteObject sprite = ref SpriteManager.Get(renderer.spriteRenderer.spriteId);

                    float scaledWidth = sprite.TextureWidth * renderer.scale.X * worldToScreenScale / Camera.PPU;
                    float scaledHeight = sprite.TextureHeight * renderer.scale.Y * worldToScreenScale / Camera.PPU;

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

                    SDL.SDL_SetTextureColorModFloat(sprite.TextureHandle, renderer.color.r, renderer.color.g,
                        renderer.color.b);
                    SDL.SDL_SetTextureAlphaModFloat(sprite.TextureHandle, renderer.color.a);
                    SDL.SDL_RenderTexture(rendererHandle, sprite.TextureHandle, ref srcRect, ref dstRect);
                    break;
                }
                case RendererType.TEXT: {
                    float textWidth = TEXT_SIZE * TextRenderer.texts[renderer.textRenderer.textId].Length;
                    float textHeight = TEXT_SIZE;
                    
                    float screenX = (renderer.position.X - Camera.Position.X) * worldToScreenScale + halfScreenWidth;
                    float screenY = screenHeight -
                                    ((renderer.position.Y - Camera.Position.Y) * worldToScreenScale + halfScreenHeight);

                    float finalX = screenX / renderer.scale.X - textWidth * 0.5f;
                    float finalY = screenY / renderer.scale.Y - textHeight * 0.5f;

                    SDL.SDL_SetRenderScale(rendererHandle, renderer.scale.X, renderer.scale.Y);
                    SDL.SDL_SetRenderDrawColorFloat(rendererHandle, renderer.color.r, renderer.color.g,
                        renderer.color.b, renderer.color.a);
                    SDL.SDL_RenderDebugText(rendererHandle, finalX, finalY, TextRenderer.texts[renderer.textRenderer.textId]);
                    
                    SDL.SDL_SetRenderScale(rendererHandle, 1, 1);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException($"Unsupported renderer type: {renderer.rendererType}.");
            }
        }

        SDL.SDL_SetRenderTarget(rendererHandle, IntPtr.Zero);
        
        SDL.SDL_FRect rect = new() {
            x = 0,
            y = 0,
            w = screenWidth,
            h = screenHeight
        };
        
        foreach (IntPtr layerTexture in layerTextures) {
            if (layerTexture != IntPtr.Zero) {
                SDL.SDL_RenderTexture(rendererHandle, layerTexture, ref rect, ref rect);
            }
        }
        
        SDL.SDL_RenderPresent(rendererHandle);
    }

    public int LoadTexture(string path) {
        return TexturePool.LoadTexture(path);
    }
    
    public void UnloadTexture(int id) {
        TexturePool.UnloadTexture(id);
    }
    
    public int CreateSprite(int textureId) {
        return SpriteManager.CreateSpriteFromTexture(textureId);
    }
    
    public int CreateSpriteWithTexture(string path) {
        return SpriteManager.CreateSpriteFromFile(path);
    }

    public void DestroySprite(int spriteId) {
        SpriteManager.DestroySprite(spriteId);
    }
    
    public void SetSpriteTexture(int spriteId, int textureId) {
        SpriteManager.Get(spriteId).textureId = textureId;
    }
    
    public int GetSpriteTexture(int id) {
        return SpriteManager.Get(id).textureId;
    }
    
    public void SetSpritePixelsPerUnit(int id, int ppu) {
        SpriteManager.Get(id).pixelsPerUnit = ppu;
    }
    
    public int GetSpritePixelsPerUnit(int id) {
        return SpriteManager.Get(id).pixelsPerUnit;
    }
    
    public int CreateRenderer(RendererType type) {
        return RendererManager.CreateRenderer(type);
    }
    
    public void DestroyRenderer(int id) {
        RendererManager.DestroyRenderer(id);
    }
    
    public void SetRendererPosition(int rendererId, Vector2 position) {
        RendererManager.GetRenderer(rendererId).position = position;
    }
    
    public Vector2 GetRendererPosition(int rendererId) {
        return RendererManager.GetRenderer(rendererId).position;
    }
    
    public void SetRendererScale(int rendererId, Vector2 scale) {
        RendererManager.GetRenderer(rendererId).scale = scale;
    }
    
    public Vector2 GetRendererScale(int rendererId) {
        return RendererManager.GetRenderer(rendererId).scale;
    }
    
    public void SetRendererSprite(int rendererId, int spriteId) {
        ref Renderer renderer = ref RendererManager.GetRenderer(rendererId);
        
        if (renderer.rendererType == RendererType.SPRITE) {
            renderer.spriteRenderer.SetSprite(spriteId);
        }
        else {
            throw new Exception("Unable to set renderer sprite; this is not a sprite renderer.");
        }
    }
    
    public int GetRendererTexture(int rendererId) {
        ref Renderer renderer = ref RendererManager.GetRenderer(rendererId);
        
        if (renderer.rendererType == RendererType.SPRITE) {
            return renderer.spriteRenderer.spriteId;
        }
        else {
            throw new Exception("Unable to get renderer sprite; this is not a sprite renderer.");
        }
    }
    
    public void SetRendererText(int rendererId, string text) {
        ref Renderer renderer = ref RendererManager.GetRenderer(rendererId);
        
        if (renderer.rendererType == RendererType.TEXT) {
            TextRenderer.texts[renderer.textRenderer.textId] = text;
        }
        else {
            throw new Exception("Unable to set renderer text; this is not a text renderer.");
        }
    }
    
    public string GetRendererText(int rendererId) {
        ref Renderer renderer = ref RendererManager.GetRenderer(rendererId);
        
        if (renderer.rendererType == RendererType.TEXT) {
            return TextRenderer.texts[renderer.textRenderer.textId];
        }
        else {
            throw new Exception("Unable to get renderer text; this is not a text renderer.");
        }
    }
    
    public void SetRendererColor(int rendererId, Color color) {
        RendererManager.GetRenderer(rendererId).color = color;
    }

    public Color GetRendererColor(int rendererId) {
        return RendererManager.GetRenderer(rendererId).color;
    }

    public void SetRendererLayer(int rendererId, byte layer) {
        RendererManager.GetRenderer(rendererId).layer = layer;
    }

    public byte GetRendererLayer(int rendererId) {
        return RendererManager.GetRenderer(rendererId).layer;
    }
}