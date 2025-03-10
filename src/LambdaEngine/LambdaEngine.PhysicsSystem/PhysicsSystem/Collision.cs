namespace LambdaEngine.PhysicsSystem;

internal readonly record struct Collision {
    /// <summary>
    /// The id of the first involved collider.
    /// </summary>
    public readonly int a;
    /// <summary>
    /// The id of the second involved collider.
    /// </summary>
    public readonly int b;

    public Collision(int a, int b) {
        this.a = a;
        this.b = b;
    }
}