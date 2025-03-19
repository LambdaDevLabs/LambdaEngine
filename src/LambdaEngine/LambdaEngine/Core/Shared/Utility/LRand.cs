namespace LambdaEngine;

public static class LRand {
    private static Random random = new();

    public static void Init(int seed) {
        random = new Random(seed);
    }

    public static int RandomInt(int min, int max) {
        if (min > max) {
            throw new ArgumentException("min must be less than max");
        }
        
        return random.Next(min, max);
    }
    
    public static long RandomLong(long min, long max) {
        if (min > max) {
            throw new ArgumentException("min must be less than max");
        }
        
        return random.NextInt64(min, max);
    }

    public static float RandomFloat(float min, float max) {
        if (min > max) {
            throw new ArgumentException("min must be less than max");
        }

        return random.NextSingle() * (max - min) + min;
    }

    public static double RandomDouble(double min, double max) {
        if (min > max) {
            throw new ArgumentException("min must be less than max");
        }

        return random.NextDouble() * (max - min) + min;
    }
}