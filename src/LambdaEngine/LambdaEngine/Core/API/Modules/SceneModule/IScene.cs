using System.Numerics;

namespace LambdaEngine.SceneModule;

public interface IScene {
    public GameObject Instantiate();
    
    public GameObject Instantiate(Vector2 position);
}