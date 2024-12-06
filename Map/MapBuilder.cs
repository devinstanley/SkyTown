using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkyTown.Map
{
    public class MapBuilder
    {
        Texture2D MapBaseTiles;
        Texture2D MapSmallFoliageTiles;
        Texture2D MapLargeFoliageTiles;

        string[][] testMap =
        {
            new string[] {"Map.Grass1", "", "Map.Grass1","Map.Grass1","Map.Grass1","Map.Grass1"},
            new string[] {"Map.Grass1", "Map.Grass2", "Map.Grass1","Map.Grass1","Map.Grass1","Map.Grass1"},
            new string[] {"Map.Grass1", "Map.Grass2", "Map.Grass1","Map.Grass1","Map.Grass1","Map.Grass1"},
            new string[] {"Map.Grass1", "Map.Grass1", "Map.Grass1","Map.Grass3","Map.Grass3","Map.Grass1"},
            new string[] {"Map.Grass1", "Map.Grass4", "Map.Grass1","Map.Grass3","Map.Grass3","Map.Grass1"},
            new string[] {"Map.Grass1", "Map.Grass1", "Map.Grass1","Map.Grass1","Map.Grass1","Map.Grass1"},
        };

        ContentManager content;

        Dictionary<string, MapTile> MapTiles = new Dictionary<string, MapTile>();


        public MapBuilder(ContentManager gameContent)
        {
            content = gameContent;
        }

        public void LoadContent()
        {
            MapBaseTiles = content.Load<Texture2D>("Assets\\Map\\GrassTiles");
            RegisterTiles();
        }

        public void RegisterTiles()
        {
            MapTiles["Map.Grass1"] = new MapTile(new Point(0, 0), new Point(16, 16));
            MapTiles["Map.Grass2"] = new MapTile(new Point(16, 0), new Point(16, 16));
            MapTiles["Map.Grass3"] = new MapTile(new Point(0, 16), new Point(16, 16));
            MapTiles["Map.Grass4"] = new MapTile(new Point(0, 128), new Point(16, 16));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 2;
            float curY = 0;
            foreach (var row in testMap)
            {
                float curX = 0;
                foreach (string key in row)
                {
                    try
                    {
                        spriteBatch.Draw(
                            MapBaseTiles, 
                            new Vector2(curX, curY), 
                            MapTiles[key].sourceRectangle, 
                            Color.White, 0f, Vector2.Zero, 
                            scale, 
                            SpriteEffects.None, 
                            0f
                        );
                    }
                    catch { }
                    curX += 16 * scale;
                }
                curY += 16 * scale;
            }
        }
    }
}

