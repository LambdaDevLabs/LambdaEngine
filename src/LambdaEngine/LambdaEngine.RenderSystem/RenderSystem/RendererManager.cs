using LambdaEngine.DebugSystem;

namespace LambdaEngine.RenderSystem;

internal static unsafe class RendererManager {
    private static readonly int RENDERER_SIZE = sizeof(Renderer);
    
    private static Dictionary<int, int> rendererIdToPosition;
    private static Dictionary<int, int> rendererPositionToId;
    private static Renderer[] renderers;

    private static Memory<Renderer> memory;

    private static IdProvider idProvider;
    
    private static int next;
    
    public static bool IsInitialized { get; private set; }

    public static int Capacity {
        get => renderers.Length;
    }

    public static int CapacityLeft {
        get => renderers.Length - next;
    }

    public static void Initialize(int bufferSize) {
        if (IsInitialized) {
            throw new Exception("Cannot initialize; already initialized.");
        }
        
        renderers = new Renderer[bufferSize];
        rendererPositionToId = new Dictionary<int, int>(bufferSize);
        rendererIdToPosition = new Dictionary<int, int>(bufferSize);

        memory = renderers;

        idProvider = new IdProvider(64, 8);

        IsInitialized = true;
    }

    /// <summary>
    /// Creates a new renderer and returns its id.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static int CreateRenderer(RendererType type) {
        if (CapacityLeft == 0) {
            IncrementCapacity();
        }
        
        int newId = idProvider.NextId();

        renderers[next] = new Renderer(type);
        rendererIdToPosition.Add(newId, next);
        rendererPositionToId.Add(next++, newId);

        if (type == RendererType.TEXT) {
            renderers[next - 1].textRenderer.textId = TextRenderer.textIdProvider.NextId();
        }

        return newId;
    }

    /// <summary>
    /// Destroys the renderer with the specified id.
    /// </summary>
    /// <param name="id"></param>
    public static void DestroyRenderer(int id) {
        if (!rendererIdToPosition.TryGetValue(id, out int cPos)) {
            throw new Exception($"The renderer with the id {id} does not exist; Unable to destroy.");
        }
        
        if (next == 1) {
            if (renderers[0].rendererType == RendererType.TEXT) { 
                TextRenderer.textIdProvider.FreeId(renderers[0].textRenderer.textId);
            }
            
            renderers[0] = default;
            
            idProvider.FreeId(id);
            
            rendererIdToPosition.Remove(id);
            rendererPositionToId.Remove(0);
            
            next = 0;
        }
        else if (rendererIdToPosition[id] == next - 1) {
            if (renderers[cPos].rendererType == RendererType.TEXT) { 
                TextRenderer.textIdProvider.FreeId(renderers[cPos].textRenderer.textId);
            }
            
            renderers[cPos] = default;
            
            idProvider.FreeId(id);
            
            rendererIdToPosition.Remove(id);
            rendererPositionToId.Remove(cPos);

            next--;
        }
        else {
            if (renderers[cPos].rendererType == RendererType.TEXT) { 
                TextRenderer.textIdProvider.FreeId(renderers[cPos].textRenderer.textId);
            }
            
            int last = --next;
                
            // Override the removed sprite with the last sprite to close the gap.
            renderers[cPos] = renderers[last];
            // Remove the now duplicated last sprite.
            renderers[last] = default;
            
            // Free the id of the removed sprite.
            idProvider.FreeId(id);

            // Retrieve the id of the moved sprite.
            int movedId = rendererPositionToId[last];
            
            // Update the id of the (formerly) last sprite with its new position
            rendererIdToPosition[movedId] = cPos;
            // Update the new position of the (formerly) last sprite with its id
            rendererPositionToId[cPos] = movedId;
            // Remove the now unused entry for the last position
            rendererPositionToId.Remove(last);
            // Remove the id entry of the removed sprite.
            rendererIdToPosition.Remove(id);
        }
    }

    /// <summary>
    /// Retrieves the renderer with the specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static ref Renderer GetRenderer(int id) {
        if (!rendererIdToPosition.TryGetValue(id, out int cPos)) {
            throw new Exception($"The renderer with the id {id} does not exist; Unable to retrieve.");
        }
        
        return ref renderers[cPos];
    }

    private static void IncrementCapacity() {
        int newCapacity = Capacity * 2;
        
        if (renderers.Length >= newCapacity) {
            return;
        }

        Renderer[] newArr = new Renderer[newCapacity];

        fixed (Renderer* src = renderers) {
            fixed (Renderer* dest = newArr) {
                Buffer.MemoryCopy(src, dest,
                    newCapacity * RENDERER_SIZE, next * RENDERER_SIZE);
            }
        }

        renderers = newArr;

        rendererPositionToId.EnsureCapacity(newCapacity);
        rendererIdToPosition.EnsureCapacity(newCapacity);

        memory = renderers;
    }

    public static Span<Renderer> AsSpan() {
        return memory[..next].Span;
    }
    
    /// <summary>
    /// Destroys all renderers.
    /// </summary>
    public static void DestroyAll() {
        renderers = null;
        rendererIdToPosition = null;
        rendererPositionToId = null;

        memory = null;
        
        next = 0;

        idProvider = null;

        IsInitialized = false;
    }
}