namespace LambdaEngine.SceneModule;

public class Texture {
    internal readonly int textureId;

    internal int TextureId {
        get => textureId;
    }

    private Texture(int textureId) {
        this.textureId = textureId;
    }

    /// <summary>
    /// Unloads this texture.
    /// </summary>
    public void Unload() {
        Renderer.UnloadTexture(textureId);
    }

    /// <summary>
    /// <para>Loads a new texture from the specified path.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture Load(string path) {
        return new Texture(Renderer.LoadTexture(path));
    }

    ~Texture() {
        Renderer.UnloadTexture(textureId);
    }
}