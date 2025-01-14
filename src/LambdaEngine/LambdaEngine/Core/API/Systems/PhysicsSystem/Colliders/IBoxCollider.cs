namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// Representation of a box collider in the physics system.
/// </summary>
public interface IBoxCollider : IPhysicsObject {
    /// <summary>
    /// The width of this boxCollider.
    /// </summary>
    public float Width { get; set; }
    
    /// <summary>
    /// The height of this boxCollider.
    /// </summary>
    public float Height { get; set; }
}