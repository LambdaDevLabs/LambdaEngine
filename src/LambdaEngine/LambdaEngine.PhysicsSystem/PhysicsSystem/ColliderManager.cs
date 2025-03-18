using System.Numerics;
using LambdaEngine.DebugSystem;

namespace LambdaEngine.PhysicsSystem;

#nullable disable

/// <summary>
/// Manages all colliders in the physics engine.
/// </summary>
public static class ColliderManager {
    private static Dictionary<int, int> colliderIdToPosition;
    private static Dictionary<int, int> colliderPositionToId;
    private static ColliderObject[] colliders;

    private static bool autoIncrement;

    private static Memory<ColliderObject> memory;

    private static IdProvider idProvider;
    
    private static int next;
    
    /// <summary>
    /// Whether the <see cref="ColliderManager"/> is initialized.
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// The Capacity of the <see cref="ColliderManager"/>.
    /// </summary>
    public static int Capacity {
        get => colliders.Length;
    }

    /// <summary>
    /// The Capacity that is left in the <see cref="ColliderManager"/>.
    /// </summary>
    public static int CapacityLeft {
        get => colliders.Length - next;
    }

    /// <summary>
    /// Initializes the <see cref="ColliderManager"/> with the specified <paramref name="bufferSize"/>.
    /// </summary>
    /// <param name="bufferSize">The initial capacity of the <see cref="ColliderManager"/>.</param>
    /// <exception cref="Exception">Throws an exception if the <see cref="ColliderManager"/> was already initialized.</exception>
    public static void Initialize(int bufferSize, bool autoIncrement) {
        if (IsInitialized) {
            throw new Exception("Cannot initialize; already initialized.");
        }
        
        colliders = new ColliderObject[bufferSize];
        colliderIdToPosition = new Dictionary<int, int>(bufferSize);
        colliderPositionToId = new Dictionary<int, int>(bufferSize);
        idProvider = new IdProvider(64 > bufferSize ? bufferSize : 64);

        ColliderManager.autoIncrement = autoIncrement;

        memory = colliders;

        IsInitialized = true;
    }

    /// <summary>
    /// Creates a new <see cref="BoxColliderObject"/>.
    /// </summary>
    /// <returns>The wrapper object of the new <see cref="BoxColliderObject"/>.</returns>
    /// <exception cref="Exception">Throws an exception if no capacity is left on the <see cref="ColliderManager"/>.</exception>
    public static int CreateNewBoxCollider() {
        if (next >= colliders.Length) {
            if (autoIncrement) {
                IncrementCapacity();
            }
            else {
                throw new Exception("Not enough capacity to add new collider.");
            }
        }

        int id = idProvider.NextId();
        
        colliders[next] = new ColliderObject(id, new BoxColliderObject());
        colliderIdToPosition.Add(id, next);
        colliderPositionToId.Add(next++, id);

        return id;
    }
    
    /// <summary>
    /// Creates a new <see cref="CircleColliderObject"/>.
    /// </summary>
    /// <returns>The wrapper object of the new <see cref="CircleColliderObject"/>.</returns>
    /// <exception cref="Exception">Throws an exception if no capacity is left on the <see cref="ColliderManager"/>.</exception>
    public static int CreateNewCircleCollider() {
        if (next >= colliders.Length) {
            if (autoIncrement) {
                IncrementCapacity();
            }
            else {
                throw new Exception("Not enough capacity to add new collider.");
            }
        }

        int id = idProvider.NextId();
        
        colliders[next] = new ColliderObject(id, new CircleColliderObject());
        colliderIdToPosition.Add(id, next);
        colliderPositionToId.Add(next++, id);

        return id;
    }

