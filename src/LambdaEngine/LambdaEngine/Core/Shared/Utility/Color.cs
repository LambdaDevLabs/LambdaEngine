namespace LambdaEngine;

public struct Color {
    public static readonly Color White = new(1, 1, 1, 1);
    public static readonly Color Black = new(1, 0, 0, 0);
    
    public float a;
    public float r;
    public float g;
    public float b;

    public Color(float r, float g, float b) {
        a = 1;
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public Color(float a, float r, float g, float b) {
        this.a = a;
        this.r = r;
        this.g = g;
        this.b = b;
    }
}