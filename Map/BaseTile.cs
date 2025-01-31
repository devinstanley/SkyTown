using Microsoft.Xna.Framework;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Map
{
    public class BaseTile : GameObject
    {
        public BaseTile(string ID, IAnimator animation, Rectangle? collisionRectangle = null) : base(ID)
        {
            CollisionRectangle = collisionRectangle;
            var animationSequence = animation;
            AnimationHandler = animationSequence;
        }
    }
}
