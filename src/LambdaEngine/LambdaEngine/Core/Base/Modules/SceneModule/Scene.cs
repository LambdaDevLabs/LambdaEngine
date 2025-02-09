using System.Numerics;
using LambdaEngine.SceneModule;

namespace LambdaEngine;

public class Scene : IScene {
    private readonly DefaultSceneModule sceneModule;
    private readonly List<GameObject> gameObjects = new(64);

    public Scene(DefaultSceneModule sceneModule) {
        this.sceneModule = sceneModule;
    }

    public void Initialize() {
        Time.SubscribeToUpdate(InvokeUpdateEvent);
    }

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

    public void DestroyGameObject(GameObject gameObject) {
        gameObjects.Remove(gameObject);
    }

    private void InvokeUpdateEvent() {
        foreach (GameObject gameObject in gameObjects) {
            sceneModule.InvokeUpdateEvents(gameObject.GetComponents<BehaviourComponent>());
        }
    }
}