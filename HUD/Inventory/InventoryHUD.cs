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
        public Texture2D selectedItemHighlight { get => ResourceManager.LoadTexture("Assets.HUDs.SelectedItem"); }
        public static Vector2 InventoryStartLoc = new Vector2(14, 73);
        public static int INVENTORYWIDTH = 9;
        public static int INVENTORYHEIGHT = 4;
        public static int MAXSLOTS = INVENTORYHEIGHT * INVENTORYWIDTH;
        public int InventorySlotDimensions = 32;
        public int InventorySpacer = 3;
        protected Game1 game;
        protected Player player;
        protected InventoryManager _inventory;
        public int SelectingSlot = -1;

        public InventoryHUD(Game1 game, Player player)
        {
            this.game = game;
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
            //Check if we are dragging item
            if (inputManager.IsLeftClicked() && SelectingSlot == -1)
            {
                SelectingSlot = _inventory.GetItemAtKey(GetKeyAtPos(inputManager));
                _inventory.CurrentSelectedItem = _inventory.GetItemAtKey(GetKeyAtPos(inputManager));
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
                    _inventory.CurrentSelectedItem = newLoc;
                }
            }
        }

        public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            Debug.WriteLine($"Mouse Pos: {mousePosition}");

            // Calculate grid-relative mouse position
            float relativeX = mousePosition.X + inventoryTexture.Width / 2f - InventoryStartLoc.X + InventorySlotDimensions - player.Position.X - player.Width/2f;
            float relativeY = mousePosition.Y + inventoryTexture.Height / 2f - InventoryStartLoc.Y + InventorySlotDimensions - player.Position.Y - player.Height/2f;

            Debug.WriteLine($"Fixed Mouse Pos: {relativeX}, {relativeY}");

            // Convert to slot indices
            float slotX = relativeX / (InventorySlotDimensions + InventorySpacer);
            float slotY = relativeY / (InventorySlotDimensions + InventorySpacer);

            // Check if within grid bounds
            if (slotX >= 0 && slotX < INVENTORYWIDTH && slotY >= 0 && slotY < INVENTORYHEIGHT)
            {
                // Convert 2D grid coordinates to the flattened key
                return (int)slotY * INVENTORYWIDTH + (int)slotX;
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
                int slotX = itemSlot.Key % INVENTORYWIDTH; // Column index
                int slotY = itemSlot.Key / INVENTORYWIDTH; // Row index

                Vector2 position = new(
                    player.Position.X + InventoryStartLoc.X + slotX*(InventorySlotDimensions + InventorySpacer) + itemSlot.Value.Item.Width / 2 - inventoryTexture.Width / 2,
                    player.Position.Y + InventoryStartLoc.Y + slotY*(InventorySlotDimensions + InventorySpacer) + itemSlot.Value.Item.Height / 2 - inventoryTexture.Height / 2
                    );

                if (slotY == 3)
                {
                    position.Y += 1;
                }

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
                if (itemSlot.Key == SelectingSlot)
                {
                    continue; //Skip drawing currently selected item
                }

                itemSlot.Value.Draw(spriteBatch, position, 0.8f);
            }
            if (SelectingSlot >= 0)
            {
                _inventory.Items[SelectingSlot].Draw(spriteBatch, _inventory.Items[SelectingSlot].Item.Position, 1f);
            }

        }
    }

}
