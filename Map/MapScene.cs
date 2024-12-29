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
using SkyTown.Entities.Base;
using SkyTown.Entities.Items;
using SkyTown.Logic;

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
        public int tileDims = 32;
        public Vector2 MapDimension;
        public float scale = 1;
        public string SceneID;
        Texture2D TextureAtlas;
        Texture2D collisionTextures;
        List<Dictionary<Vector2, int>> tileMaps = new();
        Dictionary<int, Rectangle> tileSource;

        //Handles Majority of Collision and Interaction
        private CollisionManager collisionManager = new();
        private InteractionManager interactionManager = new();
        public List<Item> SceneItems = new(); //Attainable Items
        public List<Entity> SceneEntities = new(); //Non-map collideables and interactables
        public Dictionary<Vector2, int> collisionMap = new();

        //For Holding Player and NPCs
        private Player player;
        public NPCManager npcManager;

        


        public MapScene(string ID)
        {
            SceneID = ID;

            string testItemID = "Assets.Sprites.TestItem";
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));
            SceneItems.Add(new Item(testItemID));

            string testEntityID = "Assets.Sprites.TestItem";
            SceneEntities.Add(new Entity(testEntityID));
            SceneEntities.Add(new Entity(testEntityID));
        }
        public void Initialize(Player player)
        {
            this.player = player;
            this.npcManager = new NPCManager();
            npcManager.Add(new NPC("Assets.Sprites.NPCs.Blurg"));
        }
        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>($"Assets\\Maps\\MapSheet");
            collisionTextures = content.Load<Texture2D>($"Assets\\Maps\\Collisions");
            collisionMap = content.Load<Dictionary<Vector2, int>>($"Assets\\Maps\\{SceneID}Collisions").Where(u => u.Value != -1 && u.Value != 2).ToDictionary();
            GenerateSources();
            int layer = 0;
            while (true)
            {
                try
                {
                    tileMaps.Add(content.Load<Dictionary<Vector2, int>>($"Assets\\Maps\\{SceneID}TileMapLayer{layer}"));
                    layer++;
                }
                catch
                {
                    break;
                }
            }
            int numRows = (int)tileMaps.Last().Select(u => u.Key.Y).Max();
            int numCols = (int)tileMaps.Last().Select(u => u.Key.X).Max();
            MapDimension = new(numRows, numCols);
            player.SetBounds(new Point(numCols, numRows), new Point(tileDims, tileDims));

            Random t = new Random();
            foreach (Item item in SceneItems)
            {
                item.LoadContent(content);
                item.Position = new Vector2(150 + t.NextInt64(-50, 50), 150 + t.NextInt64(-50, 50));
            }

            foreach (Entity entity in SceneEntities)
            {
                entity.LoadContent(content);
                entity.Position = new Vector2(300 + t.NextInt64(-150, 150), 200 + t.NextInt64(-150, 150));
            }
            npcManager.LoadContent(content);
            npcManager.NPCs.Last().Position = new Vector2(150, 150);
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
            foreach (Item e in SceneItems)
            {
                e.Update(gameTime);
            }
            foreach (Entity e in SceneEntities)
            {
                e.Update(gameTime);
            }
            player.Update(gameTime, inputManager, collisionManager);
            npcManager.Update(gameTime);
            collisionManager.Update(gameTime, this, player);
            interactionManager.Update(gameTime, inputManager, this, player);
            ViewCamera.SetPosition(player.Position);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
            }

            foreach (var e in SceneItems)
            {
                e.Draw(spriteBatch, e.Position, 0.5f);
            }
            foreach (var e in SceneEntities)
            {
                e.Draw(spriteBatch, e.Position);
            }

            npcManager.Draw(gameTime, spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
