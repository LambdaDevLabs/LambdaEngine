using LambdaEngine.PhysicsSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the PhysicsSystem.
/// </summary>
public static class Physics {
    private static IPhysicsSystem physicsSystem;
    
    /// <summary>
    /// Initializes the PhysicsSystem wrapper.
    /// </summary>
    /// <param name="physicsSystem"></param>
    public static void Connect(IPhysicsSystem physicsSystem) {
        Physics.physicsSystem = physicsSystem;
    }

    /// <summary>
    /// Creates a new PhysicsObject and registers it in the PhysicsSystem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPhysicsObject CreatePhysicsObject<T>() where T : IPhysicsObject, new() {
        return physicsSystem.CreatePhysicsObject<T>();
    }

    /// <summary>
    /// Creates a new CircleCollider and registers it in the PhysicsSystem.
    /// </summary>
    /// <returns></returns>
    public static ICircleCollider CreateCircleCollider() {
        return physicsSystem.CreateCircleCollider();
    }

    /// <summary>
    /// Creates a new Rigidbody and registers it in the PhysicsSystem.
    /// </summary>
    /// <returns></returns>
    public static IRigidbody CreateRigidbody() {
        return physicsSystem.CreateRigidbody();
    }
}