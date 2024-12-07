using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    private Vector2 _position;
    private float _zoom;
    private float _rotation;
    private Viewport _viewport;
    private readonly int _resolutionWidth;
    private readonly int _resolutionHeight;
    private int virtualHeight;
    private int virtualWidth;


    public Camera(Viewport viewport, int resWidth,int resHeight)
    {
        _viewport = viewport;
        _position = Vector2.Zero;
        _zoom = 2.0f;
        _rotation = 0.0f;
        _resolutionWidth = resWidth;
        _resolutionHeight = resHeight;
    }

    public void HandleScreenResize(GraphicsDevice graphicsDevice)
    {
        int curWindowHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
        int curWindowWidth = graphicsDevice.PresentationParameters.BackBufferWidth;

        if (curWindowWidth/ _resolutionWidth > curWindowHeight / _resolutionHeight)
        {
            float aspect = (float)curWindowHeight / _resolutionHeight;
            virtualWidth = (int)aspect * _resolutionWidth;
            virtualHeight = curWindowHeight;
        }
        else
        {
            float aspect = (float)curWindowWidth / _resolutionWidth;
            virtualWidth = curWindowWidth;
            virtualHeight = (int)aspect * _resolutionHeight;
        }
        _zoom = virtualWidth / (float)_resolutionWidth;

        
    }

    public Matrix GetTransformation()
    {
        return Matrix.CreateTranslation(new Vector3(-_position, 0)) *
               Matrix.CreateRotationZ(_rotation) *
               Matrix.CreateScale(_zoom) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    public void SetZoom(float zoom)
    {
        //_zoom = MathHelper.Clamp(zoom, 0.1f, 10.0f);
    }

    public void SetRotation(float rotation)
    {
        _rotation = rotation;
    }
}
