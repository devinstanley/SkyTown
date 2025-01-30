using Microsoft.Xna.Framework;
using SkyTown.Entities.Base;
using SkyTown.Entities.Interfaces;
using System.Collections.Generic;

namespace SkyTown.Map
{
    public class BaseTile : GameObject
    {
        public BaseTile(string ID, Animation animation, Rectangle? collisionRectangle = null) : base(ID)
        {
            CollisionRectangle = collisionRectangle;
            var animationSequence = animation;
            AnimationHandler = animationSequence;
        }
    }
}
