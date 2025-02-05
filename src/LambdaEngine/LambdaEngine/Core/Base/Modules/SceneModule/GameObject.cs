using System.Numerics;

namespace LambdaEngine;

public class GameObject {
    private readonly List<Component> components = new(16);
    public required Transform transform;

    public T CreateComponent<T>() where T : Component, new() {
        T c = new();
        components.Add(c);
        
        c.gameObject = this;
        c.transform = transform;
        
        c.Initialize();
        
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

    public void Update() {
        foreach (Component component in components) {
            component.Update();
        }
    }

    public static GameObject Instantiate() {
        Transform transform = new();

        GameObject gameObject = new() {
            transform = transform
        };
        
        transform.transform = transform;
        transform.gameObject = gameObject;
        
        return gameObject;
    }
    
    public static GameObject Instantiate(Vector2 position) {
        Transform transform = new();

        GameObject gameObject = new() {
            transform = new Transform()
        };
        
        transform.transform = transform;
        transform.gameObject = gameObject;
        
        transform.Position = position;
        
        return gameObject;
    }
}