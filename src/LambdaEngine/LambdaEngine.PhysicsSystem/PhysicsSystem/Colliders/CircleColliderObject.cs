using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

internal struct CircleColliderObject {
    public Vector2 position;
    public float radius;

    public CircleColliderObject() {
        radius = 1;
    }
    
    public CircleColliderObject(float radius) {
        position = Vector2.Zero;
        this.radius = radius;
    }
    
    public CircleColliderObject(float radius, Vector2 position) {
        this.radius = radius;

        this.position = position;
    }
}