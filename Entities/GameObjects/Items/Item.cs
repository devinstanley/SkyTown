using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using SkyTown.Map;
using System;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{

    public class Item : GameObject, IInteractor
    {
        int InteractionDistance = 40;
        int ItemFollowDistance = 100;
        int ItemFollowSpeed = 200;

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
            float dist = (player.Position - Position).Length();
            if (dist < 10)
            {
                player.inventory.AddItem(this);
                mapScene.SceneObjects.Remove(this);
            }
            else if (dist < ItemFollowDistance)
            {
                Vector2 vel = (player.Position - Position);
                vel.Normalize();
                float displacementScalar = (float)(ItemFollowSpeed * gameTime.ElapsedGameTime.TotalSeconds * Math.Clamp((50 / dist), 0, 1));
                Position += displacementScalar * vel;
            }
        }

        public void DrawInInventory(SpriteBatch spriteBatch, Vector2 position, float scale = -1)
        {
            InventoryAnimation.Draw(spriteBatch, position, scale);
        }
    }
}
