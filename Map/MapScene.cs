using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Items;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System.Collections.Generic;
using System.Linq;

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
        public List<HarvestableObject> SceneHarvestables = new(); //Non-map collideables and interactables

        //Holds Player
        private Player Player1;


        public MapScene(string ID)
        {
            MapID = ID;
        }
        public void Initialize(Player player)
        {
            this.Player1 = player;
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
            Player1.SetBounds(new Point(numCols, numRows), new Point(tileDims, tileDims));
        }

        public void Update(GameTime gameTime, InputManager inputManager, Camera ViewCamera)
        {

            foreach (Item item in SceneItems)
            {
                item.Update(gameTime);
            }
            foreach (HarvestableObject entity in SceneHarvestables)
            {
                entity.Update(gameTime);
            }

            Player1.Update(gameTime, inputManager);
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

            //Draw All Items/Entities
            foreach (var e in SceneItems)
            {
                e.Draw(spriteBatch, e.Position, 0.5f);
            }
            foreach (var e in SceneHarvestables)
            {
                e.Draw(spriteBatch, e.Position);
            }

            //Draw Player
            Player1.Draw(spriteBatch);
        }
    }
}
