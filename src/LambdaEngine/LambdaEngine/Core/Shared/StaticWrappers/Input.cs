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
}