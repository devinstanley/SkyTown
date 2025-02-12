using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using SkyTown.Map;

public static class Camera
{
    public static Vector2 Position;
    private static float _resolutionScalar = 1f;
    private static float _zoom = 1f;
    private static float _rotation = 0f;
    public static Viewport _viewport;
    public static int ResolutionWidth
    {
        get { return GameGlobals.InGameResolution.X; }
    }
    public static int ResolutionHeight
    {
        get { return GameGlobals.InGameResolution.Y; }
    }
    public static int virtualHeight;
    public static int virtualWidth;
    private static Vector2 _minPos, _maxPos;

    public static bool TopScreenClamp;


    static Camera()
    {
    }

    public static void SetViewport(Viewport viewport)
    {
        _viewport = viewport;
    }

    public static void HandleScreenResize(GraphicsDevice graphicsDevice)
    {
        float screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
        float screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;

        if (screenWidth / ResolutionWidth > screenHeight / ResolutionHeight)
        {
            float aspect = (float)screenHeight / ResolutionHeight;
            virtualWidth = (int)(aspect * ResolutionWidth);
            virtualHeight = (int)screenHeight;
        }
        else
        {
            float aspect = (float)screenWidth / ResolutionWidth;
            virtualWidth = (int)screenWidth;
            virtualHeight = (int)(aspect * ResolutionHeight);
        }
        _resolutionScalar = virtualWidth / (float)ResolutionWidth;
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

    public static Matrix GetTransformation()
    {
        return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
               Matrix.CreateRotationZ(_rotation) *
               Matrix.CreateScale(_resolutionScalar * _zoom) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
    }

    public static void SetZoom(float newZoom)
    {
        _zoom = newZoom;
    }

    public static void SetBounds(Vector2 mapSize)
    {
        //TODO: Handle zoom type issues HERE, for bounding to work in current state, zoom must be 1!
        float halfWidth = (ResolutionWidth / 2f) / _zoom;
        float halfHeight = (ResolutionHeight / 2f) / _zoom;

        // Calculate the map size (considering the size of the map in world coordinates)
        _minPos = new Vector2(halfWidth - TileManager.BASE_TILESIZE / 2, halfHeight - TileManager.BASE_TILESIZE / 2);

        // Max position calculation (the camera can go as far as the end of the map minus half of its size)
        _maxPos = new Vector2((mapSize.X + 1) * TileManager.BASE_TILESIZE - halfWidth - TileManager.BASE_TILESIZE / 2,
                              (mapSize.Y + 1) * TileManager.BASE_TILESIZE - halfHeight - TileManager.BASE_TILESIZE / 2);
    }

    public static void SetPosition(Vector2 position)
    {
        Position = position;

        // Check if HUD needs to move to bottom
        TopScreenClamp = _minPos.Y > position.Y ? true : false;

        // Clamp the camera position to the valid range considering the map boundaries
        Position = Vector2.Clamp(Position, _minPos, _maxPos);
    }
}
