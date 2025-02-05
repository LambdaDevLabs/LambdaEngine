using LambdaEngine.PlatformSystem.RenderSystem;

namespace LambdaEngine;

public class SpriteRenderer : Component, ITransformListener {
    public ISprite sprite;

    public override void Initialize() {
        transform.RegisterTransformListener(this);
    }

    /// <summary>
    /// Creates a new sprite from the specified path and assigns it to this <see cref="SpriteRenderer"/>.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ISprite CreateSprite(string path) {
        sprite = Renderer.CreateSprite(path);
        
        return sprite;
    }

    public void TransformUpdate(Transform transform) {
        if (sprite == null) {
            return;
        }
        Debug.Log("Propagated position to sprite.");
        sprite.Position = transform.Position;
    }
}