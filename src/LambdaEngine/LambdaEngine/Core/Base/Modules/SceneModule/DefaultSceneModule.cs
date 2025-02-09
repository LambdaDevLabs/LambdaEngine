using System.Reflection;
using LambdaEngine.DebugSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngine;

public class DefaultSceneModule : ISceneModule {
    private Dictionary<Type, Delegate> lifecycleUpdateDelegates;
    
    public void Initialize() {
        RegisterLifecycleEventHandler();
        
        Debug.Log("DefaultSceneModule initialized.", LogLevel.INFO);
    }

    private void RegisterLifecycleEventHandler() {
        lifecycleUpdateDelegates = new Dictionary<Type, Delegate>(64);
        
        foreach (Type type in Assembly.GetEntryAssembly().GetTypes()) {
            if (type.IsSubclassOf(typeof(BehaviourComponent))) {
                bool foundAttribute = false;
                
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                    IEnumerable<LifecycleUpdateAttribute> attributes =
                        method.GetCustomAttributes<LifecycleUpdateAttribute>();
                    
                    if (attributes != null && attributes.Any()) {
                        if (foundAttribute) {
                            throw new Exception(
                                $"Invalid use of attribute {typeof(LifecycleUpdateAttribute)}; this attribute can only be use once per class.");
                        }

                        Delegate del = Delegate.CreateDelegate(typeof(Action), null, method);
                        lifecycleUpdateDelegates[type] = del;

                        foundAttribute = true;
                    }
                }
            }
        }

        foreach (KeyValuePair<Type,Delegate> lifecycleUpdateDelegate in lifecycleUpdateDelegates) {
            Debug.Log($"Type: {lifecycleUpdateDelegate.Key}, Delegate: {lifecycleUpdateDelegate.Value}", LogLevel.DEBUG);
        }
    }

    internal void InvokeUpdateEvents(ICollection<BehaviourComponent> components) {
        foreach (BehaviourComponent component in components) {
            Type type = component.GetType();
            if (lifecycleUpdateDelegates.TryGetValue(type, out Delegate? @delegate)) {
                @delegate.Method.Invoke(component, null);
            }
        }
    }
}