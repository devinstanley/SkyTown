using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Base;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.Items
{
    public class Item: Entity
    {
        public Texture2D Texture;
        public String Name { get; set; }
        public String ToolTip { get; set; }
        public ItemType Type { get; set; }
        public int MaxInventoryStack { get; set; }
        public Item(ResourceManager resourceManager, int maxStack=1) : base(resourceManager)
        {
            MaxInventoryStack = maxStack;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Assets\\Sprites\\TestItem");
            animationManager.AddAnimation(0, new Animation(Texture, 1, 1, 1));
            
        }
        public void Update(GameTime gameTime)
        {
            animationManager.Update(0, gameTime);
            Width = animationManager.AnimationWidth;
            Height = animationManager.AnimationHeight;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
            Position = pos;
            animationManager.Draw(spriteBatch, pos, scale);
        }
    }

    public enum ItemType{
        None,
        RawMaterial,
        Tool,
        Consumable
    }
}
