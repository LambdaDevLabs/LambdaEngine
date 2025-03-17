using System.Numerics;

namespace LambdaEngine.SceneModule;

public class Transform : Component {
    private readonly List<ITransformListener> listeners = new(4);
    private Vector2 position;
    
    public Vector2 Position {
        get => position;
        set {
            position = value;
            UpdateListeners();
        }
    }

    private void UpdateListeners() {
        foreach (ITransformListener listener in listeners) {
            listener.TransformUpdate(this);
        }
    }

    internal void RegisterTransformListener(ITransformListener listener) {
        listeners.Add(listener);
    }

    internal void UnregisterTransformListener(ITransformListener listener) {
        listeners.Remove(listener);
    }

    public void Translate(Vector2 translation) {
        Position += translation;
    }

    public void Translate(float x, float y) {
        Position += new Vector2(x, y);
    }

    internal override void DestroyComponent() {
        listeners.Clear();
        
        base.DestroyComponent();
    }
}