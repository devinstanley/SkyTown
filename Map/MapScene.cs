using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SkyTown.LogicManagers;
using SkyTown.Entities.Characters;

namespace SkyTown.Map
{
    public enum MapScenes
    {
        MainMenu,
        Playing,
        Paused
    }

    public class MapScene
    {
        //For Drawing on Screen
        private int tileDims = 32;
        public float scale = 1;
        public string SceneID;
        Texture2D TextureAtlas;
        Texture2D collisionTextures;
        List<Dictionary<Vector2, int>> tileMaps = new();
        Dictionary<int, Rectangle> tileSource;
        Dictionary<Vector2, int> collisionMap = new();
        Dictionary<int, Rectangle> collisionSource;
        //For Holding Player and NPC
        private Player player;
        private NPCManager npcManager;
        private bool DEBUG_COLLISIONS = true;
        private CollisionManager collisionManager;


        public MapScene(string ID)
        {
            SceneID = ID;
        }
        public void Initialize(Player player)
        {
            this.player = player;
            this.npcManager = new NPCManager();
        }
        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>($"Assets\\Map\\{SceneID}TileAtlas");
            collisionTextures = content.Load<Texture2D>($"Assets\\Map\\Collisions");
            collisionMap = content.Load<Dictionary<Vector2, int>>($"Assets\\Map\\{SceneID}Collisions").Where(u => u.Value != -1 && u.Value != 2).ToDictionary();
            GenerateSources();
            GenerateCollisionSources();
            int layer = 0;
            while (true)
            {
                try
                {
                    tileMaps.Add(content.Load<Dictionary<Vector2, int>>($"Assets\\Map\\{SceneID}TileMapLayer{layer}"));
                    layer++;
                }
                catch
                {
                    break;
                }
            }
            int numRows = (int)tileMaps.Last().Select(u => u.Key.Y).Max();
            int numCols = (int)tileMaps.Last().Select(u => u.Key.X).Max();
            player.SetBounds(new Point(numCols, numRows), new Point(tileDims, tileDims));

            this.collisionManager = new CollisionManager(player, npcManager, collisionMap, tileDims);
        }

        public void LoadTestMap()
        {
            Dictionary<Vector2, int> map = new Dictionary<Vector2, int>();
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    map.Add(new Vector2(i, j), random.Next(0, 20));
                }
            }

            tileMaps.Add(map);
        }

        public void GenerateCollisionSources()
        {
            int numRows = collisionTextures.Height / tileDims;  // Rows are based on height
            int numCols = collisionTextures.Width / tileDims;   // Columns are based on width

            collisionSource = new();

            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    int index = y + (x * numCols);  // Flatten the 2D index to 1D
                    collisionSource[index] = new Rectangle(y * tileDims, x * tileDims, tileDims, tileDims);
                }
            }
        }

        public void GenerateSources()
        {
            int numRows = TextureAtlas.Height / tileDims;  // Rows are based on height
            int numCols = TextureAtlas.Width / tileDims;   // Columns are based on width

            tileSource = new();

            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    int index = y + (x * numCols);  // Flatten the 2D index to 1D
                    tileSource[index] = new Rectangle(y * tileDims, x * tileDims, tileDims, tileDims);
                }
            }
        }

        public void Update(GameTime gameTime, InputManager inputManager, Camera ViewCamera)
        {
            player.Update(gameTime, inputManager, collisionManager);
            ViewCamera.SetPosition(player.Position);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int order = 0;
            foreach (var tileMap in tileMaps)
            {
                foreach (var item in tileMap)
                {
                    if (item.Value < 0)
                    {
                        continue;
                    }
                    Vector2 dest = new Vector2((int)item.Key.X * tileDims, (int)item.Key.Y * tileDims);

                    Rectangle source = tileSource[item.Value];

                    spriteBatch.Draw(TextureAtlas, dest, source, Color.White, 0f, new Vector2(tileDims / 2, tileDims / 2), 1, SpriteEffects.None, 0);
                }
                order += 1;
            }

            if (DEBUG_COLLISIONS)
            {
                foreach (var item in collisionMap)
                {
                    if (item.Value < 0)
                    {
                        continue;
                    }

                    Vector2 dest = new Vector2((int)item.Key.X * tileDims, (int)item.Key.Y * tileDims);

                    Rectangle source = tileSource[item.Value];

                    spriteBatch.Draw(collisionTextures, dest, source, Color.White, 0f, new Vector2(tileDims / 2, tileDims / 2), 1, SpriteEffects.None, 0);
                }
            }
            

            npcManager.Draw(gameTime, spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
