using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    private Vector2 _position;
    private float _zoom;
    private float _rotation;
    private Viewport _viewport;

    public Camera(Viewport viewport)
    {
        _viewport = viewport;
        _position = Vector2.Zero;
        _zoom = 2.0f;
        _rotation = 0.0f;
    }

    public void HandleScreenResize(GraphicsDevice graphicsDevice)
    {

    }

    public Matrix GetTransformation()
    {
        return Matrix.CreateTranslation(new Vector3(-_position, 0)) *
               Matrix.CreateRotationZ(_rotation) *
               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    public void SetZoom(float zoom)
    {
        _zoom = MathHelper.Clamp(zoom, 0.1f, 10.0f);
    }

    public void SetRotation(float rotation)
    {
        _rotation = rotation;
    }
}
