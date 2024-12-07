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
        private Texture2D GameMenuBackground;
        private Song titleSong;
        private Song startEffect;
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
            GameMenuBackground = content.Load<Texture2D>($"Assets\\Menu\\Start Menu");
            titleSong = content.Load<Song>($"Assets\\Music\\Skytown Main Theme");
            startEffect = content.Load<Song>($"Assets\\Music\\Start Effect");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play( titleSong );
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (inputManager.IsNewKeyPressed(Keys.Enter))
            {
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Play(startEffect);
                StartGame();
                
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                GameMenuBackground,
                Vector2.Zero,
                null,
                Color.White, 0f, new Vector2(GameMenuBackground.Width / 2, GameMenuBackground.Height / 2),
                4,
                SpriteEffects.None,
                0f
            );
        }

        public void StartGame()
        {
            game.ChangeGameState(new GameState_Playing(game));
        }
    }
}
