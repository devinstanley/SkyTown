using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SkyTown.LogicManagers;

namespace SkyTown.Logic.GameStates
{
    internal class GameState_MainMenu : IGameState
    {
        private Game1 game;
        const GameState myStateEnum = GameState.MainMenu;
        public Camera ViewCamera { get; set; }

        public GameState_MainMenu(Game1 game)
        {
            this.game = game;
            ViewCamera = game.ViewCamera;
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(ResourceManager.LoadSong("Assets.Sounds.SkytownMainTheme"));
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (inputManager.IsNewKeyPressed(Keys.Enter))
            {
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Play(ResourceManager.LoadSong("Assets.Sounds.StartEffect"));
                StartGame();
                
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                ResourceManager.LoadTexture("Assets.Menus.StartMenu"),
                Vector2.Zero,
                null,
                Color.White, 0f, new Vector2(ResourceManager.LoadTexture("Assets.Menus.StartMenu").Width / 2, ResourceManager.LoadTexture("Assets.Menus.StartMenu").Height / 2),
                1,
                SpriteEffects.None,
                0f
            );
        }

        public void StartGame()
        {
            game.ChangeGameState(new GameState_Playing(game));
            game.ViewCamera.SetZoom(0.5f);
        }
    }
}
