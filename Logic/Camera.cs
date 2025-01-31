using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Map;

public class Camera
{
    public Vector2 _position;
    private float _resolutionScalar;
    private float _zoom = 1;
    private float _rotation;
    public Viewport _viewport;
    public readonly int _resolutionWidth;
    public readonly int _resolutionHeight;
    public int virtualHeight;
    public int virtualWidth;
    private Vector2 _minPos, _maxPos;

    public bool TopScreenClamp;


    public Camera(Viewport viewport, int resWidth, int resHeight)
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

        if (screenWidth / _resolutionWidth > screenHeight / _resolutionHeight)
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
               Matrix.CreateScale(_resolutionScalar * _zoom) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
    }

    public void SetZoom(float newZoom)
    {
        _zoom = newZoom;
    }

    public void SetBounds(Vector2 mapSize)
    {
        //TODO: Handle zoom type issues HERE, for bounding to work in current state, zoom must be 1!
        float halfWidth = (_resolutionWidth / 2f) / _zoom;
        float halfHeight = (_resolutionHeight / 2f) / _zoom;

        // Calculate the map size (considering the size of the map in world coordinates)
        _minPos = new Vector2(halfWidth - TileManager.BASE_TILESIZE / 2, halfHeight - TileManager.BASE_TILESIZE / 2);

        // Max position calculation (the camera can go as far as the end of the map minus half of its size)
        _maxPos = new Vector2((mapSize.X + 1) * TileManager.BASE_TILESIZE - halfWidth - TileManager.BASE_TILESIZE / 2,
                              (mapSize.Y + 1) * TileManager.BASE_TILESIZE - halfHeight - TileManager.BASE_TILESIZE / 2);
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;

        // Check if HUD needs to move to bottom
        TopScreenClamp = _minPos.Y > position.Y ? true : false;

        // Clamp the camera position to the valid range considering the map boundaries
        _position = Vector2.Clamp(_position, _minPos, _maxPos);
    }
}
