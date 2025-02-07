using System.Numerics;
using LambdaEngine.SceneModule;

namespace LambdaEngine;

public class Scene : IScene {
    private readonly List<GameObject> gameObjects = new(64);

    public GameObject Instantiate() {
        return Instantiate(Vector2.Zero);
    }

    public GameObject Instantiate(Vector2 position) {
        Transform transform = new() {
            Position = position
        };

        GameObject gameObject = new() {
            transform = transform
        };
        
        transform.transform = transform;
        transform.gameObject = gameObject;

        gameObject.scene = this;
        
        gameObjects.Add(gameObject);
        
        return gameObject;
    }

    public void DestroyGameObjects(GameObject gameObject) {
        gameObjects.Remove(gameObject);
    }
}