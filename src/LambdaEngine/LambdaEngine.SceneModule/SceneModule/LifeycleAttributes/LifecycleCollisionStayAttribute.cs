namespace LambdaEngine.SceneModule;

/// <summary>
/// CollisionStay is called for every frame during a collision.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LifecycleCollisionStayAttribute : LifecycleAttribute { }