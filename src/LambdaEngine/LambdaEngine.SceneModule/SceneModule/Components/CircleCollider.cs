namespace LambdaEngine.SceneModule;

/// <summary>
/// A circle collider.
/// </summary>
public class CircleCollider : Collider {
    /// <summary>
    /// The radius of the collider.
    /// </summary>
    public float Radius {
        get => Physics.GetColliderRadius(colliderId);
        set => Physics.SetColliderRadius(colliderId, value);
    }
    
    internal override void Initialize() {
        colliderId = Physics.CreateCircleCollider();
        
        base.Initialize();
    }
}