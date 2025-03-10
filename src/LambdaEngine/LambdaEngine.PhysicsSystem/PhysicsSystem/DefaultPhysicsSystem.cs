using System.Numerics;
using LambdaEngine.DebugSystem;

namespace LambdaEngine.PhysicsSystem;

public class DefaultPhysicsSystem : IPhysicsSystem {
    public int initColliderManagerBufferSize = 64;
    public bool colliderManagerAutoBufferIncrement = true;

    private Dictionary<int, List<int>> collisions;
    
    public void Initialize() {
        ColliderManager.Initialize(initColliderManagerBufferSize, colliderManagerAutoBufferIncrement);
        
        collisions = new Dictionary<int, List<int>>(initColliderManagerBufferSize);
        
        Debug.Log("DefaultPhysicsSystem initialized.", LogLevel.INFO);
    }
    
    public void SimulatePhysics() {
        collisions.Clear();
        collisions.EnsureCapacity(ColliderManager.Capacity);
        
        Span<Collision> rawCollisions = CollisionDetection.CheckCollisions(ColliderManager.AsSpan()).Span;

        for (int i = 0; i < rawCollisions.Length; i++) {
            ref Collision collision = ref rawCollisions[i];

            if (!collisions.ContainsKey(collision.a)) {
                collisions.Add(collision.a, new List<int>(8));
            }
            if (!collisions.ContainsKey(collision.b)) {
                collisions.Add(collision.b, new List<int>(8));
            }
            
            collisions[collision.a].Add(collision.b);
            collisions[collision.b].Add(collision.a);
        }
    }

    public ICollection<int> GetCollisionsFor(int id) {
        return collisions.TryGetValue(id, out List<int> collisions_) ? collisions_ : [];
    }

    public int CreateBoxCollider() {
        return ColliderManager.CreateNewBoxCollider();
    }
    
    public int CreateCircleCollider() {
        return ColliderManager.CreateNewCircleCollider();
    }
    
    public Vector2 GetColliderPosition(int id) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        return cObj.type switch {
            ColliderType.BOX => cObj.boxCollider.position,
            ColliderType.CIRCLE => cObj.circleCollider.position,
            _ => throw new Exception("Unknown collider type.")
        };
    }
    
    public void SetColliderPosition(int id, Vector2 position) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        switch (cObj.type) {
            case ColliderType.BOX:
                cObj.boxCollider.position = position;
                break;
            
            case ColliderType.CIRCLE:
                cObj.circleCollider.position = position;
                break;
            
            default:
                throw new Exception("Unknown collider type.");
        }
    }
    
    public float GetColliderWidth(int id) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.BOX) {
            throw new Exception("Unknown collider type.");
        }
        
        return cObj.boxCollider.width;
    }
    
    public void SetColliderWidth(int id, float width) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.BOX) {
            throw new Exception("Unknown collider type.");
        }
        
        cObj.boxCollider.width = width;
    }
    
    public float GetColliderHeight(int id) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.BOX) {
            throw new Exception("Unknown collider type.");
        }
        
        return cObj.boxCollider.height;
    }
    
    public void SetColliderHeight(int id, float height) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.BOX) {
            throw new Exception("Unknown collider type.");
        }
        
        cObj.boxCollider.height = height;
    }
    
    public float GetColliderRadius(int id) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.CIRCLE) {
            throw new Exception("Unknown collider type.");
        }
        
        return cObj.circleCollider.radius;
    }
    
    public void SetColliderRadius(int id, float radius) {
        ref ColliderObject cObj = ref ColliderManager.Get(id);

        if (cObj.type != ColliderType.CIRCLE) {
            throw new Exception("Unknown collider type.");
        }
        
        cObj.circleCollider.radius = radius;
    }

    public void DestroyCollider(int id) {
        ColliderManager.DestroyCollider(id);
    }
}