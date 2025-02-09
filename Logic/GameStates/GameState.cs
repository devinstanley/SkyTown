using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.LogicManagers;

namespace SkyTown.Logic.GameStates
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused
    }

    public interface IGameState
    {
        public Camera ViewCamera { get; set; }
        public void Initialize();
        public void LoadContent(ContentManager content);
        public void Update(InputManager inputManager);
        public void Draw(SpriteBatch spriteBatch);
    }
}
