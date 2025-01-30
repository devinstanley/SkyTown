using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System.Collections.Generic;

namespace SkyTown.Map
{
    public static class TileManager
    {
        public static readonly int BASE_TILESIZE = 32;
        public static Dictionary<string, Tile> TileManifest = new();

        static TileManager()
        {
            TileManifest = ResourceManager.LoadTiles($"Assets\\Tilesets\\MapSheetTileManifest");
        }

        public static void Draw(SpriteBatch spriteBatch, string tileID, Vector2 position)
        {
            if (!TileManifest.ContainsKey(tileID))
            {
                //Load tile into manifest, but it already should -> for now do nothing, eventually manage
                return;
            }
            TileManifest[tileID].Draw(spriteBatch, position);
        }

        public static Tile GetTile(string tileID)
        {
            if (!TileManifest.ContainsKey(tileID))
            {
                //Load tile into manifest, but it already should -> for now do nothing, eventually manage
                return null;
            }
            return TileManifest[tileID];
        }
    }
}
