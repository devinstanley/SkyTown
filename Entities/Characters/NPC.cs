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
using SkyTown.Entities.Items;

namespace SkyTown.Entities.Characters
{
    public class NPC: Entity
    {
        public Vector2 vel = new();
        protected readonly int _speed = 200;

        public readonly string Name;

        public double IsTalking = 0;
        public string DefaultDialog = "I am Lorde, ya ya ya";
        public string CurrentDialog = "";

        public NPC(string ID) : base(ID)
        {
            Name = "";
            CurrentDialog = DefaultDialog;
        }

        new public void LoadContent(ContentManager content)
        {
            animationManager.AddAnimation(NPCState.IdleForward, new Animation(ID, 2, 1, 1));
        }

        new public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsTalking > 0)
            {
                IsTalking -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                IsTalking = 0;
            }
        }

        public void Talk(Item gift = null)
        {
            if (true)
            {
                IsTalking = 5;
                if (gift == null)
                {
                    
                    CurrentDialog = DefaultDialog;
                }
                else
                {
                    CurrentDialog = "Thanks for the gift! \n\r And thank you Beyonce!!";
                }
                
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsTalking > 0)
            {
                spriteBatch.DrawString(
                    ResourceManager.LoadFont("Assets.Font.Arial"),
                    CurrentDialog,
                    Position,
                    Color.White
                    );
            }
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
