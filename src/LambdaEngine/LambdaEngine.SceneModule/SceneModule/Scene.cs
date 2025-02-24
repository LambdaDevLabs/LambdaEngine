using System.Numerics;

namespace LambdaEngine.SceneModule;

public class Scene : IScene {
    private readonly DefaultSceneModule sceneModule;
    private readonly List<GameObject> gameObjects = new(64);
    private readonly List<BehaviourComponent> behaviourComponents = new(64);
    private readonly List<BehaviourComponent> toStart = new(16);
    
    private readonly List<GameObject> toDestroyGameObjects = new(16);
    private readonly List<Component> toDestroyComponents = new(16);

    public Scene(DefaultSceneModule sceneModule) {
        this.sceneModule = sceneModule;
    }

    public void Initialize() {
        Time.SubscribeToStart(InvokeStartEvents);
        Time.SubscribeToUpdate(InvokeUpdateEvents);
        Time.SubscribeToDestroy(InvokeDestroyEvents);
    }

    public GameObject Instantiate() {
        return Instantiate(Vector2.Zero);
    }

    public GameObject Instantiate(Vector2 position) {
        Transform transform = new() {
            Position = position
        };

        GameObject gameObject = new(this) {
            transform = transform
        };
        
        transform.transform = transform;
        transform.gameObject = gameObject;
        
        gameObjects.Add(gameObject);
        
        return gameObject;
    }

    internal T CreateComponent<T>(GameObject gameObject) where T : Component, new() {
        T t = new();
        
        gameObject.components.Add(t);
        
        t.gameObject = gameObject;
        t.transform = gameObject.transform;
        
        t.Initialize();

        if (t is not BehaviourComponent bc) {
            return t;
        }
        
        behaviourComponents.Add(bc);
        toStart.Add(bc);
            
        InvokeAwakeEvent(bc);

        return t;
    }

    public void Destroy(GameObject gameObject) {
        toDestroyGameObjects.Add(gameObject);
    }

    public void Destroy(Component component) {
        toDestroyComponents.Add(component);
    }

    private void InvokeAwakeEvent(BehaviourComponent bc) {
        if (bc.Awake) {
            return;
        }
        
        if (sceneModule.typeEventMethodPointers[bc.GetType()].HasLifecycleEvent(LifecycleEvents.AWAKE)) {
            sceneModule.typeEventMethodPointers[bc.GetType()].awakeEventMethod!.Method.Invoke(bc, null);
        }

        bc.Awake = true;
    }

    private void InvokeStartEvents() {
        List<BehaviourComponent> toStart_ = new(toStart);
        
        foreach (BehaviourComponent bc in toStart_) {
            if (bc.Started) {
                continue;
            }
            
            if (sceneModule.typeEventMethodPointers[bc.GetType()].HasLifecycleEvent(LifecycleEvents.START)) {
                sceneModule.typeEventMethodPointers[bc.GetType()].startEventMethod!.Method.Invoke(bc, null);
            }

            bc.Started = true;
            toStart.Remove(bc);
        }
    }
    
    private void InvokeUpdateEvents() {
        List<BehaviourComponent> toUpdate = new(behaviourComponents);
        
        foreach (BehaviourComponent bc in toUpdate) {
            if (sceneModule.typeEventMethodPointers[bc.GetType()].HasLifecycleEvent(LifecycleEvents.UPDATE)) {
                sceneModule.typeEventMethodPointers[bc.GetType()].updateEventMethod!.Method.Invoke(bc, null);
            }
        }
    }

    private void InvokeDestroyEvents() {
        List<GameObject> toDestroyGameObjects_ = new(toDestroyGameObjects);
        
        foreach (GameObject gameObject in toDestroyGameObjects_) {
            foreach (Component component in gameObject.GetComponents<Component>()) {
                if (component is BehaviourComponent bc) {
                    if (sceneModule.typeEventMethodPointers[bc.GetType()].HasLifecycleEvent(LifecycleEvents.DESTROY)) {
                        sceneModule.typeEventMethodPointers[bc.GetType()].destroyEventMethod!.Method.Invoke(bc, null);
                    }
                    
                    behaviourComponents.Remove(bc);
                    toStart.Remove(bc);
                }
                
                component.DestroyComponent();
                
                toDestroyComponents.Remove(component);
            }
            
            toDestroyGameObjects.Remove(gameObject);
            gameObjects.Remove(gameObject);
        }

        List<Component> toDestroyComponents_ = new(toDestroyComponents);
        foreach (Component component in toDestroyComponents_) {
            if (component is BehaviourComponent bc) {
                if (sceneModule.typeEventMethodPointers[bc.GetType()].HasLifecycleEvent(LifecycleEvents.DESTROY)) {
                    sceneModule.typeEventMethodPointers[bc.GetType()].destroyEventMethod!.Method.Invoke(bc, null);
                }
                
                behaviourComponents.Remove(bc);
                toStart.Remove(bc);
            }
            
            component.gameObject.RemoveComponent(component);
            component.DestroyComponent();
            
            toDestroyComponents.Remove(component);
        }
    }
}