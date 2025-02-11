using LambdaEngine.TimeSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
///     Static wrapper class for easy access to the TimeSystem.
/// </summary>
public static class Time {
    private static ITimeSystem timeSystem;

    /// <summary>
    ///     Signals the main loop to stop after the completion of the current frame.
    /// </summary>
    public static bool Running {
        get => timeSystem.Running;
    }

    /// <summary>
    ///     Initializes the TimeSystem wrapper.
    /// </summary>
    /// <param name="timeSystem"></param>
    public static void Connect(ITimeSystem timeSystem) {
        Time.timeSystem = timeSystem;
    }

    #region Lifecycle Events
    public static void SubscribeToStart(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnStart += callback;
    }
    
    public static void SubscribeToFixedUpdate(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnFixedUpdate += callback;
    }
    
    public static void SubscribeToPhysicsUpdate(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnPhysicsUpdate += callback;
    }
    
    public static void SubscribeToProcessInput(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnProcessInput += callback;
    }
    
    public static void SubscribeToUpdate(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnUpdate += callback;
    }
    
    public static void SubscribeToRender(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnRender += callback;
    }
    
    public static void SubscribeToStop(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnStop += callback;
    }
    
    public static void SubscribeToDestroy(ITimeSystem.LifecycleEvent callback) {
        timeSystem.OnDestroy += callback;
    }
    #endregion
    
    #region Fixed Time
    public static float FixedDeltaTime {
        get => timeSystem.FixedDeltaTime;
        set => timeSystem.FixedDeltaTime = value;
    }

    public static double FixedDeltaTimeAsDouble {
        get => timeSystem.FixedDeltaTimeAsDouble;
        set => timeSystem.FixedDeltaTimeAsDouble = value;
    }

    public static float FixedRuntime {
        get => timeSystem.FixedRuntime;
    }

    public static double FixedRuntimeAsDouble {
        get => timeSystem.FixedRuntimeAsDouble;
    }
    #endregion

    #region Time
    public static float DeltaTime {
        get => timeSystem.DeltaTime;
    }

    public static double DeltaTimeAsDouble {
        get => timeSystem.DeltaTimeAsDouble;
    }

    public static float Runtime {
        get => timeSystem.Runtime;
    }

    public static double RuntimeAsDouble {
        get => timeSystem.RuntimeAsDouble;
    }

    public static float MaximumDeltaTime {
        get => timeSystem.MaximumDeltaTime;
        set => timeSystem.MaximumDeltaTime = value;
    }

    public static double MaximumDeltaTimeAsDouble {
        get => timeSystem.MaximumDeltaTimeAsDouble;
        set => timeSystem.MaximumDeltaTimeAsDouble = value;
    }
    #endregion

    public static void StartGameLoop() {
        timeSystem.StartGameLoop();
    }

    public static void StopGameLoop() {
        timeSystem.StopGameLoop();
    }
}