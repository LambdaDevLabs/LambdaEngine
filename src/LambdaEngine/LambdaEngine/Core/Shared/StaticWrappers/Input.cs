using LambdaEngine.PlatformSystem.InputSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the InputSystem.
/// </summary>
public static class Input {
    private static IInputSystem inputSystem;

    /// <summary>
    /// Initializes the InputSystem wrapper.
    /// </summary>
    /// <param name="inputSystem"></param>
    public static void Connect(IInputSystem inputSystem) {
        Input.inputSystem = inputSystem;
    }

    /// <summary>
    /// Returns true if the specified key was pressed down.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyDown(Keys key) {
        return inputSystem.GetKeyDown(key);
    }

    /// <summary>
    /// Returns true if the specified key is pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKey(Keys key) {
        return inputSystem.GetKey(key);
    }

    /// <summary>
    /// Returns true if the specified key was released.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyUp(Keys key) {
        return inputSystem.GetKeyUp(key);
    }
}