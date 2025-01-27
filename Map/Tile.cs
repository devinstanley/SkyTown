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
        private Texture2D tileAtlas { 
            get
            {
                return ResourceManager.LoadTexture(AtlasID);
            } 
        }
        public bool IsWalkable = true;
        public Tile(string atlas)
        {
            AtlasID = atlas;
        }
    }
}
