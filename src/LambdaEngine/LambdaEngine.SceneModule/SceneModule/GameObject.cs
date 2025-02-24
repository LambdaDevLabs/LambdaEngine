namespace LambdaEngine.SceneModule;

public class GameObject {
    internal readonly List<Component> components = new(16);
    internal readonly Scene scene;
    public required Transform transform;

    internal GameObject(Scene scene) {
        this.scene = scene;
    }
    
    public T CreateComponent<T>() where T : Component, new() {
        T c = scene.CreateComponent<T>(this);
        
        return c;
    }

    public T GetComponent<T>() where T : Component, new() {
        foreach (Component component in components) {
            if (component is T t) {
                return t;
            }
        }

        return null!;
    }

    public Component[] GetComponents() {
        return components.ToArray();
    }

    public T[] GetComponents<T>() where T : Component, new() {
        List<T> components_ = new(components.Count);

        foreach (Component component in components) {
            if (component is T t) {
                components_.Add(t);
            }
        }

        components_.TrimExcess();
        return components_.ToArray();
    }

    public bool TryGetComponent<T>(out T component) where T : Component, new() {
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

    internal void RemoveComponent(Component component) {
        components.Remove(component);
    }
}