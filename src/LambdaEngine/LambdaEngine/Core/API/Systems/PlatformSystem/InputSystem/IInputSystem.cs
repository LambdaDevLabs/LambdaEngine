namespace LambdaEngine.PlatformSystem.InputSystem;

public interface IInputSystem {
    /// <summary>
    /// Initializes the InputSystem.
    /// </summary>
    public void Initialize();

    /// <summary>
    /// Processes all accumulated input events since the last processing call.
    /// </summary>
    public void ProcessInput();

    /// <summary>
    /// Returns true if the specified key was pressed down.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyDown(Keys key);

    /// <summary>
    /// Returns true if the specified key is pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKey(Keys key);

    /// <summary>
    /// Returns true if the specified key was released.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyUp(Keys key);
}