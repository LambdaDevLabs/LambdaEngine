namespace LambdaEngine.TimeSystem;

public interface ITimeSystem {
    public delegate void LifecycleEvent();

    public event LifecycleEvent? OnStart;
    public event LifecycleEvent? OnPhysicsUpdate;
    public event LifecycleEvent? OnUpdate;
    public event LifecycleEvent? OnStop;
    
    public bool Stop { get; set; }

    public void Run();
}