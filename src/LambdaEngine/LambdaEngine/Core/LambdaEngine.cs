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
    
    public IScene Scene { get; private set; }

    public LambdaEngine(IDebugSystem debugSystem, IAssetManagementSystem assetManagementSystem, IPlatformSystem platformSystem, ITimeSystem timeSystem, IPhysicsSystem physicsSystem) {
        this.debugSystem = debugSystem;
        this.assetManagementSystem = assetManagementSystem;
        this.platformSystem = platformSystem;
        this.timeSystem = timeSystem;
        this.physicsSystem = physicsSystem;
    }
    
    public void Initialize(bool startLiveDebugger, IScene startScene) {
        AssetManagement.Connect(assetManagementSystem);
        Physics.Connect(physicsSystem);
        Time.Connect(timeSystem);
        Platform.Connect(platformSystem);
        Input.Connect(platformSystem.InputSystem);
        Renderer.Connect(platformSystem.RenderSystem);
        Audio.Connect(platformSystem.AudioSystem);
        Debug.Connect(debugSystem);
        
        // Initialize and start the Debug system first, to allow its usage as soon as possible.
        Debug.Connect(debugSystem);
        Debug.Initialize();
        Debug.Start();
        
        platformSystem.Initialize();
        platformSystem.CreateWindow();
        
        platformSystem.RenderSystem.Initialize(platformSystem);
        
        Scene = startScene;
    }

    public void Run() {
        if (Scene is null) {
            throw new InvalidOperationException("Unable to run: missing start scene. Was the engine initialized?");
        }
        
        timeSystem.StartGameLoop();
    }

    public void Stop() {
        timeSystem.StopGameLoop();
    }
}