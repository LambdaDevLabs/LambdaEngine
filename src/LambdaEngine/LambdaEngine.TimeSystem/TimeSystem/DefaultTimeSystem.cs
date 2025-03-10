namespace LambdaEngine.TimeSystem;

public class DefaultTimeSystem : ITimeSystem {
    #region LifecycleEvents
    public event ITimeSystem.LifecycleEvent? OnStart;
    public event ITimeSystem.LifecycleEvent? OnFixedUpdate;
    public event ITimeSystem.LifecycleEvent? OnPhysicsUpdate;
    public event ITimeSystem.LifecycleEvent? OnProcessInput;
    public event ITimeSystem.LifecycleEvent? OnUpdate;
    public event ITimeSystem.LifecycleEvent? OnRender;
    public event ITimeSystem.LifecycleEvent? OnDestroy;
    public event ITimeSystem.LifecycleEvent? OnStop;
    #endregion

    public bool Running { get; private set; }

    #region Fixed Time
    public float FixedDeltaTime {
        get => (float)fixedDeltaTime;
        set => fixedDeltaTime = value;
    }
    
    public double FixedDeltaTimeAsDouble {
        get => fixedDeltaTime;
        set => fixedDeltaTime = value;
    }

    public float FixedRuntime {
        get => (float)fixedTime;
    }

    public double FixedRuntimeAsDouble {
        get => fixedTime;
    }
    #endregion

    #region Time
    public float DeltaTime {
        get => (float)(deltaTime > maximumDeltaTime ? maximumDeltaTime : deltaTime);
    }

    public double DeltaTimeAsDouble {
        get => deltaTime > maximumDeltaTime ? maximumDeltaTime : deltaTime;
    }

    public float Runtime {
        get => (float)time;
    }

    public double RuntimeAsDouble {
        get => time;
    }

    public float MaximumDeltaTime {
        get => (float)maximumDeltaTime;
        set => maximumDeltaTime = value;
    }
    
    public double MaximumDeltaTimeAsDouble {
        get => maximumDeltaTime;
        set => maximumDeltaTime = value;
    }
    #endregion

    private DateTime start;
    
    private double previousTime;
    private double currentTime;
    
    private double fixedTimeAcc;
    private double fixedTime;
    private double fixedDeltaTime = 0.016666666666666666d;
    
    private double deltaTime;
    private double time;
    private double maximumDeltaTime = 0.3333333333333333d;

    public void StartGameLoop() {
        if (Running) {
            throw new InvalidOperationException("Run() has already been called.");
        }
        
        start = DateTime.Now;

        fixedTimeAcc = 0;
        time = 0;
        fixedTime = 0;
        previousTime = GetCurrentTime();

        Running = true;
        
        ExecuteGameLoop();
    }
    
    public void StopGameLoop() {
        if (!Running) {
            return;
        }
        
        Running = false;
    }

    protected void ExecuteGameLoop() {
        while (Running) {
            currentTime = GetCurrentTime();
            deltaTime = currentTime - previousTime;

            previousTime = currentTime;
            
            time += deltaTime;
            fixedTimeAcc += deltaTime;
            
            OnStart?.Invoke();

            while (fixedTimeAcc > fixedDeltaTime) {
                fixedTimeAcc -= fixedDeltaTime;
                fixedTime += fixedDeltaTime;
                
                OnFixedUpdate?.Invoke();
                OnPhysicsUpdate?.Invoke();
            }
            
            OnProcessInput?.Invoke();
            
            OnUpdate?.Invoke();
            
            OnRender?.Invoke();
            
            OnStop?.Invoke();
            
            OnDestroy?.Invoke();
        }
    }

    private double GetCurrentTime() {
        return (DateTime.Now - start).TotalSeconds;
    }
}