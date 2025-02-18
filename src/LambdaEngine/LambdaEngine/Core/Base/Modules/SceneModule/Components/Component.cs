using System.Numerics;

namespace LambdaEngine;

public class Component {
    public GameObject gameObject;
    public Transform transform;
    
    public virtual void Initialize() { }

    public T CreateComponent<T>() where T : Component, new() {
        return gameObject.CreateComponent<T>();
    }

    public T GetComponent<T>() where T : Component, new() {
        return gameObject.GetComponent<T>();
    }

    public bool TryGetComponent<T>(out T component) where T : Component, new() {
        return gameObject.TryGetComponent(out component);
    }

    public GameObject Instantiate() {
        return gameObject.scene.Instantiate();
    }

    public GameObject Instantiate(Vector2 position) {
        return gameObject.scene.Instantiate(position);
    } 
}