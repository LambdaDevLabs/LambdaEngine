namespace LambdaEngine;

public static class LMath {
    public static int Repeat(int value, int min, int max) {
        if (min > max) {
            throw new ArgumentException("min must be less than max.");
        }
        
        int range = max - min;
        return ((value - min) % range + range) % range + min;
    }
}