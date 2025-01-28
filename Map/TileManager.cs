using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Map
{
    public static class TileManager
    {
        public static readonly int BASE_TILESIZE = 32;
        static Dictionary<string, Atlas> AtlasManifest = new();

        static TileManager()
        {
           
        }

        public static void Draw(SpriteBatch spriteBatch, string tileID, Vector2 position)
        {
            //Get Atlas
            string atlasID = tileID.Split('.')[0];
            if (!AtlasManifest.ContainsKey(atlasID))
            {
                LoadAtlas(atlasID);
            }
            AtlasManifest[atlasID].Draw(spriteBatch, tileID.Split('.')[1], position);
        }

        static void LoadAtlas(string atlasID)
        {
            AtlasManifest.Add(atlasID, new Atlas(atlasID));
        }

        static void RemoveAtlas(string atlasID)
        {
            AtlasManifest.Remove(atlasID);
        }

        public static Tile GetTile(string tileID)
        {
            //Get Atlas
            string atlasID = tileID.Split('.')[0];
            tileID = tileID.Split(".")[1];
            if (!AtlasManifest.ContainsKey(atlasID))
            {
                LoadAtlas(atlasID);
            }
            return AtlasManifest[atlasID].Tiles[tileID];
        }
    }

    public class Atlas
    {
        string AtlasID;
        public Texture2D TileMap;
        public Dictionary<string, Tile> Tiles;

        public Atlas(string atlasID)
        {
            AtlasID = atlasID;
            TileMap = ResourceManager.LoadTexture($"Assets\\Tilesets\\{atlasID}");
            Tiles = ResourceManager.LoadTiles($"Assets\\Tilesets\\{atlasID}TileManifest");
        }

        public void Draw(SpriteBatch spriteBatch, string tileID, Vector2 position)
        {
            Tiles[tileID].Draw(spriteBatch, TileMap, tileID, position);
        }
    }
}
