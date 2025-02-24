namespace LambdaEngine.SceneModule;

[Flags]
internal enum LifecycleEvents {
    NONE = 0,
    AWAKE = 1 << 0,
    START = 1 << 1,
    UPDATE = 1 << 2,
    DESTROY = 1 << 3,
}