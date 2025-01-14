using LambdaEngine.PhysicsSystem;
using LambdaEngine.PlatformSystem;
using LambdaEngine.SceneModule;
using LambdaEngine.TimeSystem;

namespace LambdaEngine;

#nullable disable

public class LambdaEngine {
    public readonly IPlatformSystem platformSystem = null;
    public readonly ITimeSystem timeSystem;
    public readonly IPhysicsSystem physicsSystem;
    
    public IScene Scene { get; private set; }

    public LambdaEngine(ITimeSystem timeSystem, IPhysicsSystem physicsSystem) {
        this.timeSystem = timeSystem;
        this.physicsSystem = physicsSystem;
    }

    public void Initialize(IScene startScene) {
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