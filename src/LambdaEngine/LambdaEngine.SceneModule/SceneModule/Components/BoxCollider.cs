using LambdaEngine.DebugSystem;

namespace LambdaEngine.SceneModule;

public class BoxCollider : Collider {
    public float Width {
        get => Physics.GetColliderWidth(colliderId);
        set => Physics.SetColliderWidth(colliderId, value);
    }

    public float Height {
        get => Physics.GetColliderHeight(colliderId);
        set => Physics.SetColliderHeight(colliderId, value);
    }
    
    internal override void Initialize() {
        colliderId = Physics.CreateBoxCollider();
        
        base.Initialize();
    }
}