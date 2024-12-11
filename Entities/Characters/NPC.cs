using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkyTown.Logic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Base;

namespace SkyTown.Entities.Characters
{
    public class NPC: Entity
    {
        public Vector2 vel = new();
        protected readonly int _speed = 200;

        public readonly string Name;

        public NPC(string ID) : base(ID)
        {
            Name = "";
        }

        public void LoadContent(ContentManager content)
        {
            animationManager.AddAnimation(NPCState.IdleForward, new Animation(ID, 2, 1, 1));
        }

        public void Update(GameTime gameTime)
        {
            animationManager.Update(AnimationState, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Draw(spriteBatch, Position);
        }
    }
    public enum NPCState
    {
        IdleForward,
        IdleBackward,
        IdleLeft,
        IdleRight,

        WalkForward,
        WalkBackward,
        WalkLeft,
        WalkRight,

        RunForward,
        RunBackward,
        RunLeft,
        RunRight,
    }
}
