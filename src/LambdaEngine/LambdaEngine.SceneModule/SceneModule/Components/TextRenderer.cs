using System.Numerics;
using LambdaEngine.DebugSystem;
using LambdaEngine.RenderSystem;

namespace LambdaEngine.SceneModule;

public class TextRenderer : Component, ITransformListener {
    internal readonly int textRendererId;

    private Vector2 scale;
    private string text;
    private Color color;
    private sbyte layer;

    public Vector2 Scale {
        get => scale;
        set {
            scale = value;
            Renderer.SetRendererScale(textRendererId, scale);
        }
    }
    public string Text {
        get => text;
        set {
            text = value;

            Renderer.SetRendererText(textRendererId, text);
        }
    }

    public Color Color {
        get => color;
        set {
            color = value;
            Renderer.SetRendererColor(textRendererId, color);
        }
    }
    
    public sbyte Layer {
        get => layer;
        set {
            layer = value;
            Renderer.SetRendererLayer(textRendererId, (byte)(layer + 128));
        }
    }

    public TextRenderer() {
        textRendererId = Renderer.CreateRenderer(RendererType.TEXT);

        scale = Vector2.One;
        color = Color.White;
        
        Renderer.SetRendererPosition(textRendererId, Vector2.Zero);
        Renderer.SetRendererScale(textRendererId, scale);
        Renderer.SetRendererColor(textRendererId, color);
        Renderer.SetRendererText(textRendererId, string.Empty);
    }

    internal override void Initialize() {
        transform.RegisterTransformListener(this);
    }

    public void TransformUpdate(Transform transform) {
        Renderer.SetRendererPosition(textRendererId, transform.Position);
    }

    internal override void DestroyComponent() {
        transform.UnregisterTransformListener(this);
        
        Renderer.DestroyRenderer(textRendererId);

        text = null!;
        
        base.DestroyComponent();
    }
}