using System.Drawing;
using System.Numerics;
using LambdaEngine.RenderSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the RenderSystem.
/// </summary>
public static class Renderer {
    private static IRenderSystem renderSystem;

    /// <summary>
    /// Initializes the RenderSystem wrapper.
    /// </summary>
    /// <param name="renderSystem"></param>
    public static void Connect(IRenderSystem renderSystem) {
        Renderer.renderSystem = renderSystem;
    }

    /// <summary>
    /// Renders all existing sprites to the screen.
    /// </summary>
    public static void Render() {
        renderSystem.Render();
    }

    public static int LoadTexture(string path) {
        return renderSystem.LoadTexture(path);
    }

    public static void UnloadTexture(int id) {
        renderSystem.UnloadTexture(id);
    }

    public static int CreateSprite(int textureId) {
        return renderSystem.CreateSprite(textureId);
    }

    public static int CreateSpriteWithTexture(string path) {
        return renderSystem.CreateSpriteWithTexture(path);
    }

    public static void DestroySprite(int id) {
        renderSystem.DestroySprite(id);
    }

    public static void SetSpriteTexture(int spriteId, int textureId) {
        renderSystem.SetSpriteTexture(spriteId, textureId);
    }

    public static int GetSpriteTexture(int id) {
        return renderSystem.GetSpriteTexture(id);
    }

    public static void SetSpritePixelsPerUnit(int id, int ppu) {
        renderSystem.SetSpritePixelsPerUnit(id, ppu);
    }

    public static int GetSpritePixelsPerUnit(int id) {
        return renderSystem.GetSpritePixelsPerUnit(id);
    }

    public static int CreateRenderer(RendererType type) {
        return renderSystem.CreateRenderer(type);
    }

    public static void DestroyRenderer(int id) {
        renderSystem.DestroyRenderer(id);
    }

    public static void SetRendererPosition(int rendererId, Vector2 position) {
        renderSystem.SetRendererPosition(rendererId, position);
    }

    public static Vector2 GetRendererPosition(int rendererId) {
        return renderSystem.GetRendererPosition(rendererId);
    }

    public static void SetRendererScale(int rendererId, Vector2 scale) {
        renderSystem.SetRendererScale(rendererId, scale);
    }

    public static Vector2 GetRendererScale(int rendererId) {
        return renderSystem.GetRendererScale(rendererId);
    }

    public static void SetRendererSprite(int rendererId, int spriteId) {
        renderSystem.SetRendererSprite(rendererId, spriteId);
    }

    public static int GetRendererTexture(int rendererId) {
        return renderSystem.GetRendererTexture(rendererId);
    }

    public static void SetRendererText(int rendererId, string text) {
        renderSystem.SetRendererText(rendererId, text);
    }

    public static string GetRendererText(int rendererId) {
        return renderSystem.GetRendererText(rendererId);
    }

    public static void SetRendererColor(int rendererId, Color color) {
        renderSystem.SetRendererColor(rendererId, color);
    }

    public static Color GetRendererColor(int rendererId) {
        return renderSystem.GetRendererColor(rendererId);
    }
    
    public static void SetRendererLayer(int rendererId, byte layer) {
        renderSystem.SetRendererLayer(rendererId, layer);
    }

    public static byte GetRendererLayer(int rendererId) {
        return renderSystem.GetRendererLayer(rendererId);
    }
}