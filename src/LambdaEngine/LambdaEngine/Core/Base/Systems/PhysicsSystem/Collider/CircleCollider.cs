using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

public class CircleCollider : ICircleCollider {
    private readonly int id;
    
    public Vector2 Position {
        get => ColliderManager.Get(id).circleCollider.position;
        set => ColliderManager.Get(id).circleCollider.position = value;
    }

    public bool IsDirty { get; set; }

    public float Radius {
        get => ColliderManager.Get(id).circleCollider.radius;
        set => ColliderManager.Get(id).circleCollider.radius = value;
    }

    private CircleCollider(int id) {
        this.id = this.id;
    }
    
    public void Destroy() {
        ColliderManager.DestroyCollider(id);
    }
    
    public static CircleCollider Create() {
        return new CircleCollider(ColliderManager.CreateNewCircleCollider());
    }
}