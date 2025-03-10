using System.Numerics;

namespace LambdaEngine.RenderSystem;

public static class Camera {
    internal const float PPU = 100;
    
    public static Vector2 Position { get; set; }
    public static float Size { get; set; } = 5.0f;

    public static Vector2 WorldToScreenPos(Vector2 worldPosition) {
        float worldToScreenScale = Platform.WindowHeight / (Size * 2); // Pixels per world unit
        
        float screenX = (worldPosition.X - Position.X) * worldToScreenScale + Platform.WindowWidth * 0.5f;
        float screenY = Platform.WindowHeight - ((worldPosition.Y - Camera.Position.Y) * worldToScreenScale + Platform.WindowHeight * 0.5f);

        return new Vector2(screenX, screenY);
    }
    
    public static Vector2 ScreenToWorldPos(Vector2 screenPosition) {
        float worldToScreenScale = Platform.WindowHeight / (Size * 2); // Pixels per world unit
        
        float worldX = (screenPosition.X - Platform.WindowWidth * 0.5f) / worldToScreenScale + Position.X;
        float worldY = ((Platform.WindowHeight - screenPosition.Y) - Platform.WindowHeight * 0.5f) / worldToScreenScale + Position.Y;

        return new Vector2(worldX, worldY);
    }
}