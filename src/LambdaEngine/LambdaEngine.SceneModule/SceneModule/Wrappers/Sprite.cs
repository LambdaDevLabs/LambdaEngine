namespace LambdaEngine.SceneModule;

public class Sprite {
    internal readonly int spriteId;
    
    private Texture texture;

    /// <summary>
    /// The texture of this sprite.
    /// </summary>
    public Texture Texture {
        get => texture;
        set {
            texture = value;
            Renderer.SetSpriteTexture(spriteId, texture.TextureId);
        }
    }

    public Sprite(Texture texture) {
        this.texture = texture;
        
        spriteId = Renderer.CreateSprite(texture.TextureId);
    }

    /// <summary>
    /// Destroys this sprite.
    /// </summary>
    public void Destroy() {
        Renderer.DestroySprite(spriteId);
    }

    /// <summary>
    /// <para>Creates a new sprite with a texture from the specified path.</para>
    /// <para>If the required texture does not already exist, it is created.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Sprite CreateWithTexture(string path) {
        Texture texture = Texture.Load(path);
        
        return new Sprite(texture);
    }

    ~Sprite() {
        Renderer.DestroySprite(spriteId);
    }
}