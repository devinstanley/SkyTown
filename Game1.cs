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

        private int _resolutionWidth = 240;
        private int _resolutionHeight = 120;

        public Camera ViewCamera;


        private InputManager InputManager = new InputManager();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = _resolutionWidth * 8;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = _resolutionHeight * 8;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            ResourceManager.content = Content;
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            ViewCamera.HandleScreenResize(GraphicsDevice);
        }

        protected override void Initialize()
        {
            ViewCamera = new Camera(GraphicsDevice.Viewport, _resolutionWidth, _resolutionHeight);
            GameState = new GameState_MainMenu(this);
            ViewCamera.HandleScreenResize(GraphicsDevice);
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
            InputManager.Update(ViewCamera);
            GameState.Update(gameTime, InputManager);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Viewport = ViewCamera._viewport;
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: GameState.ViewCamera.GetTransformation());
            GameState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);



            base.Draw(gameTime);
        }
    }
}
