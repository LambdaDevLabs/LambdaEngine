using LambdaEngine.SceneModule;

namespace LambdaEngine;

public class Scene : IScene {
    private readonly List<GameObject> gameObjects = new(64);

    public void AddGameObject(GameObject gameObject) {
        gameObjects.Add(gameObject);
    }

    public void DestroyGameObjects(GameObject gameObject) {
        gameObjects.Remove(gameObject);
    }
    
    public void OnStart() {
        throw new NotImplementedException();
    }
    
    public void OnUpdate() {
        foreach (GameObject gameObject in gameObjects) {
            gameObject.Update();
        }
    }
    
    public void OnStop() {
        gameObjects.Clear();
    }
}