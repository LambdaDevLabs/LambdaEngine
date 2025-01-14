namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// Representation of a circular collider in the physics system.
/// </summary>
public interface ICircleCollider : IPhysicsObject {
    /// <summary>
    /// The radius of this circleCollider.
    /// </summary>
    public float Radius { get; set; }
}