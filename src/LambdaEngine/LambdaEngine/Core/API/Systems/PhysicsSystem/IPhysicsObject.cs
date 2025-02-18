using System.Numerics;

namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// Representation of a physicsObject.
/// </summary>
public interface IPhysicsObject {
    /// <summary>
    /// World position of this physicsObject.
    /// </summary>
    public Vector2 Position { get; set; }
    
    /// <summary>
    /// Indicates whether this physicsObject has received any changes during the physics
    /// simulation, e.g. has moved or generated a collision.
    /// </summary>
    public bool IsDirty { set; get; }

    /// <summary>
    /// Destroys this physicsObject and removes it from the simulation.
    /// </summary>
    public void Destroy();
}