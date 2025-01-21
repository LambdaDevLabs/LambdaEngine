using LambdaEngine.AssetManagementSystem;
using LambdaEngine.DebugSystem;
using LambdaEngine.PhysicsSystem;
using LambdaEngine.PlatformSystem;
using LambdaEngine.SceneModule;
using LambdaEngine.TimeSystem;

namespace LambdaEngine;

#nullable disable

public class LambdaEngine {
    public readonly IDebugSystem debugSystem;
    public readonly IAssetManagementSystem assetManagementSystem;
    public readonly IPlatformSystem platformSystem;
    public readonly ITimeSystem timeSystem;
    public readonly IPhysicsSystem physicsSystem;
    
    private IScene StartScene { get; set; }

    public LambdaEngine(IDebugSystem debugSystem, IAssetManagementSystem assetManagementSystem, IPlatformSystem platformSystem, ITimeSystem timeSystem, IPhysicsSystem physicsSystem) {
        this.debugSystem = debugSystem;
        this.assetManagementSystem = assetManagementSystem;
        this.platformSystem = platformSystem;
        this.timeSystem = timeSystem;
        this.physicsSystem = physicsSystem;
    }

    /// <summary>
    /// Initializes the engine by initializing all systems and setting the start scene.
    /// </summary>
    /// <param name="startScene"></param>
    public void Initialize(IScene startScene) {
        // Initialize and start the Debug system first, to allow its usage as soon as possible.
        Debug.Initialize(debugSystem);
        Debug.Start();
        
        AssetManagement.Initialize(assetManagementSystem);
        
        Physics.Initialize(physicsSystem);
        Time.Initialize(timeSystem);
        Platform.Initialize(platformSystem);
        Input.Initialize(platformSystem.InputSystem);
        Render.Initialize(platformSystem.RenderSystem);
        Audio.Initialize(platformSystem.AudioSystem);
        
        StartScene = startScene;

        timeSystem.OnUpdate += startScene.OnUpdate;
    }

    /// <summary>
    /// Loads the start scene.
    /// </summary>
    public void Load() {
        if (StartScene is null) {
            throw new InvalidOperationException("Unable to run: missing start scene. Was the engine initialized?");
        }
    }

    /// <summary>
    /// Starts the gameloop.
    /// </summary>
    public void Run() {
        
    }

    /// <summary>
    /// Loads the start scene and starts the gameloop.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void LoadAndRun() {
        Load();
        Run();
    }

    /// <summary>
    /// Stops the gamelood and unloads the current scene.
    /// </summary>
    public void Stop() {
        timeSystem.Stop = true;
    }
}