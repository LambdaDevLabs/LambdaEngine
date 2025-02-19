#nullable disable

using System.Security.Cryptography;
using SDL3;

namespace LambdaEngine.RenderSystem;

public static unsafe class TexturePool {
    private static IntPtr rendererHandle;
    
    private static Dictionary<Hash32, int> textureHashes;
    private static SDL.SDL_Texture*[] textures;

    private static bool autoIncrement;
    
    private static int next;
    
    public static bool IsInitialized { get; private set; }

    public static int Capacity {
        get => textures.Length;
    }

    public static int CapacityLeft {
        get => textures.Length - next;
    }

    // ReSharper disable method ParameterHidesMember
    public static void Initialize(IntPtr rendererHandle, int bufferSize, bool autoIncrement) {
        if (IsInitialized) {
            throw new Exception("Cannot initialize; already initialized.");
        }

        TexturePool.rendererHandle = rendererHandle;
        
        textures = new SDL.SDL_Texture*[bufferSize];
        textureHashes = new Dictionary<Hash32, int>(bufferSize);
        
        TexturePool.autoIncrement = autoIncrement;

        IsInitialized = true;
    }

    public static SDL.SDL_Texture* GetOrLoadTexture(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("File not found", path);
        }

        byte[] file = File.ReadAllBytes(path);
        Hash32 hash = new(SHA256.HashData(file));

        if (textureHashes.TryGetValue(hash, out int value)) {
            return textures[value];
        }
        else {
            if (CapacityLeft == 0) {
                if (autoIncrement) {
                    IncrementCapacity();
                }
                else {
                    throw new Exception("Unable load new texture: no capacity left.");
                }
            }
            
            SDL.SDL_Surface* surface = SDL.SDL_LoadBMP(path);
            if (surface == null) {
                SDL.SDL_Log($"Unable to load bitmap: {SDL.SDL_GetError()}");
                return null!;
            }

            SDL.SDL_Texture* texture = SDL.SDL_CreateTextureFromSurface(rendererHandle, new IntPtr(surface));

            if (texture == null) {
                SDL.SDL_Log($"Unable to create static texture: {SDL.SDL_GetError()}");
                return null!;
            }

            SDL.SDL_DestroySurface(new IntPtr(surface));

            textures[next] = texture;
            textureHashes.Add(hash, next++);

            return texture;
        }
    }
    
    public static bool LoadNewTexture(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("File not found", path);
        }

        byte[] file = File.ReadAllBytes(path);
        Hash32 hash = new(SHA256.HashData(file));

        if (textureHashes.TryGetValue(hash, out int value)) {
            return false;
        }
        else {
            if (CapacityLeft == 0) {
                if (autoIncrement) {
                    IncrementCapacity();
                }
                else {
                    throw new Exception("Unable load new texture: no capacity left.");
                }
            }
            
            SDL.SDL_Surface* surface = SDL.SDL_LoadBMP(path);
            if (surface == null) {
                SDL.SDL_Log($"Unable to load bitmap: {SDL.SDL_GetError()}");
                return false;
            }

            SDL.SDL_Texture* texture = SDL.SDL_CreateTextureFromSurface(rendererHandle, new IntPtr(surface));

            if (texture == null) {
                SDL.SDL_Log($"Unable to create static texture: {SDL.SDL_GetError()}");
                return false;
            }

            SDL.SDL_DestroySurface(new IntPtr(surface));

            textures[next] = texture;
            textureHashes.Add(hash, next++);

            return true;
        }
    }

    public static void UnloadTexture(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("File not found", path);
        }
        
        Hash32 hash = new(SHA256.HashData(File.ReadAllBytes(path)));
        if (!textureHashes.TryGetValue(hash, out int value)) {
            throw new KeyNotFoundException($"Texture hash {hash} not found.");
        }
        
        SDL.SDL_DestroyTexture(new IntPtr(textures[value]));

        // Override the removed collider with the last collider to close the gap.
        textures[value] = textures[next - 1];
        // Remove the unused last collider for clarity, debugging nad possible threading.
        textures[--next] = null;
    }
    
    public static void UnloadTexture(Hash32 hash) {
        if (!textureHashes.TryGetValue(hash, out int value)) {
            throw new KeyNotFoundException($"Texture hash {hash} not found.");
        }
        
        SDL.SDL_DestroyTexture(new IntPtr(textures[value]));

        // Override the removed collider with the last collider to close the gap.
        textures[value] = textures[next - 1];
        // Remove the unused last collider for clarity, debugging nad possible threading.
        textures[--next] = null;
    }
    
    public static void UnloadTexture(IntPtr textureHandle) {
        SDL.SDL_Texture* texture = (SDL.SDL_Texture*)textureHandle;

        for (int i = 0; i < textures.Length; ++i) {
            if (textures[i] != texture) {
                continue;
            }

            Hash32? hash = null;
            foreach (KeyValuePair<Hash32, int> textureHash in textureHashes) {
                if (textureHash.Value == i) {
                    hash = textureHash.Key;
                }
            }

            if (hash != null) {
                textureHashes.Remove(hash.Value);
            }

            SDL.SDL_DestroyTexture(new IntPtr(texture));

            // Override the removed collider with the last collider to close the gap.
            textures[i] = textures[next - 1];
            // Remove the unused last collider for clarity, debugging nad possible threading.
            textures[--next] = null;

            return;
        }
        
        throw new KeyNotFoundException($"Texture {textureHandle} not found.");
    }

    private static void IncrementCapacity() {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Clears everything
    /// </summary>
    public static void Cleanup() {
        textureHashes.Clear();
        textures = null;
        textures = null;
        next = 0;

        IsInitialized = false;
    }
}