using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.Base
{
    public abstract class Entity
    {
        public int ID { get; set; }
        public Vector2 Position;
        public Rectangle Hitbox => new Rectangle((int)Position.X - Width/2, (int)Position.Y - Height/2, Width, Height); // Collision box
        public Rectangle SourceRectangle { get; set; }  // Sprite sheet region
        public int Width { get; set; }
        public int Height { get; set; }
        protected readonly AnimationManager animationManager = new();
        protected NPCState AnimationState = NPCState.IdleForward;
        public bool IsCollidable { get; set; }  // Can be walked through or not
    }
}
