namespace LambdaEngine.SceneModule;

/// <summary>
/// An axis-aligned box collider.
/// </summary>
public class BoxCollider : Collider {
    /// <summary>
    /// The width of the collider.
    /// </summary>
    public float Width {
        get => Physics.GetColliderWidth(colliderId);
        set => Physics.SetColliderWidth(colliderId, value);
    }

    /// <summary>
    /// The height of the collider.
    /// </summary>
    public float Height {
        get => Physics.GetColliderHeight(colliderId);
        set => Physics.SetColliderHeight(colliderId, value);
    }
    
    internal override void Initialize() {
        colliderId = Physics.CreateBoxCollider();
        
        base.Initialize();
    }
}