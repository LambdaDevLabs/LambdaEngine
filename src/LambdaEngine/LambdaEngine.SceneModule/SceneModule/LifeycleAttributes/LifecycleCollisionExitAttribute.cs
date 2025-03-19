namespace LambdaEngine.SceneModule;

/// <summary>
/// CollisionExit is called for the first frame after a collision.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LifecycleCollisionExitAttribute : LifecycleAttribute { }