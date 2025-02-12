using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
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
            _graphics.PreferredBackBufferWidth = GameGlobals.InGameResolution.X * 4;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GameGlobals.InGameResolution.Y * 4;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            ResourceManager.content = Content;
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            Camera.HandleScreenResize(GraphicsDevice);
        }

        protected override void Initialize()
        {
            Camera.SetViewport(GraphicsDevice.Viewport);
            GameState = new GameState_MainMenu(this);
            Camera.HandleScreenResize(GraphicsDevice);
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
            GameGlobals.Update(gameTime);
            InputManager.Update();
            GameState.Update(InputManager);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Viewport = Camera._viewport;
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetTransformation());
            GameState.Draw(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);



            base.Draw(gameTime);
        }
    }
}
