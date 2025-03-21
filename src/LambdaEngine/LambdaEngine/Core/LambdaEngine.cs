﻿using LambdaEngine.DebugSystem;
using LambdaEngine.PhysicsSystem;
using LambdaEngine.PlatformSystem;
using LambdaEngine.SceneModule;
using LambdaEngine.TimeSystem;

namespace LambdaEngine;

#nullable disable

public class LambdaEngine {
    public readonly IDebugSystem debugSystem;
    public readonly IPlatformSystem platformSystem;
    public readonly ITimeSystem timeSystem;
    public readonly IPhysicsSystem physicsSystem;
    
    public readonly ISceneModule sceneModule;
    
    public IScene Scene { get; private set; }

    public LambdaEngine(IDebugSystem debugSystem, IPlatformSystem platformSystem, ITimeSystem timeSystem, IPhysicsSystem physicsSystem, ISceneModule sceneModule) {
        this.debugSystem = debugSystem;
        this.platformSystem = platformSystem;
        this.timeSystem = timeSystem;
        this.physicsSystem = physicsSystem;
        this.sceneModule = sceneModule;
    }
    
    public void Initialize(bool startLiveDebugger, IScene startScene) {
        Physics.Connect(physicsSystem);
        Time.Connect(timeSystem);
        Platform.Connect(platformSystem);
        Input.Connect(platformSystem.InputSystem);
        Renderer.Connect(platformSystem.RenderSystem);
        Audio.Connect(platformSystem.AudioSystem);
        Debug.Connect(debugSystem);
        
        Scenes.Connect(sceneModule);
        
        // Initialize and start the Debug system first, to allow its usage as soon as possible.
        Debug.Connect(debugSystem);
        Debug.Initialize();
        Debug.Start();
        
        platformSystem.Initialize();
        platformSystem.CreateWindow();
        
        platformSystem.RenderSystem.Initialize(platformSystem);
        platformSystem.InputSystem.Initialize();
        
        sceneModule.Initialize();
        
        physicsSystem.Initialize();
        
        Scene = startScene;
        
        Time.SubscribeToProcessInput(platformSystem.InputSystem.ProcessInput);
        Time.SubscribeToRender(platformSystem.RenderSystem.Render);
    }

    public void Run() {
        if (Scene is null) {
            throw new InvalidOperationException("Unable to run: missing start scene. Was the engine initialized?");
        }
        
        Scene.Initialize();
        
        timeSystem.StartGameLoop();
    }

    public void Stop() {
        timeSystem.StopGameLoop();
    }
}