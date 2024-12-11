using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    private Vector2 _position;
    private float _resolutionScalar;
    private float _zoom = 1;
    private float _rotation;
    public Viewport _viewport;
    private readonly int _resolutionWidth;
    private readonly int _resolutionHeight;
    private int virtualHeight;
    private int virtualWidth;
    private Vector2 _minPos, _maxPos;


    public Camera(Viewport viewport, int resWidth,int resHeight)
    {
        _viewport = viewport;
        _position = Vector2.Zero;
        _resolutionScalar = 1.0f;
        _zoom = 1.0f;
        _rotation = 0.0f;
        _resolutionWidth = resWidth;
        _resolutionHeight = resHeight;
    }

    public void HandleScreenResize(GraphicsDevice graphicsDevice)
    {
        float screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
        float screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;

        if (screenWidth/ _resolutionWidth > screenHeight / _resolutionHeight)
        {
            float aspect = (float)screenHeight / _resolutionHeight;
            virtualWidth = (int)(aspect * _resolutionWidth);
            virtualHeight = (int)screenHeight;
        }
        else
        {
            float aspect = (float)screenWidth / _resolutionWidth;
            virtualWidth = (int)screenWidth;
            virtualHeight = (int)(aspect * _resolutionHeight);
        }
        _resolutionScalar = virtualWidth / (float)_resolutionWidth;
        _viewport = new Viewport
        {
            X = (int)(screenWidth / 2f - virtualWidth / 2f),
            Y = (int)(screenHeight / 2f - virtualHeight / 2f),
            Width = (int)virtualWidth,
            Height = (int)virtualHeight,
            MinDepth = 0,
            MaxDepth = 1
        };
    }

    public Matrix GetTransformation()
    {
        return Matrix.CreateTranslation(new Vector3(-_position, 0)) *
               Matrix.CreateRotationZ(_rotation) *
               Matrix.CreateScale(_resolutionScalar*_zoom) * 
               Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
    }

    public void SetZoom(float newZoom)
    {
        _zoom = newZoom;
    }

    public void SetBounds()
    {
        // Calculate the half-size of the viewport in world units
        float halfViewportWidth = virtualWidth / 2f;
        float halfViewportHeight = virtualHeight / 2f;
    }

    public void SetPosition(Vector2 position, Vector2 mapDims)
    {
        _position = position;

        // Clamp the camera position to the valid range
        _position = Vector2.Clamp(_position, _minPos, _maxPos);
    }
}
