using LambdaEngine.PhysicsSystem;

namespace LambdaEngine.SceneModule;

public class BoxCollider : Component, ITransformListener {
    private IBoxCollider boxCollider;

    internal override void Initialize() {
        boxCollider = Physics.CreateBoxCollider(); 
    }

    public void TransformUpdate(Transform transform) {
        boxCollider.Position = transform.Position;
    }

    internal override void DestroyComponent() {
        boxCollider.Destroy();
        boxCollider = null!;
        
        base.DestroyComponent();
    }
}