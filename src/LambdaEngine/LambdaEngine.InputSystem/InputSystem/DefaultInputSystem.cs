using LambdaEngine.DebugSystem;
using SDL3;

namespace LambdaEngine.InputSystem;

public class DefaultInputSystem : IInputSystem { 
    private Dictionary<Keys, KeyState> keyStates;
    private List<Keys> triggeredKeys;
    
    public void Initialize() {
        Array keys = Enum.GetValues<Keys>();

        keyStates = new Dictionary<Keys, KeyState>(keys.Length);
        triggeredKeys = new List<Keys>(keys.Length);
        
        foreach (Keys key in keys) {
            keyStates[key] = KeyState.DEFAULT;
        }
        
        Debug.Log("DefaultInputSystem Initialized.", LogLevel.INFO);
    }

    public void ProcessInput() {
        foreach (Keys triggeredKey in triggeredKeys) {
            // The key was pressed down => change the state to pressed.
            if (keyStates[triggeredKey] == KeyState.DOWN) {
                keyStates[triggeredKey] = KeyState.PRESSED;
            }
            else { // Else, the key was released => reset the keys that were release in the last frame to the default state.
                keyStates[triggeredKey] = KeyState.DEFAULT;
            }
        }
        
        triggeredKeys.Clear();
        
        while (SDL.SDL_PollEvent(out SDL.SDL_Event @event)) {
            switch (@event.type) {
                case (uint)SDL.SDL_EventType.SDL_EVENT_QUIT:
                    Environment.Exit(0);
                    break;
                
                case (uint)SDL.SDL_EventType.SDL_EVENT_KEY_DOWN:
                case (uint)SDL.SDL_EventType.SDL_EVENT_KEY_UP:
                    SDL.SDL_KeyboardEvent kEvent = @event.key;

                    if (kEvent.repeat) {
                        continue;
                    }
                    
                    Keys key = SdlScancodeToKey(kEvent.scancode);
                    if (key != Keys.UNKNOWN) {
                        keyStates[key] = kEvent.down ? KeyState.DOWN : KeyState.UP;
                        
                        triggeredKeys.Add(key);
                    }
                    break;
            }
        }
    }

    public bool GetKeyDown(Keys key) {
        return keyStates[key] == KeyState.DOWN;
    }

    public bool GetKey(Keys key) {
        return keyStates[key] == KeyState.PRESSED;
    }

    public bool GetKeyUp(Keys key) {
        return keyStates[key] == KeyState.UP;
    }

    private static Keys SdlScancodeToKey(SDL.SDL_Scancode scancode) {
        Keys key = (Keys)scancode;

        return !Enum.IsDefined(key) ? Keys.UNKNOWN : key;
    }
}