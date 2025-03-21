﻿using System.Numerics;
using LambdaEngine.DebugSystem;
using LambdaEngine.RenderSystem;

namespace LambdaEngine.SceneModule;

public class SpriteRenderer : Component, ITransformListener {
    internal readonly int spriteRendererId;
    
    private Vector2 scale;
    private Sprite sprite;
    private Color color;
    private sbyte layer;

    /// <summary>
    /// The x and y scale of the sprite.
    /// </summary>
    public Vector2 Scale {
        get => scale;
        set {
            scale = value;
            Renderer.SetRendererScale(spriteRendererId, scale);
        }
    }
    
    /// <summary>
    /// The sprite to be rendered.
    /// </summary>
    public Sprite Sprite {
        get => sprite;
        set {
            sprite = value;

            Renderer.SetRendererSprite(spriteRendererId, sprite.spriteId);
        }
    }

    /// <summary>
    /// The color applied to the sprite.
    /// </summary>
    public Color Color {
        get => color;
        set {
            color = value;
            Renderer.SetRendererColor(spriteRendererId, color);
        }
    }

    /// <summary>
    /// The rendering layer of the sprite.
    /// </summary>
    public sbyte Layer {
        get => layer;
        set {
            layer = value;
            Renderer.SetRendererLayer(spriteRendererId, (byte)(layer + 128));
        }
    }

    public SpriteRenderer() {
        spriteRendererId = Renderer.CreateRenderer(RendererType.SPRITE);

        scale = Vector2.One;
        color = Color.White;
        
        Renderer.SetRendererPosition(spriteRendererId, Vector2.Zero);
        Renderer.SetRendererScale(spriteRendererId, scale);
        Renderer.SetRendererColor(spriteRendererId, color);
        Renderer.SetRendererSprite(spriteRendererId, -1);
    }

    internal override void Initialize() {
        transform.RegisterTransformListener(this);
    }

    void ITransformListener.TransformUpdate(Transform transform) {
        Renderer.SetRendererPosition(spriteRendererId, transform.Position);
    }

    internal override void DestroyComponent() {
        transform.UnregisterTransformListener(this);
        
        Renderer.DestroyRenderer(spriteRendererId);

        sprite = null!;
        
        base.DestroyComponent();
    }
}