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
        public String Name { get; set; }
        public String ToolTip { get; set; }
        public ItemType Type { get; set; }
        public int MaxInventoryStack { get; set; }
        public Item(string ID, int maxStack=3) : base(ID)
        {
            MaxInventoryStack = maxStack;
        }

        public void LoadContent(ContentManager content)
        {
            animationManager.AddAnimation(0, new Animation(ID, 1, 1, 1));
            
        }
        public void Update(GameTime gameTime)
        {
            animationManager.Update(0, gameTime);
            Width = animationManager.AnimationWidth;
            Height = animationManager.AnimationHeight;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
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
