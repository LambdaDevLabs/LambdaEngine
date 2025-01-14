namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// The physicsSystem is responsible for the physicsSimulation, e.g. CollisionDetection, -Resolution, etc.
/// </summary>
public interface IPhysicsSystem {
    /// <summary>
    /// Creates a new physicsObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IPhysicsObject CreatePhysicsObject<T>() where T : IPhysicsObject;
    
    /// <summary>
    /// Creates a new circleCollider.
    /// </summary>
    /// <returns></returns>
    public ICircleCollider CreateCircleCollider();
    
    /// <summary>
    /// Creates a new boxCollider.
    /// </summary>
    /// <returns></returns>
    public IBoxCollider CreateBoxCollider();
    
    /// <summary>
    /// Creates a new rigidbody.
    /// </summary>
    /// <returns></returns>
    public IRigidbody CreateRigidbody();
}