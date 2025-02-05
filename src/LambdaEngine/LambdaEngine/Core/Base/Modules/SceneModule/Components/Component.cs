namespace LambdaEngine;

public class Component {
    public GameObject gameObject;
    public Transform transform;
    
    public virtual void Initialize() { }

    public virtual void Update() { }
}