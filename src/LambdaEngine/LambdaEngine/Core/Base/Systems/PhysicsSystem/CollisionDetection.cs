namespace LambdaEngine.PhysicsSystem;

public static class CollisionDetection {
    public static Memory<Collision> CheckCollisions(Span<ColliderObject> colliders) {
        Collision[] collisions = new Collision[(int)(colliders.Length * (colliders.Length - 1) * 0.5)];
        int nextCIndex = 0;
        
        for (int i = 0; i < colliders.Length; i++) {
            ref ColliderObject a = ref colliders[i];
            for (int j = i + 1; j < colliders.Length; j++) {
                ref ColliderObject b = ref colliders[j];
                
                switch (a.type) {
                    case ColliderType.BOX when b.type == ColliderType.BOX:
                        if (CollidesBoxBox(ref a.boxCollider, ref b.boxCollider)) {
                            collisions[nextCIndex++] = new Collision(a.id, b.id);
                        }
                        break;

                    case ColliderType.BOX when b.type == ColliderType.CIRCLE:
                        if (CollidesBoxCircle(ref a.boxCollider, ref b.circleCollider)) {
                            collisions[nextCIndex++] = new Collision(a.id, b.id);
                        }
                        break;
                    
                    case ColliderType.CIRCLE when b.type == ColliderType.BOX:
                        if (CollidesBoxCircle(ref b.boxCollider, ref a.circleCollider)) {
                            collisions[nextCIndex++] = new Collision(a.id, b.id);
                        }
                        break;
                    
                    case ColliderType.CIRCLE when b.type == ColliderType.CIRCLE:
                        if (CollidesCircleCircle(ref a.circleCollider, ref b.circleCollider)) {
                            collisions[nextCIndex++] = new Collision(a.id, b.id);
                        }
                        break;
                }
            }
        }

        Memory<Collision> mem = new(collisions);
        
        return mem[..nextCIndex];
    }
    
    /// <summary>
    /// Computes a collision between a <see cref="BoxColliderObject"/> and a <see cref="BoxColliderObject"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Whether a collision was detected.</returns>
    public static bool CollidesBoxBox(ref BoxColliderObject a, ref BoxColliderObject b) {
        float maxXa = a.position.X + a.width * 0.5f;
        float minXa = maxXa - a.width;
        float maxYa = a.position.Y + a.height * 0.5f;
        float minYa = maxYa - a.height;
        
        float maxXb = b.position.X + b.width * 0.5f;
        float minXb = maxXb - b.width;
        float maxYb = b.position.Y + b.height * 0.5f;
        float minYb = maxYb - b.height;

        return !(maxXa < minXb || minXa > maxXb || maxYa < minYb || minYa > maxYb);
    }
    
    /// <summary>
    /// Computes a collision between a <see cref="BoxColliderObject"/> and a <see cref="CircleColliderObject"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Whether a collision was detected.</returns>
    public static bool CollidesBoxCircle(ref BoxColliderObject a, ref CircleColliderObject b) {
        float maxXa = a.position.X + a.width * 0.5f;
        float minXa = maxXa - a.width;
        float maxYa = a.position.Y + a.height * 0.5f;
        float minYa = maxYa - a.height;
        
        float minX = b.position.X < maxXa ? b.position.X : maxXa;
        float minY = b.position.Y < maxYa ? b.position.X : maxYa;
        
        float distX = b.position.X - (minXa > minX ? minXa : minX);
        float distY = b.position.Y - (minYa > minY ? minYa : minY);
        
        float distanceSquared = distX * distX + distY * distY;
        
        return distanceSquared <= b.radius * b.radius;
    }

    /// <summary>
    /// Computes a collision between a <see cref="CircleColliderObject"/> and a <see cref="CircleColliderObject"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Whether a collision was detected.</returns>
    public static bool CollidesCircleCircle(ref CircleColliderObject a, ref CircleColliderObject b) {
        float distX = b.position.X - a.position.X;
        float distY = b.position.Y - a.position.Y;
        
        float distanceSquared = distX * distX + distY * distY;
        
        float bothRadii = a.radius + b.radius;
        
        return distanceSquared <= bothRadii * bothRadii;
    }
}