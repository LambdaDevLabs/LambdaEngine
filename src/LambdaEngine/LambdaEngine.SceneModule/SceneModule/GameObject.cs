namespace LambdaEngine.SceneModule;

public class GameObject {
    internal readonly List<Component> components = new(16);
    internal readonly Scene scene;
    public required Transform transform;

    internal GameObject(Scene scene) {
        this.scene = scene;
    }
    
    /// <summary>
    /// Creates a new <see cref="Component"/> of type <typeparamref name="T"/> for the gameObject of this component.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Component"/> to create.</typeparam>
    /// <returns></returns>
    public T CreateComponent<T>() where T : Component, new() {
        T c = scene.CreateComponent<T>(this);
        
        return c;
    }

    /// <summary>
    /// Returns the first <see cref="Component"/> of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComponent<T>() where T : class {
        foreach (Component component in components) {
            if (component is T t) {
                return t;
            }
        }

        return null!;
    }

    /// <summary>
    /// Returns all components attached to this gameObject.
    /// </summary>
    /// <returns></returns>
    public Component[] GetComponents() {
        return components.ToArray();
    }

    /// <summary>
    /// Returns all components of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetComponents<T>() where T : class {
        List<T> components_ = new(components.Count);

        foreach (Component component in components) {
            if (component is T t) {
                components_.Add(t);
            }
        }

        components_.TrimExcess();
        return components_.ToArray();
    }

    /// <summary>
    /// Tries to retrieve the first component of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="component">The retrieved component. If no component was found, this is null.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if a component was found, otherwise false.</returns>
    public bool TryGetComponent<T>(out T component) where T : class {
        foreach (Component c in components) {
            if (c is not T t) {
                continue;
            }

            component = t;
            return true;
        }

        component = null!;
        return false;
    }

    /// <summary>
    /// Returns true if the gameObject has at least one component of the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>() where T : class {
        foreach (Component c in components) {
            if (c is not T) {
                continue;
            }

            return true;
        }

        return false;
    }

    internal void RemoveComponent(Component component) {
        components.Remove(component);
    }
}