using System.Runtime.InteropServices;

namespace LambdaEngine.PhysicsSystem;

[StructLayout(LayoutKind.Explicit)]
internal struct ColliderObject {
    [FieldOffset(0)] public BoxColliderObject boxCollider;
    [FieldOffset(0)] public CircleColliderObject circleCollider;

    // Alignment: 16 bytes (BoxCollider - larger than CircleCollider)
    [FieldOffset(16)] public readonly int id;
    
    [FieldOffset(20)] public readonly ColliderType type;

    // 3 bytes of padding to ensure alignment to 4 (size 24)
    [FieldOffset(23)] private readonly byte padding = 0;

    public ColliderObject(int id, BoxColliderObject boxCollider) {
        this.id = id;
        this.boxCollider = boxCollider;

        type = ColliderType.BOX;
    }
    
    public ColliderObject(int id, CircleColliderObject circleCollider) {
        this.id = id;
        this.circleCollider = circleCollider;

        type = ColliderType.CIRCLE;
    }
}