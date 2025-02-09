using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using SkyTown.Map;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Item : GameObject, IInteractor
    {
        public int MaxStack;
        private Animation InventoryAnimation;
        public Item(string id, int maxStack, IAnimator animation, Animation inventoryAnimation) : base(id)
        {
            MaxStack = maxStack;
            AnimationHandler = animation;
            InventoryAnimation = inventoryAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            InventoryAnimation.Update(gameTime);
            base.Update(gameTime);
        }

        public void Interact(Player player, MapScene mapScene)
        {

        }

        public void DrawInInventory(SpriteBatch spriteBatch, Vector2 position, float scale = -1)
        {
            InventoryAnimation.Draw(spriteBatch, position, scale);
        }
    }
}
