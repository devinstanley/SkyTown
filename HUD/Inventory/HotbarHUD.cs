using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace SkyTown.HUD.Inventory
{
    internal class HotbarHUD: InventoryHUD
    {
        new Texture2D inventoryTexture { get => ResourceManager.LoadTexture("Assets.HUDs.HotbarHUD"); }
        new Vector2 InventoryStartLoc = new(5, 5);
        new int InventorySlotDimensions = 16;
        bool bottomRender { 
            get
            {
                return game.ViewCamera.TopScreenClamp;
            } 
        }
        float tileScale = 0.5f;
        public HotbarHUD(Game1 game, Player player): base(game, player)
        {
        }

        new public void Update(GameTime gameTime, InputManager inputManager)
        {

            HandleInput(inputManager);
            _inventory.Update(gameTime);
        }

        new public void HandleInput(InputManager inputManager)
        {
            int keyNum = inputManager.GetNumKeyDown();
            if (keyNum != -1)
            {
                //Convert Key Number to Position in Hotbar
                _inventory.CurrentItemKey = keyNum + InventoryManager.INVENTORYWIDTH * (InventoryManager.INVENTORYHEIGHT - 1) - 1;
            }

            if (inputManager.IsLeftClicked() && SelectingSlot == -1)
            {
                int curItem = GetKeyAtPos(inputManager) % InventoryManager.INVENTORYWIDTH;
                if (curItem != -1)
                {
                    _inventory.CurrentItemKey = curItem;
                }
            }
            if (inputManager.IsLeftClickHolding() && SelectingSlot == -1)
            {
                SelectingSlot = _inventory.GetItemAtKey(GetKeyAtPos(inputManager));

            }
            if (SelectingSlot != -1)
            {
                if (inputManager.IsLeftClickHolding())
                {
                    _inventory.Items[SelectingSlot].Item.Position = inputManager.GetMousePosition();
                }
                else
                {
                    int newLoc = GetKeyAtPos(inputManager);
                    //Drop Item
                    if (newLoc == -1)
                    {

                    }
                    if (newLoc != -1 && newLoc != SelectingSlot)
                    {
                        _inventory.SwapOrStack(SelectingSlot, newLoc);
                    }
                    if (newLoc > InventoryManager.INVENTORYWIDTH * (InventoryManager.INVENTORYHEIGHT - 1) - 1)
                    {
                        _inventory.CurrentItemKey = newLoc;
                    }

                    SelectingSlot = -1;
                }
            }
        }

        new public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            float correctedMousePositionX = mousePosition.X - game.ViewCamera._position.X;
            float correctedMousePositionY = mousePosition.Y - game.ViewCamera._position.Y;

            Debug.WriteLine($"Point Corrected: {correctedMousePositionX}, {correctedMousePositionY}");

            if (bottomRender)
            {
                correctedMousePositionY = mousePosition.Y - game.ViewCamera._position.Y - game.ViewCamera._resolutionHeight / 2f - player.Height/2f - 16;
            }
            else
            {
                correctedMousePositionY = mousePosition.Y - game.ViewCamera._position.Y + game.ViewCamera._resolutionHeight / 2f + player.Height/2f + 16;
            }

            Debug.WriteLine($"HUD Pos Corrected: {correctedMousePositionX}, {correctedMousePositionY}");

            // Calculate grid-relative mouse position
            float relativeX = correctedMousePositionX + inventoryTexture.Width / 2f - InventoryStartLoc.X;
            float relativeY = correctedMousePositionY + inventoryTexture.Height / 2f - InventoryStartLoc.Y;

            Debug.WriteLine($"HUD Pos Full Corrected: {relativeX}, {relativeY}");

            // Convert to slot indices
            int slotX = (int) Math.Floor(relativeX / (InventorySlotDimensions + InventorySpacer));
            int slotY = (int) Math.Floor(relativeY / (InventorySlotDimensions + InventorySpacer));

            Debug.WriteLine($"Slots: {slotX}, {slotY}");

            // Check if within grid bounds
            if (slotX >= 0 && slotX < InventoryManager.INVENTORYWIDTH && slotY == 0)
            {
                // Convert 2D grid coordinates to the flattened key
                return (int)3 * InventoryManager.INVENTORYWIDTH + (int)slotX;
            }

            // Return -1 if the mouse position is outside the grid
            return -1;
        }

        new public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 hudPos = new();
            if (bottomRender)
            {
                hudPos = new Vector2(game.ViewCamera._position.X, game.ViewCamera._position.Y + 16 + player.Height / 2f + game.ViewCamera._resolutionHeight / 2f);
            }
            else
            {
                hudPos = new Vector2(game.ViewCamera._position.X, game.ViewCamera._position.Y - 16 - player.Height / 2f - game.ViewCamera._resolutionHeight / 2f);
            }
            //Draw hotbar HUD
            spriteBatch.Draw(
                inventoryTexture,
                hudPos,
                null,
                Color.White,
                0f,
                new Vector2(inventoryTexture.Width / 2, inventoryTexture.Height / 2),
                1f, SpriteEffects.None, 0f);

            //Draw selection highlight
            int slotX = _inventory.CurrentItemKey % InventoryManager.INVENTORYWIDTH; // Column index
            int slotY = _inventory.CurrentItemKey / InventoryManager.INVENTORYWIDTH; // Row index
            Vector2 position = new();
            if (bottomRender)
            {
                position = new(
                game.ViewCamera._position.X - inventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                game.ViewCamera._position.Y + player.Height / 2 + 16 + game.ViewCamera._resolutionHeight / 2
                );
            }
            else
            {
                position = new(
                game.ViewCamera._position.X - inventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                game.ViewCamera._position.Y - player.Height / 2 - 16 - game.ViewCamera._resolutionHeight / 2
                );
            }
            position.Y += 0.5f;
            spriteBatch.Draw(
                selectedItemHighlight,
                position,
                null,
                Color.White,
                0f,
                new Vector2(selectedItemHighlight.Width / 2, selectedItemHighlight.Height / 2),
                tileScale, SpriteEffects.None, 0f);

            
            foreach (var itemSlot in _inventory.Items)
            {
                slotX = itemSlot.Key % InventoryManager.INVENTORYWIDTH; // Column index
                slotY = itemSlot.Key / InventoryManager.INVENTORYWIDTH; // Row index

                //Only Draw Hotbar Items
                if (slotY != 3)
                {
                    continue;
                }
                position = new();
                if (bottomRender)
                {
                    position = new(
                    game.ViewCamera._position.X - inventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                    game.ViewCamera._position.Y + player.Height / 2 + 16 + game.ViewCamera._resolutionHeight / 2
                    );
                }
                else {
                    position = new(
                    game.ViewCamera._position.X - inventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                    game.ViewCamera._position.Y - player.Height/2  - 16 - game.ViewCamera._resolutionHeight / 2
                    );
                }
                if (itemSlot.Key == SelectingSlot)
                {
                    continue; //Skip drawing currently selected item
                }
                itemSlot.Value.Draw(spriteBatch, position, scale: tileScale - 0.05f);
            }

            if (SelectingSlot >= 0)
            {
                _inventory.Items[SelectingSlot].Draw(spriteBatch, _inventory.Items[SelectingSlot].Item.Position, scale: tileScale);
            }
        }
    }
}
