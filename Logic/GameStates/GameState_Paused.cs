﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.HUD.Inventory;
using SkyTown.LogicManagers;

namespace SkyTown.Logic.GameStates
{
    internal class GameState_Paused : IGameState
    {
        private Game1 game;
        private Player player;
        private SpriteFont font;
        const GameState myStateEnum = GameState.Paused;
        InventoryHUD inventoryHUD;

        public GameState_Paused(Game1 game, Player player)
        {
            this.game = game;
            this.player = player;
            this.inventoryHUD = new InventoryHUD(player);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            inventoryHUD.LoadContent();
        }

        public void Update(InputManager inputManager)
        {
            inventoryHUD.Update(inputManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            inventoryHUD.Draw(spriteBatch);
        }
    }
}
