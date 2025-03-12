namespace LambdaEngine.RenderSystem;

internal unsafe struct SpriteObject {
    public int textureId;
    public int pixelsPerUnit = 100;
    
    public int TextureWidth {
        get => TexturePool.GetTexture(textureId)->w;
    }

    public int TextureHeight {
        get => TexturePool.GetTexture(textureId)->h;
    }

    public IntPtr TextureHandle {
        get => new(TexturePool.GetTexture(textureId));
    }

    public SpriteObject(int textureId) {
        this.textureId = textureId;
    }
}