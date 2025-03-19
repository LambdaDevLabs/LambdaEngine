namespace LambdaEngine.SceneModule;

/// <summary>
/// Destroy is called when the component or gameObject is destroyed.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LifecycleDestroyAttribute : LifecycleAttribute { }