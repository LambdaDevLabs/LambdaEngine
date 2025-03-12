using System.Drawing;
using System.Numerics;
using LambdaEngine.PlatformSystem;

namespace LambdaEngine.RenderSystem;

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

    public int LoadTexture(string path);
    
    public void UnloadTexture(int id);

    public int CreateSprite(int textureId);

    public int CreateSpriteWithTexture(string path);
    
    public void DestroySprite(int id);

    public void SetSpriteTexture(int spriteId, int textureId);
    
    public int GetSpriteTexture(int id);

    public void SetSpritePixelsPerUnit(int id, int ppu);

    public int GetSpritePixelsPerUnit(int id);

    public int CreateRenderer(RendererType type);
    
    public void DestroyRenderer(int id);
    
    public void SetRendererPosition(int rendererId, Vector2 position);
    
    public Vector2 GetRendererPosition(int rendererId);
    
    public void SetRendererScale(int rendererId, Vector2 scale);
    
    public Vector2 GetRendererScale(int rendererId);
    
    public void SetRendererSprite(int rendererId, int spriteId);
    
    public int GetRendererTexture(int rendererId);
    
    public void SetRendererText(int rendererId, string text);
    
    public string GetRendererText(int rendererId);

    public void SetRendererColor(int rendererId, Color color);
    
    public Color GetRendererColor(int rendererId);

    public void SetRendererLayer(int rendererId, byte layer);
    
    public byte GetRendererLayer(int rendererId); 
}