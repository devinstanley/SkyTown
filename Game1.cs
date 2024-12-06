using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Logic.GameStates;
using SkyTown.LogicManagers;

namespace SkyTown
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IGameState GameState;

        private InputManager InputManager = new InputManager();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            GameState = new GameState_MainMenu(this);
            GameState.Initialize();
            base.Initialize();
        }

        public void ChangeGameState(IGameState newState)
        {
            GameState = newState;
            GameState.Initialize();
            GameState.LoadContent(Content);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameState.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(GameState.ViewCamera.GetTransformation());
            GameState.Update(gameTime, InputManager);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(transformMatrix: GameState.ViewCamera.GetTransformation());
            GameState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
