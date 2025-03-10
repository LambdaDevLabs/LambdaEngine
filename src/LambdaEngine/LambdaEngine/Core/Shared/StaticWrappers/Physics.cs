using System.Numerics;
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

    public static void SimulatePhysics() {
        physicsSystem.SimulatePhysics();
    }

    /// <summary>
    /// Retuns all collisions of the collider with the specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ICollection<int> GetCollisionsFor(int id) {
        return physicsSystem.GetCollisionsFor(id);
    }

    public static int CreateBoxCollider() {
        return physicsSystem.CreateBoxCollider();
    }
    
    public static  int CreateCircleCollider() {
        return physicsSystem.CreateCircleCollider();
    }
    
    public static Vector2 GetColliderPosition(int id) {
        return physicsSystem.GetColliderPosition(id);
    }
    
    public static void SetColliderPosition(int id, Vector2 position) {
        physicsSystem.SetColliderPosition(id, position);
    }
    
    public static float GetColliderWidth(int id) {
        return physicsSystem.GetColliderWidth(id);
    }
    
    public static void SetColliderWidth(int id, float width) {
        physicsSystem.SetColliderWidth(id, width);
    }
    
    public static float GetColliderHeight(int id) {
        return physicsSystem.GetColliderHeight(id);
    }
    
    public static void SetColliderHeight(int id, float height) {
        physicsSystem.SetColliderHeight(id, height);
    }
    
    public static float GetColliderRadius(int id) {
        return physicsSystem.GetColliderRadius(id);
    }
    
    public static void SetColliderRadius(int id, float radius) {
        physicsSystem.SetColliderRadius(id, radius);
    }

    public static void DestroyCollider(int id) {
        physicsSystem.DestroyCollider(id);
    }
}