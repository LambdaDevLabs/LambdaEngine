using System.Numerics;

namespace LambdaEngine.SceneModule;

public class Transform : Component {
    private readonly List<ITransformListener> listeners = new(4);
    private Vector2 position;
    
    /// <summary>
    /// The position of this transform
    /// </summary>
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

    /// <summary>
    /// Applies the specified translation to the position of this transform.
    /// </summary>
    /// <param name="translation"></param>
    public void Translate(Vector2 translation) {
        Position += translation;
    }

    /// <summary>
    /// Applies the specified translation to the position of this transform.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Translate(float x, float y) {
        Position += new Vector2(x, y);
    }

    internal override void DestroyComponent() {
        listeners.Clear();
        
        base.DestroyComponent();
    }
}