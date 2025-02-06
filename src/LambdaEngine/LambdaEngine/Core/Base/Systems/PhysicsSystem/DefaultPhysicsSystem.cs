using LambdaEngine.DebugSystem;
using LambdaEngine.PlatformSystem.RenderSystem;

namespace LambdaEngine.PhysicsSystem;

public class DefaultPhysicsSystem : IPhysicsSystem {
    public int initColliderManagerBufferSize;
    public bool colliderManagerAutoBufferIncrement;
    
    public void Initialize() {
        ColliderManager.Initialize(initColliderManagerBufferSize, colliderManagerAutoBufferIncrement);
        
        Debug.Log("DefaultPhysicsSystem initialized.", LogLevel.INFO);
    }
    
    public void SimulatePhysics() {
        CollisionDetection.CheckCollisions(ColliderManager.AsSpan());
    }
    
    public ICircleCollider CreateCircleCollider() {
        return CircleCollider.Create();
    }

    public IBoxCollider CreateBoxCollider() {
        return BoxCollider.Create();
    }

    public IRigidbody CreateRigidbody() {
        throw new NotImplementedException();
    }
}