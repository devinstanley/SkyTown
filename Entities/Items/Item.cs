using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
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
            base.LoadContent(content);
        }
        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
            base.Draw(spriteBatch, pos, scale);
        }
    }

    public enum ItemType{
        None,
        RawMaterial,
        Tool,
        Consumable
    }
}
