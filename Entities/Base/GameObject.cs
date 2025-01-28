using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyTown.Entities.Base
{
    public abstract class GameObject
    {
        public string ID { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle? CollisionRectangle { get; set; }
        public IAnimationProvider AnimationHandler { get; set; }

        public GameObject(string id)
        {
            ID = id;
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimationHandler?.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            AnimationHandler?.Draw(spriteBatch, Position);
        }
    }
}
