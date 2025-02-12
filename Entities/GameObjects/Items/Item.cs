using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using SkyTown.Map;
using System;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{

    public class Item : GameObject, IInteractor
    {
        int ItemFollowDistance = 100;
        int ItemFollowSpeed = 200;

        public int MaxStack;
        public IAnimator InventoryAnimation;
        public Item(string id, int maxStack, IAnimator animation, IAnimator inventoryAnimation) : base(id)
        {
            MaxStack = maxStack;
            AnimationHandler = animation;
            InventoryAnimation = inventoryAnimation;
        }

        public override void Update()
        {
            InventoryAnimation.Update();
            base.Update();
        }

        public void Interact(Player player, MapScene mapScene)
        {
            float dist = (player.Position - Position).Length();
            if (dist < 10)
            {
                player.inventory.AddItem(this.ObjectID);
                mapScene.SceneObjects.Remove(this);
            }
            else if (dist < ItemFollowDistance)
            {
                Vector2 vel = (player.Position - Position);
                vel.Normalize();
                float displacementScalar = (float)(ItemFollowSpeed * GameGlobals.ElapsedGameTime * Math.Clamp((50 / dist), 0, 1));
                Position += displacementScalar * vel;
            }
        }

        public void DrawInInventory(SpriteBatch spriteBatch, Vector2 position, float scale = -1)
        {
            InventoryAnimation.Draw(spriteBatch, position, scale);
        }
    }
}
