using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
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
        public Vector2 MapDimension;

        public List<Dictionary<Vector2, string>> TileMapLayers = new();
        public List<GameObject> SceneObjects = new(); //Attainable Items

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
            MapDimension = new(numCols, numRows);
            Player1.SetBounds(MapDimension);
            SceneObjects.Add(ItemManager.GetItem("berries"));
            SceneObjects.Add(ItemManager.GetItem("pickaxe"));

            //Generates Dispenseable Object
            var dispense_test = GameObjectManager.GetItem("berrybush");
            dispense_test.Position = new Vector2(120, 120);
            SceneObjects.Add(dispense_test);

            var dispense_test1 = GameObjectManager.GetItem("berrybush");
            dispense_test1.Position = new Vector2(170, 120);
            SceneObjects.Add(dispense_test1);

            var dispense_test2 = GameObjectManager.GetItem("berrybush");
            dispense_test2.Position = new Vector2(16, 120);
            SceneObjects.Add(dispense_test2);

            var dispense_test3 = GameObjectManager.GetItem("berrybush");
            dispense_test3.Position = new Vector2(75, 120);
            SceneObjects.Add(dispense_test3);

            var harvest_test1 = GameObjectManager.GetItem("rock");
            harvest_test1.Position = new Vector2(150, 160);
            SceneObjects.Add(harvest_test1);
        }

        public void Update(InputManager inputManager)
        {
            TileManager.Update();
            foreach (GameObject sceneObject in SceneObjects.ToList())
            {
                sceneObject.Update();
            }

            Player1.Update(inputManager);
            CollisionManager.Update(this, Player1);
            InteractionManager.Update(inputManager, this, Player1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Map Scene Here Using Tileset
            foreach (Dictionary<Vector2, string> TileMapLayer in TileMapLayers)
            {
                foreach (KeyValuePair<Vector2, string> tileDict in TileMapLayer)
                {
                    TileManager.Draw(spriteBatch, tileDict.Value, tileDict.Key * TileManager.BASE_TILESIZE);
                }
            }

            //Draw All Items/Entities
            foreach (var e in SceneObjects)
            {
                if (e is Item item)
                {
                    item.Draw(spriteBatch, scale:0.5f);
                    continue;
                }
                if (e is GameObject gameObject)
                {
                    gameObject.Draw(spriteBatch);
                    continue;
                }
            }

            //Draw Player
            Player1.Draw(spriteBatch);
        }
    }
}
