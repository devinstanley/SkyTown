using Microsoft.Xna.Framework;
using SkyTown.Entities.Base;
using SkyTown.Entities.Interfaces;
using System.Collections.Generic;

namespace SkyTown.Map
{
    public class Tile : GameObject
    {

        public Tile(string ID, Rectangle textureSource, Rectangle? collisionRectangle = null) : base(ID)
        {
            CollisionRectangle = collisionRectangle;
            var animationSequence = new Animation(TextureID, 1, new List<Rectangle>([textureSource]));
            AnimationHandler = animationSequence;
        }
    }
}
