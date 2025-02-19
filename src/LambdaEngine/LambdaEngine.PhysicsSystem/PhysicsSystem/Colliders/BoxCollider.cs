using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

public class BoxCollider : IBoxCollider {
    private readonly int id;
    
    public Vector2 Position {
        get => ColliderManager.Get(id).boxCollider.position;
        set => ColliderManager.Get(id).boxCollider.position = value;
    }

    public bool IsDirty { get; set; }

    public float Width {
        get => ColliderManager.Get(id).boxCollider.width;
        set => ColliderManager.Get(id).boxCollider.width = value;
    }

    public float Height {
        get => ColliderManager.Get(id).boxCollider.height;
        set => ColliderManager.Get(id).boxCollider.height = value;
    }

    private BoxCollider(int id) {
        this.id = id;
    }
    
    public void Destroy() {
        ColliderManager.DestroyCollider(id);
    }
    
    public static BoxCollider Create() {
        return new BoxCollider(ColliderManager.CreateNewBoxCollider());
    }
}