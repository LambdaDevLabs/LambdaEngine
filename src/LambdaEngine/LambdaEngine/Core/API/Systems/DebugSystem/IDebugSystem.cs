namespace LambdaEngine.DebugSystem;

public interface IDebugSystem {
    public void Initialize();

    public void Log(string message);
    
    public void Log(string message, int loglevel);

    public void StartDebugConsole();

    public void StopDebugConsole();
}