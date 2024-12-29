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
        new Vector2 InventoryStartLoc = new(8, 8);
        float scale = 0.5f;
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
            Keys[] numKeys = {
                Keys.NumPad1, Keys.D1,
                Keys.NumPad2, Keys.D2,
                Keys.NumPad3, Keys.D3,
                Keys.NumPad4, Keys.D4,
                Keys.NumPad5, Keys.D5,
                Keys.NumPad6, Keys.D6,
                Keys.NumPad7, Keys.D7,
                Keys.NumPad8, Keys.D8,
                Keys.NumPad9, Keys.D9,
            };
            foreach (Keys key in numKeys)
            {
                if (inputManager.IsKeyDown(key))
                {

                }
            }

            inputManager.IsKeyDown
        }

        new public int GetKeyAtPos(InputManager inputManager)
        {
            // Get the mouse position
            Vector2 mousePosition = inputManager.GetMousePosition();

            // Calculate grid-relative mouse position
            float relativeX = mousePosition.X + scale*inventoryTexture.Width / 2f - scale*InventoryStartLoc.X + scale*InventorySlotDimensions - player.Position.X - player.Width / 2f + 16;
            float relativeY = mousePosition.Y + scale*inventoryTexture.Height / 2f - scale * InventoryStartLoc.Y + 16 - player.Position.Y - player.Height / 2f + game.ViewCamera._resolutionHeight / 2 + 64;

            // Convert to slot indices
            float slotX = relativeX / (InventorySlotDimensions + InventorySpacer);
            float slotY = relativeY / (InventorySlotDimensions + InventorySpacer);

            // Check if within grid bounds
            if (slotX >= 0 && slotX < INVENTORYWIDTH && slotY == 0)
            {
                // Convert 2D grid coordinates to the flattened key
                return (int)3 * INVENTORYWIDTH + (int)slotX;
            }

            // Return -1 if the mouse position is outside the grid
            return -1;
        }

        new public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            bool bottomRender = game.ViewCamera.TopScreenClamp;
            Vector2 hudPos = new();
            if (bottomRender)
            {
                hudPos = new Vector2(game.ViewCamera._position.X, game.ViewCamera._position.Y + player.Height / 2 + 16 + game.ViewCamera._resolutionHeight / 2);
            }
            else
            {
                hudPos = new Vector2(game.ViewCamera._position.X, game.ViewCamera._position.Y - player.Height / 2 - 16 - game.ViewCamera._resolutionHeight / 2);
            }
            spriteBatch.Draw(
                inventoryTexture,
                hudPos,
                null,
                Color.White,
                0f,
                new Vector2(inventoryTexture.Width / 2, inventoryTexture.Height / 2),
                scale, SpriteEffects.None, 0f);

            foreach (var itemSlot in _inventory.Items)
            {
                int slotX = itemSlot.Key % INVENTORYWIDTH; // Column index
                int slotY = itemSlot.Key / INVENTORYWIDTH; // Row index

                if (slotY != 3)
                {
                    continue;
                }
                Vector2 position = new();
                if (bottomRender)
                {
                    position = new(
                    game.ViewCamera._position.X - scale * inventoryTexture.Width / 2 + scale * InventoryStartLoc.X + scale * InventorySlotDimensions / 2 + (slotX * scale * (InventorySlotDimensions + InventorySpacer)),
                    game.ViewCamera._position.Y + player.Height / 2 + inventoryTexture.Height / 2 - 16 + game.ViewCamera._resolutionHeight / 2 - InventoryStartLoc.Y
                    );
                }
                else {
                    position = new(
                    game.ViewCamera._position.X - scale * inventoryTexture.Width / 2 + scale * InventoryStartLoc.X + scale * InventorySlotDimensions / 2 + (slotX * scale * (InventorySlotDimensions + InventorySpacer)),
                    game.ViewCamera._position.Y - player.Height/2 - inventoryTexture.Height / 2 + 16 - game.ViewCamera._resolutionHeight / 2 - InventoryStartLoc.Y
                    );
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
                        scale + 0.05f, SpriteEffects.None, 0f);
                }
                itemSlot.Value.Draw(spriteBatch, position, scale - 0.05f);
            }
        }
    }

    public static class NumKeys
    {
        static List<NumKey> numKeys = new List<NumKey>();

        public static void NumKey()
        {

        }
    }

    public class NumKey
    {
        public int Value;
        public List<Keys> keys;
    }
}
