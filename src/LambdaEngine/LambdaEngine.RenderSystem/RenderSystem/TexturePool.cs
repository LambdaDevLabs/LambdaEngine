#nullable disable

using System.Security.Cryptography;
using LambdaEngine.DebugSystem;
using SDL3;

namespace LambdaEngine.RenderSystem;

internal static unsafe class TexturePool {
    private static IntPtr rendererHandle;
    
    private static Dictionary<Hash32, int> textureHashToId;
    private static Dictionary<int, int> textureIdToPosition;
    private static Dictionary<int, int> texturePositionToId;
    private static SDL.SDL_Texture*[] textures;

    private static IdProvider idProvider;
    
    private static int next;
    
    public static bool IsInitialized { get; private set; }

    public static int Capacity {
        get => textures.Length;
    }

    public static int CapacityLeft {
        get => textures.Length - next;
    }

    public static void Initialize(IntPtr rendererHandle, int bufferSize) {
        if (IsInitialized) {
            throw new Exception("Cannot initialize; already initialized.");
        }

        TexturePool.rendererHandle = rendererHandle;
        
        textures = new SDL.SDL_Texture*[bufferSize];
        texturePositionToId = new Dictionary<int, int>(bufferSize);
        textureIdToPosition = new Dictionary<int, int>(bufferSize);
        textureHashToId = new Dictionary<Hash32, int>(bufferSize);

        idProvider = new IdProvider(64, 4);

        IsInitialized = true;
    }

    /// <summary>
    /// Loads a new texture and returns its id.
    /// If the same texture is already loaded, this id is returned.
    /// </summary>
    /// <param name="path">The path of the bitmap file to load the texture from.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static int LoadTexture(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("File not found", path);
        }

        byte[] file = File.ReadAllBytes(path);
        Hash32 hash = new(SHA256.HashData(file));

        if (textureHashToId.TryGetValue(hash, out int id)) {
            return id;
        }

        if (CapacityLeft == 0) {
            IncrementCapacity();
        }

        SDL.SDL_Surface* surface = SDL.SDL_LoadBMP(path);
        if (surface == null) {
            throw new Exception($"Unable to load bitmap: {SDL.SDL_GetError()}");
        }

        SDL.SDL_Texture* texture = SDL.SDL_CreateTextureFromSurface(rendererHandle, new IntPtr(surface));

        if (texture == null) {
            throw new Exception($"Unable to create texture: {SDL.SDL_GetError()}");
        }

        SDL.SDL_DestroySurface(new IntPtr(surface));
        
        int newId = idProvider.NextId();

        textures[next] = texture;
        textureIdToPosition.Add(newId, next);
        texturePositionToId.Add(next, newId);

        textureHashToId.Add(hash, next++);

        return newId;
    }

    /// <summary>
    /// Unloads the texture with the specified id.
    /// </summary>
    /// <param name="id"></param>
    public static void UnloadTexture(int id) {
        if (!textureIdToPosition.TryGetValue(id, out int cPos)) {
            throw new Exception($"The texture with the id {id} does not exist; Unable to unload.");
        }
        
        SDL.SDL_DestroyTexture(new IntPtr(textures[cPos]));
        
        if (next == 1) {
            textures[0] = null;
            
            idProvider.FreeId(id);
            
            textureIdToPosition.Remove(id);
            texturePositionToId.Remove(0);
            
            next = 0;
        }
        else if (textureIdToPosition[id] == next - 1) {
            textures[cPos] = null;
            
            idProvider.FreeId(id);
            
            textureIdToPosition.Remove(id);
            texturePositionToId.Remove(cPos);

            next--;
        }
        else {
            int last = --next;
                
            // Override the removed sprite with the last sprite to close the gap.
            textures[cPos] = textures[last];
            // Remove the now duplicated last sprite.
            textures[last] = null;
            
            // Free the id of the removed sprite.
            idProvider.FreeId(id);

            // Retrieve the id of the moved sprite.
            int movedId = texturePositionToId[last];
            
            // Update the id of the (formerly) last sprite with its new position
            textureIdToPosition[movedId] = cPos;
            // Update the new position of the (formerly) last sprite with its id
            texturePositionToId[cPos] = movedId;
            // Remove the now unused entry for the last position
            texturePositionToId.Remove(last);
            // Remove the id entry of the removed sprite.
            textureIdToPosition.Remove(id);
        }
    }

    /// <summary>
    /// Retrieves the texture with the specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static SDL.SDL_Texture* GetTexture(int id) {
        if (!textureIdToPosition.TryGetValue(id, out int cPos)) {
            throw new Exception($"The texture with the id {id} does not exist; Unable to retrieve.");
        }
        
        return textures[cPos];
    }

    private static void IncrementCapacity() {
        int newCapacity = Capacity * 2;
        
        if (textures.Length >= newCapacity) {
            return;
        }

        SDL.SDL_Texture*[] newArr = new SDL.SDL_Texture*[newCapacity];

        fixed (SDL.SDL_Texture** src = textures) {
            fixed (SDL.SDL_Texture** dest = newArr) {
                Buffer.MemoryCopy(src, dest,
                    newCapacity * sizeof(SDL.SDL_Texture*), next * sizeof(SDL.SDL_Texture*));
            }
        }

        textures = newArr;

        texturePositionToId.EnsureCapacity(newCapacity);
        textureIdToPosition.EnsureCapacity(newCapacity);
    }
    
    /// <summary>
    /// Unloads all textures.
    /// </summary>
    public static void UnloadAll() {
        foreach (SDL.SDL_Texture* sdlTexture in textures) {
            SDL.SDL_DestroyTexture(new IntPtr(sdlTexture));
        }
        
        textureHashToId.Clear();
        textures = null;
        textureIdToPosition = null;
        texturePositionToId = null;
        next = 0;

        IsInitialized = false;
    }
}