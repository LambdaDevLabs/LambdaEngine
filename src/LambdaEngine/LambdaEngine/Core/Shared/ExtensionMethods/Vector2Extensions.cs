using System.Numerics;

namespace LambdaEngine;

public static class Vector2Extensions {
    private static Vector2 Vec2NaN {
        get => new(float.NaN, float.NaN);
    }
    
    /// <summary>
    /// <para>Returns a vector with the same direction as the specified vector, but with a length of one.</para>
    /// <para>Invalid vectors are handled gracefully by returning an unmodified vector.</para>
    /// </summary>
    /// <param name="vector">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Vector2 Normalize(this Vector2 vector) {
        if (vector == Vector2.Zero || vector == Vec2NaN) {
            return vector;
        }

        return Vector2.Normalize(vector);
    }
}