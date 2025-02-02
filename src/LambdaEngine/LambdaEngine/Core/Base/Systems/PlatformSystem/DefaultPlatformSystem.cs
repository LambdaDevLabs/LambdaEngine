﻿using LambdaEngine.DebugSystem;
using LambdaEngine.PlatformSystem.AudioSystem;
using LambdaEngine.PlatformSystem.InputSystem;
using LambdaEngine.PlatformSystem.RenderSystem;
using SDL3;

namespace LambdaEngine.PlatformSystem;

/// <summary>
/// Initializes the platform/window system and exposes important handles.
/// </summary>
public class DefaultPlatformSystem : IPlatformSystem {
    private IntPtr windowHandle;
    private IntPtr rendererHandle;

    public IRenderSystem RenderSystem { get; }

    public IInputSystem InputSystem { get; }

    public IAudioSystem AudioSystem { get; }

    public int WindowWidth { get; set; }

    public int WindowHeight { get; set; }

    public IntPtr WindowHandle {
        get => windowHandle;
        set => windowHandle = value;
    }

    public IntPtr RendererHandle {
        get => rendererHandle;
        set => rendererHandle = value;
    }

    public DefaultPlatformSystem(IRenderSystem renderSystem, IInputSystem inputSystem, IAudioSystem audioSystem) {
        RenderSystem = renderSystem;
        InputSystem = inputSystem;
        AudioSystem = audioSystem;
    }
    
    public void SetWindowSize(int width, int height) {
        WindowWidth = width;
        WindowHeight = height;
    }
    
    public bool CreateWindow() {
        SDL.SDL_SetAppMetadata("My Game", "1.0", "com.example.my-game");

        if (!SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_VIDEO)) {
            Debug.Log($"Couldn't initialize SDL: {SDL.SDL_GetError()}", LogLevel.FATAL);
            return false;
        }
        
        Debug.Log("SDL3 initialized.", LogLevel.INFO);

        if (!SDL.SDL_CreateWindowAndRenderer("My Game", WindowWidth, WindowHeight, 0, out windowHandle, out rendererHandle)) {
            Debug.Log($"Couldn't create window/renderer: {SDL.SDL_GetError()}", LogLevel.FATAL);
            return false;
        }
        
        Debug.Log("Window created.", LogLevel.INFO);
        
        return true;
    }

    public void Initialize() {
        
    }

    public void PollEvents() {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event @event)) {
            if (@event.type == (uint)SDL.SDL_EventType.SDL_EVENT_QUIT) {
                Environment.Exit(0);
            }
        }
    }
}