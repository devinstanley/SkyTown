using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Logic.GameStates;
using SkyTown.LogicManagers;
using SkyTown.Logic;

namespace SkyTown
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IGameState GameState;

        public ResourceManager resourceManager;

        private int _resolutionWidth = 1920;
        private int _resolutionHeight = 1080;
        private RenderTarget2D _renderTarget;

        public Camera ViewCamera;


        private InputManager InputManager = new InputManager();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = _resolutionWidth;
            _graphics.PreferredBackBufferHeight = _resolutionHeight;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            resourceManager = new ResourceManager(Content);
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            //ViewCamera.HandleScreenResize(GraphicsDevice);
        }

        protected override void Initialize()
        {
            ViewCamera = new Camera(GraphicsDevice.Viewport, _resolutionWidth, _resolutionWidth);
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
            _renderTarget = new RenderTarget2D(GraphicsDevice, _resolutionWidth, _resolutionHeight);
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
            GraphicsDevice.Clear(Color.Black);
            //GraphicsDevice.SetRenderTarget(_renderTarget);
            _spriteBatch.Begin(transformMatrix: GameState.ViewCamera.GetTransformation());
            GameState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            

            base.Draw(gameTime);
        }
    }
}
