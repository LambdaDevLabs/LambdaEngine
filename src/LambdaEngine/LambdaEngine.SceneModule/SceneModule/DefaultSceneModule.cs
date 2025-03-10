using System.Reflection;
using LambdaEngine.DebugSystem;

namespace LambdaEngine.SceneModule;

public class DefaultSceneModule : ISceneModule {
    internal Dictionary<Type, TypeEventMethodPointers> typeEventMethodPointers;
    
    public void Initialize() {
        RegisterLifecycleEventTypeDelegates();
        
        Debug.Log("DefaultSceneModule initialized.", LogLevel.INFO);
    }

    private void RegisterLifecycleEventTypeDelegates() {
        // Ignore nullable assembly as this is not unmanaged code.
        Type[] types = Assembly.GetEntryAssembly()!.GetTypes();

        int behaviourTypeCount = types.Count(type => type.IsSubclassOf(typeof(BehaviourComponent)));
        
        typeEventMethodPointers = new Dictionary<Type, TypeEventMethodPointers>(behaviourTypeCount);

        foreach (Type type in types) {
            bool isValidType = type.IsSubclassOf(typeof(BehaviourComponent));

            TypeEventMethodPointers.TypeEventMethodPointersFactory factory = new();
            
            List<Type> foundAttributes = new(8);
            
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                bool isValidMethod = method is { IsVirtual: true, IsPrivate: false } && method.ReturnType == typeof(void);
                
                IEnumerable<LifecycleAttribute> attributes = method.GetCustomAttributes<LifecycleAttribute>();
                
                switch (attributes.Count()) {
                    case 1 when !isValidType:
                        throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                            $"lifecycle attributes may only be used in Components derived from {nameof(BehaviourComponent)}.");
                    
                    case 1: {
                        switch (attributes.First()) {
                            case LifecycleAwakeAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 0) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleAwakeAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleAwake attributes in type {type.Name}: only one lifecycleAwake attribute is allowed per class.");
                                }
                                
                                Delegate delAwake = Delegate.CreateDelegate(typeof(Action), null, method);
                                factory.AddAwakeEventMethod(delAwake);
                                
                                foundAttributes.Add(typeof(LifecycleAwakeAttribute));
                                break;
                            
                            case LifecycleStartAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 0) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleStartAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleStart attributes in type {type.Name}: only one lifecycleStart attribute is allowed per class.");
                                }
                                
                                Delegate delStart = Delegate.CreateDelegate(typeof(Action), null, method);
                                factory.AddStartEventMethod(delStart);
                                
                                foundAttributes.Add(typeof(LifecycleStartAttribute));
                                break;
                            
                            case LifecycleFixedUpdateAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 0) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleFixedUpdateAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleFixedUpdate attributes in type {type.Name}: only one lifecycleFixedUpdate attribute is allowed per class.");
                                }
                                
                                Delegate delFixedUpdate = Delegate.CreateDelegate(typeof(Action), null, method);
                                factory.AddFixedUpdateEventMethod(delFixedUpdate);
                                
                                foundAttributes.Add(typeof(LifecycleFixedUpdateAttribute));
                                break;
                            
                            case LifecycleUpdateAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 0) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleUpdateAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleUpdate attributes in type {type.Name}: only one lifecycleUpdate attribute is allowed per class.");
                                }

                                Delegate delUpdate = Delegate.CreateDelegate(typeof(Action), null, method);
                                factory.AddUpdateEventMethod(delUpdate);
                                
                                foundAttributes.Add(typeof(LifecycleUpdateAttribute));
                                break;
                            
                            case LifecycleDestroyAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 0) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleDestroyAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleDestroy attributes in type {type.Name}: only one lifecycleDestroy attribute is allowed per class.");
                                }

                                Delegate delDestroy = Delegate.CreateDelegate(typeof(Action), null, method);
                                factory.AddDestroyEventMethod(delDestroy);
                                
                                foundAttributes.Add(typeof(LifecycleDestroyAttribute));
                                break;
                            
                            case LifecycleCollisionEnterAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 1) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }

                                if (method.GetParameters()[0].ParameterType != typeof(Collision)) {
                                    throw new Exception($"Invalid use of collision lifecycle attribute in method {method.Name} of class {type.Name}:" +
                                                        $"Collision lifecycle attributes may only be used on non-private virtual methods with a return type" +
                                                        $"of void and one parameter of type {typeof(Collision)}.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleCollisionEnterAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleCollisionEnter attributes in type {type.Name}: only one lifecycleCollisionEnter attribute is allowed per class.");
                                }

                                Action<Collision> delCollisionEnter = (Action<Collision>)Delegate.CreateDelegate(typeof(Action<Collision>), null, method);
                                factory.AddCollisionEnterEventMethod(delCollisionEnter);
                                
                                foundAttributes.Add(typeof(LifecycleCollisionEnterAttribute));
                                break;
                            
                            case LifecycleCollisionStayAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 1) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }

                                if (method.GetParameters()[0].ParameterType != typeof(Collision)) {
                                    throw new Exception($"Invalid use of collision lifecycle attribute in method {method.Name} of class {type.Name}:" +
                                                        $"Collision lifecycle attributes may only be used on non-private virtual methods with a return type" +
                                                        $"of void and one parameter of type {typeof(Collision)}.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleCollisionStayAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleCollisionStay attributes in type {type.Name}: only one lifecycleCollisionStay attribute is allowed per class.");
                                }

                                Action<Collision> delCollisionStay = (Action<Collision>)Delegate.CreateDelegate(typeof(Action<Collision>), null, method);
                                factory.AddCollisionStayEventMethod(delCollisionStay);
                                
                                foundAttributes.Add(typeof(LifecycleCollisionStayAttribute));
                                break;
                            
                            case LifecycleCollisionExitAttribute:
                                if (!isValidMethod || method.GetParameters().Length != 1) {
                                    throw new Exception($"Invalid use of lifecycle attributes in method {method.Name} of class {type.Name}:" +
                                                        $"lifecycle attributes may only be used on non-private virtual methods with no parameters and a return type of void.");
                                }

                                if (method.GetParameters()[0].ParameterType != typeof(Collision)) {
                                    throw new Exception($"Invalid use of collision lifecycle attribute in method {method.Name} of class {type.Name}:" +
                                                        $"Collision lifecycle attributes may only be used on non-private virtual methods with a return type" +
                                                        $"of void and one parameter of type {typeof(Collision)}.");
                                }
                                
                                if (foundAttributes.Contains(typeof(LifecycleCollisionExitAttribute))) {
                                    throw new Exception(
                                        $"Invalid use of lifecycleCollisionExit attributes in type {type.Name}: only one lifecycleCollisionExit attribute is allowed per class.");
                                }

                                Action<Collision> delCollisionExit = (Action<Collision>)Delegate.CreateDelegate(typeof(Action<Collision>), null, method);
                                factory.AddCollisionExitEventMethod(delCollisionExit);
                                
                                foundAttributes.Add(typeof(LifecycleCollisionExitAttribute));
                                break;
                        }
                        
                        break;
                    }
                    
                    case > 1:
                        throw new Exception(
                            $"Invalid use of lifecycle attributes in method {method.Name} of type {type.Name}: only one lifecycle attribute is allowed per method.");
                }
            }

            if (isValidType) {
                typeEventMethodPointers.Add(type, factory.Create());
            }
        }
    }
}