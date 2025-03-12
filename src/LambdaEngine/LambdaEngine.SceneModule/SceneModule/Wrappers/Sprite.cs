namespace LambdaEngine.SceneModule;

public class Sprite {
    internal readonly int spriteId;
    
    private Texture texture;

    public Texture Texture {
        get => texture;
        set {
            texture = value;
            Renderer.SetSpriteTexture(spriteId, texture.TextureId);
        }
    }

    private Sprite(Texture texture) {
        this.texture = texture;
        
        spriteId = Renderer.CreateSprite(texture.TextureId);
    }

    public void Destroy() {
        Renderer.DestroySprite(spriteId);
    }

    public static Sprite CreateWithTexture(string path) {
        Texture texture = Texture.Load(path);
        
        return new Sprite(texture);
    }
}