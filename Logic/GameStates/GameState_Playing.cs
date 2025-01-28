using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using SkyTown.Map;
using SkyTown.LogicManagers;
using SkyTown.Entities.Characters;
using SkyTown.HUD.Inventory;

namespace SkyTown.Logic.GameStates
{
    internal class GameState_Playing : IGameState
    {
        private Game1 game;
        GameState mySubstate = GameState.Playing;
        private MapScene CurrentScene;
        private Player Player1;
        private NPCManager NPCs = new NPCManager();
        private GameState_Paused pauseMenu;
        private HotbarHUD Hotbar;
        public Camera ViewCamera { get; set; }

        public GameState_Playing(Game1 game)
        {
            this.game = game;
            CurrentScene = new MapScene("TestMap");
            this.ViewCamera = game.ViewCamera;
        }

        public void Initialize()
        {
            Player1 = new Player("Assets.Sprites.spriteAnimationTest");
            Hotbar = new HotbarHUD(game, Player1);
            pauseMenu = new GameState_Paused(game, Player1);
            CurrentScene.Initialize(Player1);
        }

        public void LoadContent(ContentManager content)
        {
            Player1.LoadContent(content);
            pauseMenu.LoadContent(content);
            CurrentScene.LoadContent(content);
            ViewCamera.SetBounds(CurrentScene.MapDimension, CurrentScene.tileDims, Player1);
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (inputManager.IsNewKeyPressed(Keys.Escape))
            {
                if (mySubstate == GameState.Playing)
                {
                    mySubstate = GameState.Paused;
                }
                else
                {
                    mySubstate = GameState.Playing;
                }
                
            }
            
            switch (mySubstate)
            {
                case GameState.Playing:
                    CurrentScene.Update(gameTime, inputManager, ViewCamera);
                    Hotbar.Update(gameTime, inputManager);
                    break;
                case GameState.Paused:
                    pauseMenu.Update(gameTime, inputManager);
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(gameTime, spriteBatch);
            if (mySubstate == GameState.Paused)
            {
                pauseMenu.Draw(gameTime, spriteBatch);
            }
            else
            {
                Hotbar.Draw(gameTime, spriteBatch);
            }
        }

        public void ChangeScene(MapScene newScene)
        {
            CurrentScene = newScene;
            ViewCamera.SetBounds(CurrentScene.MapDimension, CurrentScene.tileDims, Player1);
        }
    }
}
