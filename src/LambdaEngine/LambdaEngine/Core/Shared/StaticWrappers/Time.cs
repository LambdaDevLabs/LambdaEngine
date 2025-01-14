using LambdaEngine.TimeSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the TimeSystem.
/// </summary>
public static class Time {
    private static ITimeSystem timeSystem;

    /// <summary>
    /// Signals the main loop to stop after the completion of the current frame.
    /// </summary>
    public static bool Stop {
        get => timeSystem.Stop;
        set => timeSystem.Stop = value;
    }

    /// <summary>
    /// Initializes the TimeSystem wrapper.
    /// </summary>
    /// <param name="timeSystem"></param>
    public static void Initialize(ITimeSystem timeSystem) {
        Time.timeSystem = timeSystem;
    }

    /// <summary>
    /// Starts the main loop.
    /// </summary>
    public static void Run() {
        timeSystem.Run();
    }
}