using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
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
        public void Update(GameTime gameTime, InputManager inputManager);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
