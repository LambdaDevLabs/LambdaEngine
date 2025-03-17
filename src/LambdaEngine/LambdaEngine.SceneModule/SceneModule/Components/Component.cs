using System.Numerics;

namespace LambdaEngine.SceneModule;

/// <summary>
/// Base class for all components
/// </summary>
public abstract class Component {
    public GameObject gameObject;
    public Transform transform;

    internal virtual void Initialize() { }

    /// <summary>
    /// Creates a new <see cref="Component"/> of type <typeparamref name="T"/> for the gameObject of this component.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Component"/> to create.</typeparam>
    /// <returns></returns>
    public T CreateComponent<T>() where T : Component, new() {
        return gameObject.CreateComponent<T>();
    }

    /// <summary>
    /// Returns the first <see cref="Component"/> of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComponent<T>() where T : Component {
        return gameObject.GetComponent<T>();
    }
    
    /// <summary>
    /// Returns all components of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetComponents<T>() where T : Component {
        return gameObject.GetComponents<T>();
    }

    /// <summary>
    /// Tries to retrieve the first component of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="component">The retrieved component. If no component was found, this is null.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if a component was found, otherwise false.</returns>
    public bool TryGetComponent<T>(out T component) where T : Component {
        return gameObject.TryGetComponent(out component);
    }

    /// <summary>
    /// Instantiates a new <see cref="GameObject"/>.
    /// </summary>
    /// <returns></returns>
    public GameObject Instantiate() {
        return gameObject.scene.Instantiate();
    }

    /// <summary>
    /// Instantiates a new <see cref="GameObject"/> at the specified position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject Instantiate(Vector2 position) {
        return gameObject.scene.Instantiate(position);
    }
    
    internal virtual void DestroyComponent() {
        transform = null!;
        gameObject = null!;
    }
}