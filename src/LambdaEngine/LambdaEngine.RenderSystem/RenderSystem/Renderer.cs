using System.Numerics;
using System.Runtime.InteropServices;

namespace LambdaEngine.RenderSystem;

[StructLayout(LayoutKind.Explicit)]
internal struct Renderer {
    [FieldOffset(0)] public RendererType rendererType;
    
    [FieldOffset(4)] public Vector2 position;
    [FieldOffset(12)] public Vector2 scale;
    [FieldOffset(20)] public Color color;

    [FieldOffset(36)] public byte layer;
    
    [FieldOffset(37)] public SpriteRenderer spriteRenderer;
    [FieldOffset(37)] public TextRenderer textRenderer;

    public Renderer(RendererType rendererType) {
        this.rendererType = rendererType;
        
        position = Vector2.Zero;
        scale = Vector2.One;
        color = Color.White;

        switch (this.rendererType) {
            case RendererType.SPRITE:
                spriteRenderer = new SpriteRenderer();
                break;
            
            case RendererType.TEXT:
                textRenderer = new TextRenderer();
                break;
        }
    }
}