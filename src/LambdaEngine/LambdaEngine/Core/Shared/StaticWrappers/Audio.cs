using LambdaEngine.AudioSystem;

namespace LambdaEngine;

#nullable disable

/// <summary>
/// Static wrapper class for easy access to the AudioSystem.
/// </summary>
public static class Audio {
    private static IAudioSystem audioSystem;

    /// <summary>
    /// Initializes the AudioSystem wrapper.
    /// </summary>
    /// <param name="audioSystem"></param>
    public static void Connect(IAudioSystem audioSystem) {
        Audio.audioSystem = audioSystem;
    }
}