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

    public void Initialize(IScene startScene) {
        Debug.Initialize(debugSystem);
        AssetManagement.Initialize(assetManagementSystem);
        Physics.Initialize(physicsSystem);
        Time.Initialize(timeSystem);
        Platform.Initialize(platformSystem);
        Input.Initialize(platformSystem.InputSystem);
        Render.Initialize(platformSystem.RenderSystem);
        Audio.Initialize(platformSystem.AudioSystem);
        
        Scene = startScene;

        timeSystem.OnUpdate += startScene.OnUpdate;
    }

    public void Run() {
        if (Scene is null) {
            throw new InvalidOperationException("Unable to run: missing start scene. Was the engine initialized?");
        }
        
        timeSystem.Run();
    }

    public void Stop() {
        timeSystem.Stop = true;
    }
}