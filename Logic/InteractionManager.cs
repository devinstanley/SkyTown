using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Items;
using SkyTown.LogicManagers;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Logic
{
    public class InteractionManager
    {
        int InteractionDistance = 40;
        int ItemFollowDistance = 100;
        int ItemFollowSpeed = 200;
        public InteractionManager()
        {

        }

        public void Update(GameTime gameTime, InputManager inputManager, MapScene scene, Player player)
        {
            //HandlePlayerNPCInteractions(gameTime, inputManager, scene.NpcManager, player);
            HandlePlayerEntityInteractions(gameTime, inputManager, scene.SceneHarvestables, player);
            HandlePlayerItemInteractions(gameTime, scene.SceneItems, player);
        }

        public void HandlePlayerNPCInteractions(GameTime gameTime, InputManager inputManager, NPCManager npcManager, Player player)
        {
            List<NPC> interactableNPCs = new List<NPC>();
            Rectangle playerRangeRect = new Rectangle(
                (int)(player.Position.X - player.Width / 2f - InteractionDistance / 2f),
                (int)(player.Position.Y - player.Height / 2f - InteractionDistance / 2f),
                player.Width + InteractionDistance,
                player.Height + InteractionDistance
                );


            /*
            if (inputManager.IsRightClicked())
            {
                //Get All NPCs Inside of the Players Interaction Range
                foreach (NPC npc in npcManager.NPCs)
                {
                    if (playerRangeRect.Contains(npc.Position))
                    {
                        interactableNPCs.Add(npc);
                    }
                }

                foreach (NPC npc in interactableNPCs)
                {
                    Rectangle entityRect = new Rectangle(
                        (int)npc.Position.X - npc.Width / 2,
                        (int)npc.Position.Y - npc.Height / 2,
                        npc.Width,
                        npc.Height
                        );
                    if (entityRect.Contains(inputManager.GetMousePosition()))
                    {
                        if (player.inventory.CurrentItem != null && player.inventory.CurrentItem.Giftable)
                        {
                            npc.Talk(player.inventory.CurrentItem);
                            player.inventory.RemoveItem(player.inventory.CurrentItemKey);
                        }
                        else
                        {
                            npc.Talk();
                        }
                    }
                }
            }
            */
        }

        public void HandlePlayerEntityInteractions(GameTime gameTime, InputManager inputManager, List<HarvestableObject> sceneEntities, Player player)
        {
            List<HarvestableObject> interactableEntities = new List<HarvestableObject>();
            Rectangle playerRangeRect = new Rectangle(
                (int)(player.Position.X - player.Width / 2f - InteractionDistance / 2f),
                (int)(player.Position.Y - player.Height / 2f - InteractionDistance / 2f),
                player.Width + InteractionDistance,
                player.Height + InteractionDistance
                );

            //Get All Entities Inside of the Players Interaction Range
            foreach (HarvestableObject entity in sceneEntities)
            {
                if (playerRangeRect.Contains(entity.Position))
                {
                    interactableEntities.Add(entity);
                }
            }

            if (inputManager.IsRightClicked())
            {
                foreach (HarvestableObject entity in interactableEntities)
                {
                    Rectangle entityRect = new Rectangle(
                        (int)entity.Position.X - entity.Width / 2,
                        (int)entity.Position.Y - entity.Height / 2,
                        entity.Width,
                        entity.Height
                        );
                    /*
                    if (entityRect.Contains(inputManager.GetMousePosition()))
                    {
                        if (entity.HarvestAction == HarvestActionEnum.HAND)
                        {
                            Item harvestedItem = entity.ToItem();
                            player.inventory.AddItem(entity.ToItem());
                            sceneEntities.Remove(entity);
                        }
                    }
                    */
                }
            }
        }

        public void HandlePlayerItemInteractions(GameTime gameTime, List<Item> AttainableItems, Player player)
        {
            List<Item> ItemsCopy = new List<Item>(AttainableItems);

            foreach (Item item in ItemsCopy)
            {
                float dist = (player.Position - item.Position).Length();
                if (dist < 10)
                {
                    player.inventory.AddItem(item);
                    AttainableItems.Remove(item);
                }
                else if (dist < ItemFollowDistance)
                {
                    Vector2 vel = (player.Position - item.Position);
                    vel.Normalize();
                    float displacementScalar = (float)(ItemFollowSpeed * gameTime.ElapsedGameTime.TotalSeconds * Math.Clamp((50 / dist), 0, 1));
                    item.Position += displacementScalar * vel;
                }
            }
        }
    }
}
