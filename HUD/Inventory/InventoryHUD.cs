using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Characters;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.HUD.Inventory
{
    internal class InventoryHUD
    {
        public Texture2D inventoryTexture { get => ResourceManager.LoadTexture("Assets.HUDs.InventoryHUD"); }
        public static int MAXSLOTS = 20;
        public static int INVENTORYWIDTH = 5;
        public static int INVENTORYHEIGHT = 4;
        public Vector2 InventoryStartLoc;
        public int InventorySlotDimensions = 32;
        private Player player;
        private InventoryManager _inventory;
        public int SelectingSlot = -1;

        public InventoryHUD(Player player)
        {
            this.player = player;
            _inventory = player.inventory;
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            HandleInput(inputManager);
            _inventory.Update(gameTime);
        }

        public void HandleInput(InputManager inputManager)
        {
            //Check if we are selecting item
            if (inputManager.IsLeftClicked() && SelectingSlot == -1)
            {
                SelectingSlot = _inventory.GetItemAtClick(inputManager);
            }
            if (SelectingSlot != -1)
            {
                if (inputManager.IsLeftClickDown())
                {
                    _inventory.Items[SelectingSlot].Item.Position = inputManager.GetMousePosition();
                }
                else
                {
                    int newLoc = GetKeyAtPos(inputManager);
                    if (newLoc != -1)
                    {
                        _inventory.Swap(SelectingSlot, newLoc);
                    }
                    SelectingSlot = -1;
                }
            }
        }

        public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            // Calculate grid-relative mouse position
            float relativeX = mousePosition.X - player.Position.X + inventoryTexture.Width/2;// - inventoryTexture.Width/2f;
            float relativeY = mousePosition.Y - player.Position.Y + inventoryTexture.Height/2;

            Debug.WriteLine($"Mouse Pos: {relativeX}, {relativeY}");

            // Convert to slot indices
            int slotX = (int)(relativeX / InventorySlotDimensions);
            int slotY = (int)(relativeY / InventorySlotDimensions);

            // Check if within grid bounds
            if (slotX >= 0 && slotX < INVENTORYWIDTH && slotY >= 0 && slotY < INVENTORYHEIGHT)
            {
                // Convert 2D grid coordinates to the flattened key
                return slotY * INVENTORYWIDTH + slotX;
            }

            // Return -1 if the mouse position is outside the grid
            return -1;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                inventoryTexture,
                player.Position,
                null,
                Color.White,
                0f,
                new Vector2(inventoryTexture.Width/2, inventoryTexture.Height/2),
                1f, SpriteEffects.None, 0f);
            foreach (var itemSlot in _inventory.Items)
            {
                if (itemSlot.Key == SelectingSlot)
                {
                    continue; //Skip currently selected item
                }
                int slotX = itemSlot.Key % INVENTORYWIDTH; // Column index
                int slotY = itemSlot.Key / INVENTORYWIDTH; // Row index

                Vector2 position = new(
                    player.Position.X + slotX*InventorySlotDimensions + itemSlot.Value.Item.Width / 2 - inventoryTexture.Width / 2,
                    player.Position.Y + slotY*InventorySlotDimensions + itemSlot.Value.Item.Height / 2 - inventoryTexture.Height / 2
                    );
                itemSlot.Value.Draw(spriteBatch, position, 0.9f);
            }
            if (SelectingSlot >= 0)
            {
                _inventory.Items[SelectingSlot].Draw(spriteBatch, _inventory.Items[SelectingSlot].Item.Position, 1f);
            }

        }
    }

}
