using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System.Diagnostics;

namespace SkyTown.HUD.Inventory
{
    internal class InventoryHUD
    {
        public Texture2D InventoryTexture { get => ResourceManager.LoadTexture("Assets.HUDs.InventoryHUD"); }
        public Texture2D selectedItemHighlight { get => ResourceManager.LoadTexture("Assets.HUDs.SelectedItemInventoryHUD"); }
        public static Vector2 InventoryStartLoc = new Vector2(14, 73);
        public int InventorySlotDimensions = 32;
        public int InventorySpacer = 3;

        public int SelectingSlot = -1;

        protected Game1 Game;
        protected Player Player1;
        protected InventoryManager Inventory;


        public InventoryHUD(Game1 game, Player player)
        {
            Game = game;
            Player1 = player;
            Inventory = player.inventory;
        }

        public void LoadContent()
        {
        }

        public void Update(InputManager inputManager)
        {
            HandleInput(inputManager);
            Inventory.Update();
        }

        public void HandleInput(InputManager inputManager)
        {
            //Change held item - need to merge some logic with hotbar hud
            int keyNum = inputManager.GetNumKeyDown();
            if (keyNum != -1)
            {
                //Convert Key Number to Position in Hotbar
                Inventory.CurrentItemKey = keyNum + InventoryManager.INVENTORYWIDTH * (InventoryManager.INVENTORYHEIGHT - 1) - 1;
            }

            //Check if we are dragging item
            if (inputManager.IsRightClicked() && SelectingSlot == -1)
            {
                SelectingSlot = Inventory.GetItemAtKey(GetKeyAtPos(inputManager));
                Inventory.SplitStack(SelectingSlot);
                SelectingSlot = -1;
            }
            if (inputManager.IsLeftClicked() && SelectingSlot == -1)
            {
                SelectingSlot = Inventory.GetItemAtKey(GetKeyAtPos(inputManager));
            }
            if (SelectingSlot != -1)
            {
                if (inputManager.IsLeftClickDown())
                {
                    Inventory.Items[SelectingSlot].Item.Position = inputManager.GetMousePosition();
                }
                else
                {
                    int newLoc = GetKeyAtPos(inputManager);
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

        public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            Debug.WriteLine($"Mouse Pos: {mousePosition}");

            // Calculate grid-relative mouse position
            float relativeX = mousePosition.X + InventoryTexture.Width / 2f - InventoryStartLoc.X + InventorySlotDimensions - Game.ViewCamera._position.X - Player1.Width / 2f;
            float relativeY = mousePosition.Y + InventoryTexture.Height / 2f - InventoryStartLoc.Y + InventorySlotDimensions - Game.ViewCamera._position.Y - Player1.Height / 2f;

            Debug.WriteLine($"Fixed Mouse Pos: {relativeX}, {relativeY}");

            // Convert to slot indices
            float slotX = relativeX / (InventorySlotDimensions + InventorySpacer);
            float slotY = relativeY / (InventorySlotDimensions + InventorySpacer);

            // Check if within grid bounds
            if (slotX >= 0 && slotX < InventoryManager.INVENTORYWIDTH && slotY >= 0 && slotY < InventoryManager.INVENTORYHEIGHT)
            {
                // Convert 2D grid coordinates to the flattened key
                return (int)slotY * InventoryManager.INVENTORYWIDTH + (int)slotX;
            }

            // Return -1 if the mouse position is outside the grid
            return -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw inventory HUD
            spriteBatch.Draw(
                InventoryTexture,
                Game.ViewCamera._position,
                null,
                Color.White,
                0f,
                new Vector2(InventoryTexture.Width / 2, InventoryTexture.Height / 2),
                1f, SpriteEffects.None, 0f);

            //Draw Current Item Selection
            //Draw selection highlight
            int slotX = Inventory.CurrentItemKey % InventoryManager.INVENTORYWIDTH; // Column index
            int slotY = InventoryManager.INVENTORYHEIGHT - 1; // Row index
            Vector2 position = new();
            position = new(
                    Game.ViewCamera._position.X + InventoryStartLoc.X + slotX * (InventorySlotDimensions + InventorySpacer) + selectedItemHighlight.Width / 2 - InventoryTexture.Width / 2,
                    Game.ViewCamera._position.Y + InventoryStartLoc.Y + slotY * (InventorySlotDimensions + InventorySpacer) + selectedItemHighlight.Height / 2 - InventoryTexture.Height / 2 + 1
                    );
            //Draw selected item highlight
            spriteBatch.Draw(
                selectedItemHighlight,
                position,
                null,
                Color.White,
                0f,
                new Vector2(selectedItemHighlight.Width / 2, selectedItemHighlight.Height / 2),
                1.0f, SpriteEffects.None, 0f);

            foreach (var itemSlot in Inventory.Items)
            {
                slotX = itemSlot.Key % InventoryManager.INVENTORYWIDTH; // Column index
                slotY = itemSlot.Key / InventoryManager.INVENTORYWIDTH; // Row index

                position = new(
                    Game.ViewCamera._position.X + InventoryStartLoc.X + slotX * (InventorySlotDimensions + InventorySpacer) + itemSlot.Value.Item.InventoryAnimation.Width / 2 - InventoryTexture.Width / 2,
                    Game.ViewCamera._position.Y + InventoryStartLoc.Y + slotY * (InventorySlotDimensions + InventorySpacer) + itemSlot.Value.Item.InventoryAnimation.Height / 2 - InventoryTexture.Height / 2
                    );

                if (slotY == 3)
                {
                    position.Y += 1;
                }
                if (itemSlot.Key == SelectingSlot)
                {
                    continue; //Skip drawing currently selected item
                }

                itemSlot.Value.Draw(spriteBatch, position, 0.8f);
            }
            if (SelectingSlot >= 0)
            {
                Inventory.Items[SelectingSlot].Draw(spriteBatch, Inventory.Items[SelectingSlot].Item.Position, 1f);
            }

        }
    }

}
