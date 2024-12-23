using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using SkyTown.Logic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SkyTown.Entities.Base
{
    public abstract class Entity
    {
        public string ID { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Hitbox => new Rectangle((int)Position.X - Width/2, (int)Position.Y - Height/2, Width, Height); // Collision box
        public Rectangle SourceRectangle { get; set; }  // Sprite sheet region
        public int Width { get; set; }
        public int Height { get; set; }
        protected readonly AnimationManager animationManager = new();
        protected NPCState AnimationState = NPCState.IdleForward;
        public CollisionActionEnum CollisionAction { get; set; }


        public Entity(string ID)
        {
            this.ID = ID;
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

    public enum CollisionActionEnum{
        PASS,       //Can pass through
        SLIDING,    //Can slide up against
        COLLECT,    //Can be collected
    }
}
