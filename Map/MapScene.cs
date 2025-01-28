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
        public string MapID;

        //For Drawing on Screen
        public int tileDims = 32;
        public Vector2 MapDimension;
        
        public List<Dictionary<Vector2, string>> TileMapLayers = new();

        //Handles Majority of Collision and Interaction
        private CollisionManager collisionManager = new();
        private InteractionManager interactionManager = new();
        public List<Item> SceneItems = new(); //Attainable Items
        public List<Entity> SceneEntities = new(); //Non-map collideables and interactables

        //For Holding Player and NPCs
        private Player Player1;
        public NPCManager NpcManager;

       
        public MapScene(string ID)
        {
            MapID = ID;
        }
        public void Initialize(Player player)
        {
            this.Player1 = player;
            this.NpcManager = new NPCManager();
            NpcManager.Add(new NPC("Assets.Sprites.NPCs.Blurg"));
        }
        public void LoadContent(ContentManager content)
        {
            int layer = 0;
            while (true)
            {
                try
                {
                    TileMapLayers.Add(content.Load<Dictionary<Vector2, string>>($"Assets\\Maps\\{MapID}\\Layer{layer}"));
                    layer++;
                }
                catch
                {
                    break;
                }
            }
            int numRows = (int)TileMapLayers.Last().Select(u => u.Key.Y).Max();
            int numCols = (int)TileMapLayers.Last().Select(u => u.Key.X).Max();
            MapDimension = new(numRows, numCols);
            NpcManager.LoadContent(content);
            NpcManager.NPCs.Last().Position = new Vector2(80, 80);
            Player1.SetBounds(new Point(numCols, numRows), new Point(tileDims, tileDims));
        }

        public void Update(GameTime gameTime, InputManager inputManager, Camera ViewCamera)
        {
            
            foreach (Item item in SceneItems)
            {
                item.Update(gameTime);
            }
            foreach (Entity entity in SceneEntities)
            {
                entity.Update(gameTime);
            }
            Player1.Update(gameTime, inputManager, collisionManager);
            NpcManager.Update(gameTime);
            collisionManager.Update(gameTime, this, Player1);
            interactionManager.Update(gameTime, inputManager, this, Player1);
            ViewCamera.SetPosition(Player1.Position);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw Map Scene Here Using Tileset
            foreach (Dictionary<Vector2, string> TileMapLayer in TileMapLayers)
            {
                foreach (KeyValuePair<Vector2, string> tileDict in TileMapLayer)
                {
                    TileManager.Draw(spriteBatch, tileDict.Value, tileDict.Key * tileDims);
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

            NpcManager.Draw(gameTime, spriteBatch);
            Player1.Draw(spriteBatch);
        }
    }
}
