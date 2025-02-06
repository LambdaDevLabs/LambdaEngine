using LambdaEngine.PhysicsSystem;

namespace LambdaEngine;

public class BoxCollider : Component, ITransformListener {
    public IBoxCollider boxCollider;

    public override void Initialize() {
        boxCollider = Physics.CreateBoxCollider();
    }

    public void TransformUpdate(Transform transform) {
        boxCollider.Position = transform.Position;
    }
}