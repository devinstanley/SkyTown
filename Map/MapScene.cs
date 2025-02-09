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

        //Handles Majority of Collision and Interaction
        private CollisionManager collisionManager = new();
        private InteractionManager interactionManager = new();
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
            Player1.SetBounds(new Point(numCols, numRows));
        }

        public void Update(InputManager inputManager, Camera ViewCamera)
        {
            TileManager.Update();
            foreach (Item item in SceneObjects)
            {
                item.Update();
            }

            Player1.Update(inputManager);
            collisionManager.Update(this, Player1);
            interactionManager.Update(inputManager, this, Player1);
            ViewCamera.SetPosition(Player1.Position);
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
                e.Draw(spriteBatch, e.Position, 0.5f);
            }

            //Draw Player
            Player1.Draw(spriteBatch);
        }
    }
}
