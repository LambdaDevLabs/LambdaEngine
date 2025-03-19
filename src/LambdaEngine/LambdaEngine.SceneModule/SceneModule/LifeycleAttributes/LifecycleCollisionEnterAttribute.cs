namespace LambdaEngine.SceneModule;

/// <summary>
/// CollisionEnter is called for the first frame of a collision.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LifecycleCollisionEnterAttribute : LifecycleAttribute { }