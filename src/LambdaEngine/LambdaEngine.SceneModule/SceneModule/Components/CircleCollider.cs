namespace LambdaEngine.SceneModule;

public class CircleCollider : Collider {
    public float Radius {
        get => Physics.GetColliderRadius(colliderId);
        set => Physics.SetColliderRadius(colliderId, value);
    }
    
    internal override void Initialize() {
        colliderId = Physics.CreateCircleCollider();
        
        base.Initialize();
    }
}