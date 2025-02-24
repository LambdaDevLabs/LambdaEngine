namespace LambdaEngine.SceneModule;

internal readonly struct TypeEventMethodPointers {
    private readonly LifecycleEvents lifecycleEvents;

    public readonly Delegate? awakeEventMethod;
    public readonly Delegate? startEventMethod;
    public readonly Delegate? updateEventMethod;
    public readonly Delegate? destroyEventMethod;

    private TypeEventMethodPointers(Delegate? awakeEventMethod, Delegate? startEventMethod, Delegate? updateEventMethod, Delegate? destroyEventMethod) {
        this.awakeEventMethod = awakeEventMethod;
        this.startEventMethod = startEventMethod;
        this.updateEventMethod = updateEventMethod;
        this.destroyEventMethod = destroyEventMethod;
        
        lifecycleEvents = LifecycleEvents.NONE;
        
        if (this.awakeEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.AWAKE;
        }

        if (this.startEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.START;
        }

        if (this.updateEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.UPDATE;
        }

        if (this.destroyEventMethod != null) {
            lifecycleEvents |= LifecycleEvents.DESTROY;
        }
    }
    
    public bool HasLifecycleEvent(LifecycleEvents lifecycleEvent) {
        return (lifecycleEvents & lifecycleEvent) == lifecycleEvent;
    }

    public class TypeEventMethodPointersFactory {
        private Delegate? awakeEventMethod;
        private Delegate? startEventMethod;
        private Delegate? updateEventMethod;
        private Delegate? destroyEventMethod;

        public void AddAwakeEventMethod(Delegate awakeEventMethod) {
            this.awakeEventMethod = awakeEventMethod;
        }
        
        public void AddStartEventMethod(Delegate startEventMethod) {
            this.startEventMethod = startEventMethod;
        }

        public void AddUpdateEventMethod(Delegate updateEventMethod) {
            this.updateEventMethod = updateEventMethod;
        }

        public void AddDestroyEventMethod(Delegate destroyEventMethod) {
            this.destroyEventMethod = destroyEventMethod;
        }

        public TypeEventMethodPointers Create() {
            return new TypeEventMethodPointers(awakeEventMethod, startEventMethod, updateEventMethod, destroyEventMethod);
        }
    }
}