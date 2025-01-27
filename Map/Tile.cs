using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Map
{
    internal class Tile
    {
        public string AtlasID;
        public string TextureID;
        private Texture2D TileAtlas { 
            get
            {
                return ResourceManager.LoadTexture(AtlasID);
            } 
        }
        CollisionActions CollisionAction { get; set; }
        CollisionShapes CollisionShapes { get; set; }

        public Tile(string ID)
        {
            AtlasID = ID.Split('.')[0];
            TextureID = ID.Split(".")[1];
        }
    }

    public enum CollisionActions
    {
        None = 0,
        Block = 1,
        Transport = 2,
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
