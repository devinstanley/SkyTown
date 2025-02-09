using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Characters;
using SkyTown.HUD.Inventory;
using SkyTown.LogicManagers;
using SkyTown.Map;

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
            Player1 = new Player("Assets.Sprites.spriteAnimationTest::PlayerSprite");
            Player1.Position = new Vector2(64, 64);
            Hotbar = new HotbarHUD(game, Player1);
            pauseMenu = new GameState_Paused(game, Player1);
            CurrentScene.Initialize(Player1);
        }

        public void LoadContent(ContentManager content)
        {
            Player1.LoadContent(content);
            pauseMenu.LoadContent(content);
            CurrentScene.LoadContent(content);
            ViewCamera.SetBounds(CurrentScene.MapDimension);
        }

        public void Update(InputManager inputManager)
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
                    CurrentScene.Update(inputManager, ViewCamera);
                    Hotbar.Update(inputManager);
                    break;
                case GameState.Paused:
                    pauseMenu.Update(inputManager);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);
            if (mySubstate == GameState.Paused)
            {
                pauseMenu.Draw(spriteBatch);
            }
            else
            {
                Hotbar.Draw(spriteBatch);
            }
        }

        public void ChangeScene(MapScene newScene)
        {
            CurrentScene = newScene;
            ViewCamera.SetBounds(CurrentScene.MapDimension);
        }
    }
}
