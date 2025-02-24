namespace LambdaEngine.SceneModule;

public class BehaviourComponent : Component {
    public bool Awake { get; internal set; }
    public bool Started { get; internal set; }

    public void Destroy() {
        gameObject.scene.Destroy(this);
    }

    public void Destroy(GameObject gameObject) {
        gameObject.scene.Destroy(gameObject);
    }

    public void Destroy(Component component) {
        gameObject.scene.Destroy(component);
    }
}