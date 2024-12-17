using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyTown.Logic;

namespace SkyTown.HUD.Inventory
{
    internal class HotbarHUD: InventoryHUD
    {
        new Texture2D inventoryTexture { get => ResourceManager.LoadTexture("Assets.HUDs.HotbarHUD"); }
        public HotbarHUD(Player player): base(player)
        {

        }

        new public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                inventoryTexture,
                new Vector2(player.Position.X, inventoryTexture.Height / 2),
                null,
                Color.White,
                0f,
                new Vector2(inventoryTexture.Width / 2, inventoryTexture.Height / 2),
                1f, SpriteEffects.None, 0f);

            foreach (var itemSlot in _inventory.Items)
            {
                int slotX = itemSlot.Key % INVENTORYWIDTH; // Column index
                int slotY = itemSlot.Key / INVENTORYWIDTH; // Row index

                if (slotY != 0)
                {
                    continue;
                }

                Vector2 position = new(
                    player.Position.X + slotX * InventorySlotDimensions + itemSlot.Value.Item.Width / 2 - inventoryTexture.Width / 2,
                    player.Position.Y + slotY * InventorySlotDimensions + itemSlot.Value.Item.Height / 2 - inventoryTexture.Height / 2
                    );

                if (itemSlot.Key == _inventory.CurrentSelectedItem)
                {
                    //Draw selected item highlight
                    spriteBatch.Draw(
                        selectedItemHighlight,
                        position,
                        null,
                        Color.White,
                        0f,
                        new Vector2(selectedItemHighlight.Width / 2, selectedItemHighlight.Height / 2),
                        1.0f, SpriteEffects.None, 0f);
                }
            }
            }
    }
}
