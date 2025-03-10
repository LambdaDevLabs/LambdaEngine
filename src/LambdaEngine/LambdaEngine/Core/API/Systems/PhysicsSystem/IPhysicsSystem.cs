using System.Numerics;

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
    /// Returns all collisions for the collider with the specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ICollection<int> GetCollisionsFor(int id);

    /// <summary>
    /// Creates a new boxCollider.
    /// </summary>
    /// <returns>The id of the new collider.</returns>
    public int CreateBoxCollider();

    /// <summary>
    /// Creates a new circleCollider.
    /// </summary>
    /// <returns>The id of the new collider.</returns>
    public int CreateCircleCollider();

    /// <summary>
    /// Gets the position of the specified collider.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Vector2 GetColliderPosition(int id);

    /// <summary>
    /// Sets the position of the specified collider.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void SetColliderPosition(int id, Vector2 position);

    /// <summary>
    /// This method is only valid for BoxColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public float GetColliderWidth(int id);

    /// <summary>
    /// This method is only valid for BoxColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="width"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetColliderWidth(int id, float width);

    /// <summary>
    /// This method is only valid for BoxColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public float GetColliderHeight(int id);

    /// <summary>
    /// This method is only valid for BoxColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="height"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetColliderHeight(int id, float height);

    /// <summary>
    /// This method is only valid for CircleColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public float GetColliderRadius(int id);

    /// <summary>
    /// This method is only valid for CircleColliders.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="radius"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetColliderRadius(int id, float radius);

    /// <summary>
    /// Destroys the collider with the specified id.
    /// </summary>
    /// <param name="id"></param>
    public void DestroyCollider(int id);
}