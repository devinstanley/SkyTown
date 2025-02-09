using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System.Collections.Generic;

namespace SkyTown.Map
{
    public static class TileManager
    {
        public static readonly int BASE_TILESIZE = 32;
        public static Dictionary<string, BaseTile> TileManifest = new();

        static TileManager()
        {
            TileManifest = ResourceManager.LoadTiles($"Assets\\Tilesets\\MapSheetJSON");
        }

        public static void Update()
        {
            foreach (var tile in TileManifest)
            {
                tile.Value.Update();
            }
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

        public static BaseTile GetTile(string tileID)
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
