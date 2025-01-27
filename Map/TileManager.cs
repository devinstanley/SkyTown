using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Map
{
    public static class TileManager
    {
        static Dictionary<string, Tile> Tiles;
        static Dictionary<string, Atlas> AtlasManifest;

        static TileManager()
        {

        }

        static Atlas GetAtlas(string AtlasID)
        {
            if (AtlasManifest.ContainsKey(AtlasID))
            {
                return AtlasManifest[AtlasID];
            }
            else
            {

            }
        }

        static void LoadAtlas(string AtlasID)
        {

        }

        static Tile GetTile(string tileID)
        {
            if (Tiles.ContainsKey(tileID))
            {
                return Tiles[tileID];
            }
            else if 
            {

            }
        }
    }

    public class Atlas
    {
        string AtlasID;
        Dictionary<string, Tuple<Vector2, Vector2>> IDMap;
    }
}
