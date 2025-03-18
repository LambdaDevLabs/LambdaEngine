namespace LambdaEngine.SceneModule;

public class Collision {
    /// <summary>
    /// The gameObject of the collider that was hit.
    /// </summary>
    public GameObject? gameObject;
    /// <summary>
    /// The collider that was hit.
    /// </summary>
    public Collider? collider;

    public Collision(Collider? collider) {
        this.collider = collider;

        gameObject = collider?.gameObject;
    }
}