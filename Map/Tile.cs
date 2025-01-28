using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyTown.Map
{
    public class Tile
    {
        public string AtlasID;
        public string TextureID;
        public Rectangle TextureSource;
        public Rectangle? CollisionRectangle;

        public Tile(string ID, Rectangle textureSource, Rectangle? collisionRectangle = null)
        {
            AtlasID = ID.Split('.')[0];
            TextureID = ID.Split(".")[1];
            TextureSource = textureSource;
            CollisionRectangle = collisionRectangle;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileMap, string tileID, Vector2 position)
        {
            spriteBatch.Draw(
                tileMap,
                position,
                TextureSource,
                Color.White,
                0f,
                new Vector2(TextureSource.Width / 2, TextureSource.Height / 2),
                1,
                SpriteEffects.None,
                0
            );
        }
    }
}
