namespace LambdaEngine.TimeSystem;

public interface ITimeSystem {
    public delegate void LifecycleEvent();

    #region Lifecycle Events
    public event LifecycleEvent? OnStart;
    public event LifecycleEvent? OnFixedUpdate;
    public event LifecycleEvent? OnPhysicsUpdate;
    public event LifecycleEvent? OnProcessInput;
    public event LifecycleEvent? OnUpdate;
    public event LifecycleEvent? OnRender;
    public event LifecycleEvent? OnDestroy;
    public event LifecycleEvent? OnStop;
    #endregion

    public bool Running { get; }

    #region Fixed Time
    public float FixedDeltaTime { get; set; }

    public double FixedDeltaTimeAsDouble { get; set; }

    public float FixedRuntime { get; }

    public double FixedRuntimeAsDouble { get; }
    #endregion

    #region Time
    public float DeltaTime { get; }

    public double DeltaTimeAsDouble { get; }

    public float Runtime { get; }

    public double RuntimeAsDouble { get; }

    public float MaximumDeltaTime { get; set; }
    
    public double MaximumDeltaTimeAsDouble { get; set; }
    #endregion

    public void StartGameLoop() { }
    
    public void StopGameLoop() { }
}