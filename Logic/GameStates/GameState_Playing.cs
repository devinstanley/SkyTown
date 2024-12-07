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

namespace SkyTown.Logic.GameStates
{
    internal class GameState_Playing : IGameState
    {
        private Game1 game;
        const GameState myStateEnum = GameState.Playing;
        GameState mySubstate = GameState.Playing;
        private MapScene CurrentScene;
        private Player player;
        private NPCManager NPCs = new NPCManager();
        private GameState_Paused pauseMenu;
        public Camera ViewCamera { get; set; }

        public GameState_Playing(Game1 game)
        {
            this.game = game;
            CurrentScene = new MapScene("DevelopmentMap");
            this.ViewCamera = game.ViewCamera;
        }

        public void Initialize()
        {
            player = new Player("PlayerCollision", game.resourceManager);
            pauseMenu = new GameState_Paused(game, player);
            CurrentScene.Initialize(player);
        }

        public void LoadContent(ContentManager content)
        {
            player.LoadContent(content);
            pauseMenu.LoadContent(content);
            CurrentScene.LoadContent(content);
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
        }

        public void ChangeScene(MapScene newScene)
        {

        }

        public void CheckPause()
        {

        }
    }
}
