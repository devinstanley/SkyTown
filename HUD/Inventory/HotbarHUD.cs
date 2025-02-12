using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System;

namespace SkyTown.HUD.Inventory
{
    internal class HotbarHUD : InventoryHUD
    {
        new Texture2D InventoryTexture { get => ResourceManager.LoadTexture("Assets.HUDs.HotbarHUD"); }
        new Texture2D selectedItemHighlight { get => ResourceManager.LoadTexture("Assets.HUDs.SelectedItemHotbarHUD"); }
        new Vector2 InventoryStartLoc = new(5, 5);
        new int InventorySlotDimensions = 16;
        bool BottomRender
        {
            get
            {
                return Camera.TopScreenClamp;
            }
        }
        float tileScale = 0.5f;
        public HotbarHUD(Player player) : base(player)
        {
        }

        new public void Update(InputManager inputManager)
        {

            HandleInput(inputManager);
            Inventory.Update();
        }

        new public void HandleInput(InputManager inputManager)
        {
            int keyNum = inputManager.GetNumKeyDown();
            if (keyNum != -1)
            {
                //Convert Key Number to Position in Hotbar
                Inventory.CurrentItemKey = keyNum + InventoryManager.INVENTORYWIDTH * (InventoryManager.INVENTORYHEIGHT - 1) - 1;
            }

            if (inputManager.IsLeftClicked() && SelectingSlot == -1)
            {
                int curItem = GetKeyAtPos(inputManager) % InventoryManager.INVENTORYWIDTH;
                if (curItem != -1)
                {
                    Inventory.CurrentItemKey = curItem;
                }
            }
            if (inputManager.IsLeftClickHolding() && SelectingSlot == -1)
            {
                SelectingSlot = Inventory.GetItemAtKey(GetKeyAtPos(inputManager));

            }
            if (SelectingSlot != -1)
            {
                if (inputManager.IsLeftClickHolding())
                {
                    Inventory.Items[SelectingSlot].Item.Position = inputManager.GetMousePosition();
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
                        Inventory.SwapOrStack(SelectingSlot, newLoc);
                    }
                    if (newLoc > InventoryManager.INVENTORYWIDTH * (InventoryManager.INVENTORYHEIGHT - 1) - 1)
                    {
                        Inventory.CurrentItemKey = newLoc;
                    }

                    SelectingSlot = -1;
                }
            }
        }

        new public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            float correctedMousePositionX = mousePosition.X - Camera.Position.X;
            float correctedMousePositionY = mousePosition.Y - Camera.Position.Y;

            if (BottomRender)
            {
                correctedMousePositionY = mousePosition.Y - Camera.Position.Y - Camera.ResolutionHeight / 2f + InventoryTexture.Height / 2;
            }
            else
            {
                correctedMousePositionY = mousePosition.Y - Camera.Position.Y + Camera.ResolutionHeight / 2f - InventoryTexture.Height / 2;
            }


            // Calculate grid-relative mouse position
            float relativeX = correctedMousePositionX + InventoryTexture.Width / 2f - InventoryStartLoc.X;
            float relativeY = correctedMousePositionY + InventoryTexture.Height / 2f - InventoryStartLoc.Y;


            // Convert to slot indices
            int slotX = (int)Math.Floor(relativeX / (InventorySlotDimensions + InventorySpacer));
            int slotY = (int)Math.Floor(relativeY / (InventorySlotDimensions + InventorySpacer));


            // Check if within grid bounds
            if (slotX >= 0 && slotX < InventoryManager.INVENTORYWIDTH && slotY == 0)
            {
                // Convert 2D grid coordinates to the flattened key
                return (int)3 * InventoryManager.INVENTORYWIDTH + (int)slotX;
            }

            // Return -1 if the mouse position is outside the grid
            return -1;
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 hudPos = new();
            if (BottomRender)
            {
                hudPos = new Vector2(Camera.Position.X, Camera.Position.Y + Camera.ResolutionHeight / 2 - InventoryTexture.Height / 2);
            }
            else
            {
                hudPos = new Vector2(Camera.Position.X, Camera.Position.Y - Camera.ResolutionHeight / 2 + InventoryTexture.Height / 2);
            }
            //Draw hotbar HUD
            spriteBatch.Draw(
                InventoryTexture,
                hudPos,
                null,
                Color.White,
                0f,
                new Vector2(InventoryTexture.Width / 2, InventoryTexture.Height / 2),
                1f, SpriteEffects.None, 0f);

            //Draw selection highlight
            int slotX = Inventory.CurrentItemKey % InventoryManager.INVENTORYWIDTH; // Column index
            int slotY = Inventory.CurrentItemKey / InventoryManager.INVENTORYWIDTH; // Row index
            Vector2 position = new();
            if (BottomRender)
            {
                position = new(
                Camera.Position.X - InventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                Camera.Position.Y + Camera.ResolutionHeight / 2 - InventoryTexture.Height / 2
                );
            }
            else
            {
                position = new(
                Camera.Position.X - InventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                Camera.Position.Y - Camera.ResolutionHeight / 2 + InventoryTexture.Height / 2
                );
            }
            spriteBatch.Draw(
                selectedItemHighlight,
                position,
                null,
                Color.White,
                0f,
                new Vector2(selectedItemHighlight.Width / 2, selectedItemHighlight.Height / 2),
                1f, SpriteEffects.None, 0f);


            foreach (var itemSlot in Inventory.Items)
            {
                slotX = itemSlot.Key % InventoryManager.INVENTORYWIDTH; // Column index
                slotY = itemSlot.Key / InventoryManager.INVENTORYWIDTH; // Row index

                //Only Draw Hotbar Items
                if (slotY != 3)
                {
                    continue;
                }
                position = new();
                if (BottomRender)
                {
                    position = new(
                    Camera.Position.X - InventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                    Camera.Position.Y + Camera.ResolutionHeight / 2 - InventoryTexture.Height / 2
                    );
                }
                else
                {
                    position = new(
                    Camera.Position.X - InventoryTexture.Width / 2 + InventoryStartLoc.X + InventorySlotDimensions / 2 + (slotX * (InventorySlotDimensions + InventorySpacer)),
                    Camera.Position.Y - Camera.ResolutionHeight / 2 + InventoryTexture.Height / 2
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
                Inventory.Items[SelectingSlot].Draw(spriteBatch, Inventory.Items[SelectingSlot].Item.Position, scale: tileScale);
            }
        }
    }
}
