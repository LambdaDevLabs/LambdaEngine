namespace LambdaEngine.SceneModule;

/// <summary>
/// FixedUpdate is called once per physics frame.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LifecycleFixedUpdateAttribute : LifecycleAttribute { }