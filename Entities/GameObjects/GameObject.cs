using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.GameObjects
{
    public class GameObject
    {
        public string FullID { get; set; }
        public string TextureID
        {
            get { return FullID.Split("::")[0]; }
        }
        public string ObjectID
        {
            get { return FullID.Split("::")[1]; }
        }
        public Vector2 Position { get; set; }
        public Rectangle? CollisionRectangle { get; set; }
        public IAnimator AnimationHandler { get; set; }

        public int Height
        {
            get { return AnimationHandler.Height; }
        }
        public int Width
        {
            get { return AnimationHandler.Width; }
        }

        public GameObject(string id, Rectangle? collisionRect = null)
        {
            FullID = id;
            CollisionRectangle = collisionRect;
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update()
        {
            AnimationHandler?.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2? position = null, float scale = -1)
        {
            if (position.HasValue)
            {
                AnimationHandler?.Draw(spriteBatch, position.Value, scale);
            }
            else
            {
                AnimationHandler?.Draw(spriteBatch, Position, scale);
            }
        }
    }
}
