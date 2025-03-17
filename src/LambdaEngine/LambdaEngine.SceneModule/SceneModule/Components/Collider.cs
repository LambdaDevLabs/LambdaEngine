using System.Numerics;
using LambdaEngine.DebugSystem;

namespace LambdaEngine.SceneModule;

/// <summary>
/// Base class for all collider components.
/// </summary>
public abstract class Collider : Component, ITransformListener {
    private static readonly Dictionary<int, Collider> colliders = new(32);

    private List<int> oldCollisions;
    
    internal List<Collision> enteredCollisions;
    internal List<Collision> stayedCollisions;
    internal List<Collision> exitedCollisions;
    
    protected int colliderId;

    /// <summary>
    /// The position of the collider.
    /// </summary>
    public Vector2 Position {
        get => Physics.GetColliderPosition(colliderId);
        set => Physics.SetColliderPosition(colliderId, value);
    }
    
    internal override void Initialize() {
        colliders.Add(colliderId, this);

        oldCollisions = [];

        enteredCollisions = new List<Collision>(8);
        stayedCollisions = new List<Collision>(8);
        exitedCollisions = new List<Collision>(8);
        
        transform.RegisterTransformListener(this);
    }

    internal void ProcessCollisions() {
        ICollection<int> collisions = Physics.GetCollisionsFor(colliderId);
        
        enteredCollisions.Clear();
        stayedCollisions.Clear();
        exitedCollisions.Clear();
        
        foreach (int collision in collisions) {
            if (oldCollisions.Contains(collision)) {
                stayedCollisions.Add(new Collision(colliders[collision]));
                oldCollisions.Remove(collision);
            }
            else {
                enteredCollisions.Add(new Collision(colliders[collision]));
            }
        }

        foreach (int oldCollision in oldCollisions) {
            exitedCollisions.Add(colliders.TryGetValue(oldCollision, out Collider collider)
                ? new Collision(collider)
                : new Collision(null!));
        }
        
        oldCollisions.Clear();
        oldCollisions.EnsureCapacity(collisions.Count);
        oldCollisions.AddRange(collisions);
    }

    void ITransformListener.TransformUpdate(Transform transform) {
        Physics.SetColliderPosition(colliderId, transform.Position);
    }

    internal override void DestroyComponent() {
        transform.UnregisterTransformListener(this);
        
        colliders.Remove(colliderId);
        
        Physics.DestroyCollider(colliderId);
        
        base.DestroyComponent();
    }
}