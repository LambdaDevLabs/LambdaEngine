﻿using System.Numerics;

namespace LambdaEngine.SceneModule;

public class Component {
    public GameObject gameObject;
    public Transform transform;

    internal virtual void Initialize() { }

    public T CreateComponent<T>() where T : Component, new() {
        return gameObject.CreateComponent<T>();
    }

    public T GetComponent<T>() where T : Component {
        return gameObject.GetComponent<T>();
    }
    
    public T[] GetComponents<T>() where T : Component {
        return gameObject.GetComponents<T>();
    }

    public bool TryGetComponent<T>(out T component) where T : Component {
        return gameObject.TryGetComponent(out component);
    }

    public GameObject Instantiate() {
        return gameObject.scene.Instantiate();
    }

    public GameObject Instantiate(Vector2 position) {
        return gameObject.scene.Instantiate(position);
    }

    internal virtual void DestroyComponent() {
        transform = null!;
        gameObject = null!;
    }
}