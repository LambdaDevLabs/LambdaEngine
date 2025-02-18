namespace LambdaEngine.DebugSystem;

/// <summary>
/// API for the DebugSystem.
/// </summary>
public interface IDebugSystem {
    /// <summary>
    /// The active loglevel of this debugger.
    /// </summary>
    public LogLevel LogLevel { get; set; }
    
    /// <summary>
    /// Initialize the DebugSystem.
    /// </summary>
    public void Initialize();
    
    /// <summary>
    /// Starts this debugger.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stops this debugger.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Queue a log message.
    /// </summary>
    /// <param name="message"></param>
    public void Log(string message);
    
    /// <summary>
    /// Queue a log message.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="loglevel"></param>
    public void Log(string message, LogLevel loglevel);
}