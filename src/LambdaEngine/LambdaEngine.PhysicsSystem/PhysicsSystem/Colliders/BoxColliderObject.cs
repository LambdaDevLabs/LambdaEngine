using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

internal struct BoxColliderObject {
    public Vector2 position;
    public float width;
    public float height;

    public BoxColliderObject() {
        position = Vector2.Zero;
        width = 1;
        height = 1;
    }

    public BoxColliderObject(float width, float height) {
        position = Vector2.Zero;
        this.width = width;
        this.height = height;
    }
    
    public BoxColliderObject(float width, float height, Vector2 position) {
        this.width = width;
        this.height = height;

        this.position = position;
    }
}