using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// Representation of a rigidbody in the physics system.
/// </summary>
public interface IRigidbody : IPhysicsObject {
    /// <summary>
    /// The current velocity of this rigidbody.
    /// </summary>
    public Vector2 Velocity { get; set; }
    
    /// <summary>
    /// The mass of this rigidbody.
    /// </summary>
    public float Mass { get; set; }
}