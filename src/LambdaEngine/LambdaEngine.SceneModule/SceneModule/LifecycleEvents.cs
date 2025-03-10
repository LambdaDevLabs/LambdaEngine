namespace LambdaEngine.SceneModule;

[Flags]
internal enum LifecycleEvents {
    NONE = 0,
    AWAKE = 1 << 0,
    START = 1 << 1,
    FIXED_UPDATE = 1 << 2,
    UPDATE = 1 << 3,
    DESTROY = 1 << 4,
    
    COLLISION_ENTER = 1 << 5,
    COLLISION_STAY = 1 << 6,
    COLLISION_EXIT = 1 << 7,
}