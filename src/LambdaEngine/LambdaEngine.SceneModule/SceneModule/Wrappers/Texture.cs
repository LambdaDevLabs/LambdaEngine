namespace LambdaEngine.SceneModule;

public class Texture {
    internal readonly int textureId;

    internal int TextureId {
        get => textureId;
    }

    private Texture(int textureId) {
        this.textureId = textureId;
    }

    public void Unload() {
        Renderer.UnloadTexture(textureId);
    }

    public static Texture Load(string path) {
        return new Texture(Renderer.LoadTexture(path));
    }
}