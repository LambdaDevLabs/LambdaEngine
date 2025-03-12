using LambdaEngine.DebugSystem;

namespace LambdaEngine.RenderSystem;

#nullable disable

/// <summary>
/// Manages all sprites in the render system.
/// </summary>
internal static class SpriteManager {
    private static Dictionary<int, int> spriteIdToPosition;
    private static Dictionary<int, int> spritePositionToId;
    private static SpriteObject[] sprites;

    private static Memory<SpriteObject> memory;

    private static IdProvider idProvider;
    
    private static int next;
    
    /// <summary>
    /// Whether the <see cref="SpriteManager"/> is initialized.
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// The Capacity of the <see cref="SpriteManager"/>.
    /// </summary>
    public static int Capacity {
        get => sprites.Length;
    }

    /// <summary>
    /// The Capacity that is left in the <see cref="SpriteManager"/>.
    /// </summary>
    public static int CapacityLeft {
        get => sprites.Length - next;
    }

    /// <summary>
    /// Initializes the <see cref="SpriteManager"/> with the specified <paramref name="bufferSize"/>.
    /// </summary>
    /// <param name="bufferSize">The initial capacity of the <see cref="SpriteManager"/>.</param>
    /// <exception cref="Exception">Throws an exception if the <see cref="SpriteManager"/> was already initialized.</exception>
    public static void Initialize(int bufferSize) {
        if (IsInitialized) {
            throw new Exception("Cannot initialize; already initialized.");
        }
        
        sprites = new SpriteObject[bufferSize];
        spriteIdToPosition = new Dictionary<int, int>(bufferSize);
        spritePositionToId = new Dictionary<int, int>(bufferSize);
        idProvider = new IdProvider(64 > bufferSize ? bufferSize : 64);

        memory = sprites;

        IsInitialized = true;
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> based on the specified texture file at '<paramref name="path"/>'.
    /// </summary>
    /// <returns>The id of the new <see cref="Sprite"/>.</returns>
    /// <exception cref="Exception">Throws an exception if no capacity is left on the <see cref="SpriteManager"/>.</exception>
    public static unsafe int CreateSpriteFromFile(string path) {
        if (next >= sprites.Length) {
            IncrementCapacity();
        }
    
        int id = idProvider.NextId();

        int textureId = TexturePool.LoadTexture(path);
        sprites[next] = new SpriteObject(textureId);
        spriteIdToPosition.Add(id, next);
        spritePositionToId.Add(next++, id);
        
        return id;
    }
    
    /// <summary>
    /// Creates a new <see cref="Sprite"/> based on the specified texture.
    /// </summary>
    /// <returns>The id of the new <see cref="Sprite"/>.</returns>
    /// <exception cref="Exception">Throws an exception if no capacity is left on the <see cref="SpriteManager"/>.</exception>
    public static unsafe int CreateSpriteFromTexture(int textureId) {
        if (next >= sprites.Length) {
            IncrementCapacity();
        }
    
        int id = idProvider.NextId();
        
        sprites[next] = new SpriteObject(textureId);
        spriteIdToPosition.Add(id, next);
        spritePositionToId.Add(next++, id);
        
        return id;
    }
    
    /// <summary>
    /// Attempts to create a new <see cref="Sprite"/> based on the specified texture file at '<paramref name="path"/>'.
    /// </summary>
    /// <param name="path">The path of the file to load a texture from.</param>
    /// <param name="id">The id of the new <see cref="Sprite"/>.</param>
    /// <returns>True if the <see cref="Sprite"/> was successfully created, otherwise false.</returns>
    public static unsafe bool TryCreateSpriteFromFile(string path, out int id) {
        if (next >= sprites.Length) {
            IncrementCapacity();
        }
    
        id = idProvider.NextId();
        
        int textureId = TexturePool.LoadTexture(path);
        sprites[next] = new SpriteObject(textureId);
        spriteIdToPosition.Add(id, next);
        spritePositionToId.Add(next++, id);

        return true;
    }
    
    /// <summary>
    /// Attempts to create a new <see cref="Sprite"/> based on the specified texture.
    /// </summary>
    /// <param name="textureId">The id of the texture that is used to create the <see cref="Sprite"/></param>
    /// <param name="id">The id of the new <see cref="Sprite"/>.</param>
    /// <returns>True if the <see cref="Sprite"/> was successfully created, otherwise false.</returns>
    public static unsafe bool TryCreateSpriteFromTexture(int textureId, out int id) {
        if (next >= sprites.Length) {
            IncrementCapacity();
        }
    
        id = idProvider.NextId();
        
        sprites[next] = new SpriteObject(textureId);
        spriteIdToPosition.Add(id, next);
        spritePositionToId.Add(next++, id);

        return true;
    }

    /// <summary>
    /// Destroys the <see cref="Sprite"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="Sprite"/> to destroy.</param>
    /// <exception cref="KeyNotFoundException">Throws an exception if no <see cref="Sprite"/> with the specified <paramref name="id"/> exists.</exception>
    public static void DestroySprite(int id) {
        if (!spriteIdToPosition.TryGetValue(id, out int cPos)) {
            throw new Exception($"Unable to remove sprite with id '{id}'; the id is not used.");
        }

        if (next == 1) {
            sprites[0] = default;
            
            idProvider.FreeId(id);
            
            spriteIdToPosition.Remove(id);
            spritePositionToId.Remove(0);
            
            next = 0;
        }
        else if (spriteIdToPosition[id] == next - 1) {
            sprites[cPos] = default;
            
            idProvider.FreeId(id);
            
            spriteIdToPosition.Remove(id);
            spritePositionToId.Remove(cPos);

            next--;
        }
        else {
            int last = --next;
                
            // Override the removed sprite with the last sprite to close the gap.
            sprites[cPos] = sprites[last];
            // Remove the now duplicated last sprite.
            sprites[last] = default;
            
            // Free the id of the removed sprite.
            idProvider.FreeId(id);

            // Retrieve the id of the moved sprite.
            int movedId = spritePositionToId[last];
            
            // Update the id of the (formerly) last sprite with its new position
            spriteIdToPosition[movedId] = cPos;
            // Update the new position of the (formerly) last sprite with its id
            spritePositionToId[cPos] = movedId;
            // Remove the now unused entry for the last position
            spritePositionToId.Remove(last);
            // Remove the id entry of the removed sprite.
            spriteIdToPosition.Remove(id);
        }
    }
    
    /// <summary>
    /// Attempts to destroy the <see cref="Sprite"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="Sprite"/> to destroy.</param>
    /// <returns>True if the <see cref="Sprite"/> was successfully destroyed, otherwise false.</returns>
    public static bool TryDestroySprite(int id) {
        if (!spriteIdToPosition.TryGetValue(id, out int cPos)) {
            return false;
        }

        if (next == 1) {
            sprites[0] = default;
            
            idProvider.FreeId(id);
            
            spriteIdToPosition.Remove(id);
            spritePositionToId.Remove(0);
            
            next = 0;
        }
        else if (spriteIdToPosition[id] == next - 1) {
            sprites[cPos] = default;
            
            idProvider.FreeId(id);
            
            spriteIdToPosition.Remove(id);
            spritePositionToId.Remove(cPos);

            next--;
        }
        else {
            int last = --next;
                
            // Override the removed sprite with the last sprite to close the gap.
            sprites[cPos] = sprites[last];
            // Remove the now duplicated last sprite.
            sprites[last] = default;
            
            // Free the id of the removed sprite.
            idProvider.FreeId(id);

            // Retrieve the id of the moved sprite.
            int movedId = spritePositionToId[last];
            
            // Update the id of the (formerly) last sprite with its new position
            spriteIdToPosition[movedId] = cPos;
            // Update the new position of the (formerly) last sprite with its id
            spritePositionToId[cPos] = movedId;
            // Remove the now unused entry for the last position
            spritePositionToId.Remove(last);
            // Remove the id entry of the removed sprite.
            spriteIdToPosition.Remove(id);
        }

        return true;
    }

    /// <summary>
    /// <para>Ensures that the <see cref="SpriteManager"/> has at least the specified <paramref name="capacity"/>.</para>
    /// <para>Be careful when calling this method, as it may introduce significant CPU work.</para>
    /// <para>Using this method may invalidate all references previously
    /// obtained by <see cref="SpriteManager"/>.<see cref="Get"/>.</para>
    /// </summary>
    /// <param name="capacity">The new minimal capacity of the <see cref="SpriteManager"/>.</param>
    public static unsafe void EnsureCapacity(int capacity) {
        if (sprites.Length >= capacity) {
            return;
        }
        
        Debug.Log($"Resizing SpriteManager capacity from {Capacity} to {capacity}.", LogLevel.INFO);

        SpriteObject[] newArr = new SpriteObject[capacity];

        fixed (SpriteObject* src = sprites) {
            fixed (SpriteObject* dest = newArr) {
                Buffer.MemoryCopy(src, dest,
                    capacity * sizeof(SpriteObject), next * sizeof(SpriteObject));
            }
        }

        sprites = newArr;

        spritePositionToId.EnsureCapacity(capacity);
        spriteIdToPosition.EnsureCapacity(capacity);

        memory = sprites;
    }

    /// <summary>
    /// Returns a <see cref="Span{T}"/> over all sprites in the <see cref="SpriteManager"/>.
    /// </summary>
    /// <returns></returns>
    public static Span<SpriteObject> AsSpan() {
         return memory[..next].Span;
    }
    
    /// <summary>
    /// Returns a reference to the <see cref="Sprite"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="Sprite"/> that is returned as a reference.</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">Throws an exception if not <see cref="Sprite"/> with the specified <paramref name="id"/> exists.</exception>
    public static ref SpriteObject Get(int id) {
        if (id < 0 || !spriteIdToPosition.TryGetValue(id, out int value)) {
            throw new Exception($"The sprite with the id {id} does not exist.");
        }
        
        return ref sprites[value];
    }

    private static void IncrementCapacity() {
        EnsureCapacity(sprites.Length * 2);
    }

    /// <summary>
    /// Destroys all sprites, gives all allocated memory away to the GC and sets <see cref="IsInitialized"/> to false.
    /// </summary>
    public static void Cleanup() {
        memory = null;
        idProvider = null;
        spritePositionToId = null;
        spriteIdToPosition = null;
        sprites = null;
        next = 0;

        IsInitialized = false;
    }
}