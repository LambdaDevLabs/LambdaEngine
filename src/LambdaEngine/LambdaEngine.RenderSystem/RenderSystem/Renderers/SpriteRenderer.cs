namespace LambdaEngine.RenderSystem;

internal struct SpriteRenderer {
    public int spriteId;

    public int TextureId {
        get => SpriteManager.Get(spriteId).textureId;
    }

    public int PixelsPerUnit {
        get => SpriteManager.Get(spriteId).pixelsPerUnit;
    }

    public SpriteRenderer() {
        spriteId = -1;
    }

    public void SetSprite(int spriteId) {
        this.spriteId = spriteId;
    }
}