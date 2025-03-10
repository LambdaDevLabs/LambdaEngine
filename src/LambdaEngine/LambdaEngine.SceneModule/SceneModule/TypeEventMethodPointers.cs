namespace LambdaEngine.SceneModule;

internal readonly struct TypeEventMethodPointers {
    private readonly LifecycleEvents lifecycleEvents;

    public readonly Delegate? awakeEventMethod;
    public readonly Delegate? startEventMethod;
    public readonly Delegate? fixedUpdateEventMethod;
    public readonly Delegate? updateEventMethod;
    public readonly Delegate? destroyEventMethod;
    
    public readonly Action<Collision>? collisionEnterEventMethod;
    public readonly Action<Collision>? collisionStayEventMethod;
    public readonly Action<Collision>? collisionExitEventMethod;

    private TypeEventMethodPointers(Delegate? awakeEventMethod, Delegate? startEventMethod, Delegate? fixedUpdateEventMethod,
        Delegate? updateEventMethod, Delegate? destroyEventMethod, Action<Collision>? collisionEnterEventMethod,
        Action<Collision>? collisionStayEventMethod, Action<Collision>? collisionExitEventMethod) {
        this.awakeEventMethod = awakeEventMethod;
        this.startEventMethod = startEventMethod;
        this.fixedUpdateEventMethod = fixedUpdateEventMethod;
        this.updateEventMethod = updateEventMethod;
        this.destroyEventMethod = destroyEventMethod;
        
        this.collisionEnterEventMethod = collisionEnterEventMethod;
        this.collisionStayEventMethod = collisionStayEventMethod;
        this.collisionExitEventMethod = collisionExitEventMethod;
        
        lifecycleEvents = LifecycleEvents.NONE;
        
        if (this.awakeEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.AWAKE;
        }

        if (this.startEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.START;
        }

        if (this.fixedUpdateEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.FIXED_UPDATE;
        }

        if (this.updateEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.UPDATE;
        }

        if (this.destroyEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.DESTROY;
        }

        if (this.collisionEnterEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.COLLISION_ENTER;
        }
        
        if (this.collisionStayEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.COLLISION_STAY;
        }
        
        if (this.collisionExitEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.COLLISION_EXIT;
        }
    }
    
    public bool HasLifecycleEvent(LifecycleEvents lifecycleEvent) {
        return (lifecycleEvents & lifecycleEvent) == lifecycleEvent;
    }

    public class TypeEventMethodPointersFactory {
        private Delegate? awakeEventMethod;
        private Delegate? startEventMethod;
        private Delegate? fixedUpdateEventMethod;
        private Delegate? updateEventMethod;
        private Delegate? destroyEventMethod;
        
        private Action<Collision>? collisionEnterEventMethod;
        private Action<Collision>? collisionStayEventMethod;
        private Action<Collision>? collisionExitEventMethod;

        public void AddAwakeEventMethod(Delegate awakeEventMethod) {
            this.awakeEventMethod = awakeEventMethod;
        }
        
        public void AddStartEventMethod(Delegate startEventMethod) {
            this.startEventMethod = startEventMethod;
        }
        
        public void AddFixedUpdateEventMethod(Delegate fixedUpdateEventMethod) {
            this.fixedUpdateEventMethod = fixedUpdateEventMethod;
        }

        public void AddUpdateEventMethod(Delegate updateEventMethod) {
            this.updateEventMethod = updateEventMethod;
        }

        public void AddDestroyEventMethod(Delegate destroyEventMethod) {
            this.destroyEventMethod = destroyEventMethod;
        }
        
        public void AddCollisionEnterEventMethod(Action<Collision> collisionEnterEventMethod) {
            this.collisionEnterEventMethod = collisionEnterEventMethod;
        }
        
        public void AddCollisionStayEventMethod(Action<Collision> collisionStayEventMethod) {
            this.collisionStayEventMethod = collisionStayEventMethod;
        }
        
        public void AddCollisionExitEventMethod(Action<Collision> collisionExitEventMethod) {
            this.collisionExitEventMethod = collisionExitEventMethod;
        }

        public TypeEventMethodPointers Create() {
            return new TypeEventMethodPointers(awakeEventMethod, startEventMethod, fixedUpdateEventMethod, updateEventMethod,
                destroyEventMethod, collisionEnterEventMethod, collisionStayEventMethod, collisionExitEventMethod);
        }
    }
}