    /// <summary>
    /// Destroys the <see cref="ColliderObject"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="ColliderObject"/> to destroy.</param>
    /// <exception cref="KeyNotFoundException">Throws an exception if no <see cref="ColliderObject"/> with the specified <paramref name="id"/> exists.</exception>
    public static void DestroyCollider(int id) {
        if (!colliderIdToPosition.TryGetValue(id, out int cPos)) {
            throw new KeyNotFoundException($"Unable to remove collider with id '{id}'; the id is not used.");
        }
        
        int last = next - 1;

        if (cPos == last) {
            colliders[last] = default;
            colliderIdToPosition.Remove(id);
            colliderPositionToId.Remove(cPos);
            idProvider.FreeId(id);
            next--;
        }
        else if (next == 1) {
            next = 0;
            colliderIdToPosition.Remove(id);
            colliderPositionToId.Remove(0);
            idProvider.FreeId(id);
        }
        else {
            int moveId = colliderPositionToId[last];
            
            // Override the removed collider with the last collider to close the gap.
            colliders[cPos] = colliders[last];
            // Remove the unused last collider for clarity, debugging and possible threading.
            colliders[last] = default;
            // Find the id of the (formerly) last collider.
            // Update the position of the (formerly) last collider.
            colliderIdToPosition[moveId] = cPos;
            // Update the id of the collider at the new position.
            colliderPositionToId[cPos] = moveId;
            // Remove of id entry of the removed collider.
            colliderIdToPosition.Remove(id);
            // Remove the entry for the last collider from the colliderValues dict.
            colliderPositionToId.Remove(last);
            
            idProvider.FreeId(id);
                
            next--;
        }
    }

    /// <summary>
    /// <para>Ensures that the <see cref="ColliderManager"/> has at least the specified <paramref name="capacity"/>.</para>
    /// <para>Be careful when calling this method, as it may introduce significant CPU work.</para>
    /// <para>Using this method may invalidate all references previously
    /// obtained by <see cref="ColliderManager"/>.<see cref="Get"/>.</para>
    /// </summary>
    /// <param name="capacity">The new minimal capacity of the <see cref="ColliderManager"/>.</param>
    public static unsafe void EnsureCapacity(int capacity) {
        if (colliders.Length >= capacity) {
            return;
        }

        ColliderObject[] newArr = new ColliderObject[capacity];

        fixed (ColliderObject* src = colliders) {
            fixed (ColliderObject* dest = newArr) {
                Buffer.MemoryCopy(src, dest,
                    capacity * sizeof(ColliderObject), next * sizeof(ColliderObject));
            }
        }

        colliders = newArr;
        memory = colliders;

        colliderPositionToId.EnsureCapacity(capacity);
        colliderIdToPosition.EnsureCapacity(capacity);
    }

    /// <summary>
    /// Increments the capacity by doubling it.
    /// </summary>
    private static void IncrementCapacity() {
        EnsureCapacity(Capacity * 2);
    }

    /// <summary>
    /// Returns a <see cref="Span{T}"/> over all colliders in the <see cref="ColliderManager"/>.
    /// </summary>
    /// <returns></returns>
    public static Span<ColliderObject> AsSpan() {
         return memory[..next].Span;
    }
    
    /// <summary>
    /// Returns a reference to the collider with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="ColliderObject"/> that is returned as a reference.</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">Throws an exception if not <see cref="ColliderObject"/> with the specified <paramref name="id"/> exists.</exception>
    public static ref ColliderObject Get(int id) {
        if (id < 0 || !colliderIdToPosition.TryGetValue(id, out int pos)) {
            throw new KeyNotFoundException($"The collider with the id {id} does not exist.");
        }
        
        return ref colliders[pos];
    }

    /// <summary>
    /// Destroys all colliders, gives all allocated memory away to the GC and sets <see cref="IsInitialized"/> to false.
    /// </summary>
    public static void Cleanup() {
        colliderIdToPosition.Clear();
        colliderPositionToId.Clear();

        Span<ColliderObject> span = AsSpan();
        for (int i = 0; i < span.Length; i++) {
            span[i] = default;
        }
        
        memory = null;
        idProvider = null;
        colliderPositionToId = null;
        colliderIdToPosition = null;
        colliders = null;
        next = 0;

        IsInitialized = false;
    }
}