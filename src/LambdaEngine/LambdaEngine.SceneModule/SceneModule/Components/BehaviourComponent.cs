namespace LambdaEngine.SceneModule;

/// <summary>
/// Use this class to create new components for gameObjects.
/// </summary>
public class BehaviourComponent : Component {
    /// <summary>
    /// If awake has been called.
    /// </summary>
    public bool Awake { get; internal set; }
    /// <summary>
    /// If Start has been called.
    /// </summary>
    public bool Started { get; internal set; }

    /// <summary>
    /// Mark this component for destruction.
    /// </summary>
    public void Destroy() {
        gameObject.scene.Destroy(this);
    }

    /// <summary>
    /// Mark the specified gameObject for destruction.
    /// </summary>
    /// <param name="gameObject"></param>
    public void Destroy(GameObject gameObject) {
        gameObject.scene.Destroy(gameObject);
    }

    /// <summary>
    /// Mark the specified component for destruction.
    /// </summary>
    /// <param name="component"></param>
    public void Destroy(Component component) {
        gameObject.scene.Destroy(component);
    }
}