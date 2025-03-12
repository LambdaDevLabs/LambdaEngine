namespace LambdaEngine.RenderSystem;

internal struct TextRenderer {
    public static readonly Dictionary<int, string> texts = new();
    public static readonly IdProvider textIdProvider = new(16, 4);
    
    public int textId;
}