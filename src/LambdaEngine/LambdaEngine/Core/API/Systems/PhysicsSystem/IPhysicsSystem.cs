namespace LambdaEngine.PhysicsSystem;

/// <summary>
/// The physicsSystem is responsible for the physicsSimulation, e.g. CollisionDetection, -Resolution, etc.
/// </summary>
public interface IPhysicsSystem {
    /// <summary>
    /// Initializes the physicsSystem.
    /// </summary>
    public void Initialize();

    /// <summary>
    /// Moves the physics simulation forward by one step.
    /// </summary>
    public void SimulatePhysics();
    
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