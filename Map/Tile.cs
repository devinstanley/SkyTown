using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Base;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Map
{
    public class Tile
    {
        public string AtlasID;
        public string TextureID;
        public Rectangle TextureSource;
        CollisionShapes CollisionShape { get; set; }

        public Tile(string ID, Rectangle textureSource, CollisionShapes collisionShape)
        {
            AtlasID = ID.Split('.')[0];
            TextureID = ID.Split(".")[1];
            TextureSource = textureSource;
            CollisionShape = collisionShape;
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


    //Figure out way to handle animations here
    public class AnimatedTile: Tile
    {
        Animation TileAnimation { get; set; }
        public AnimatedTile(
            string ID, 
            List<Rectangle> SourceRectangles,  
            CollisionShapes collisionShape
        ) : base(ID, SourceRectangles[0], collisionShape)
        {
        }
    }

    public enum CollisionShapes
    {
        None = 0,
        Entire = 1,
        TopHalf = 2,
        BottomHalf = 3,
        LeftHalf = 4,
        RightHalf = 5,
        TopRightQuad = 6,
        BottomRightQuad = 7,
        TopLeftQuad = 8,
        BottomLeftQuad = 9,
    }
}
