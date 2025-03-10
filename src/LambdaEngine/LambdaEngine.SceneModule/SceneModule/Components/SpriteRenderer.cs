using LambdaEngine.RenderSystem;

namespace LambdaEngine.SceneModule;

public class SpriteRenderer : Component, ITransformListener {
    public ISprite sprite;

    internal override void Initialize() {
        transform.RegisterTransformListener(this);
    }

    /// <summary>
    /// Creates a new sprite from the specified path and assigns it to this <see cref="SpriteRenderer"/>.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ISprite CreateSprite(string path) {
        sprite = Renderer.CreateSprite(path);

        sprite.Position = transform.Position;
        
        return sprite;
    }

    public void TransformUpdate(Transform transform) {
        if (sprite == null) {
            return;
        }
        
        sprite.Position = transform.Position;
    }

    internal override void DestroyComponent() {
        transform.UnregisterTransformListener(this);
        
        sprite.Destroy();

        sprite = null!;
        
        base.DestroyComponent();
    }
}