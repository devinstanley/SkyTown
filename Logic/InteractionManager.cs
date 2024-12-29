using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Items;
using SkyTown.LogicManagers;
using SkyTown.Map;

namespace SkyTown.Logic
{
    public class InteractionManager
    {
        int InteractionDistance = 40;
        public InteractionManager()
        {

        }

        public void Update(GameTime gameTime, InputManager inputManager, MapScene scene, Player player)
        {
            HandlePlayerNPCInteractions(gameTime, inputManager, scene.npcManager, player);
            HandlePlayerEntityInteractions(gameTime, inputManager, scene.SceneEntities, player);
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

            //Get All NPCs Inside of the Players Interaction Range
            foreach (NPC npc in npcManager.NPCs)
            {
                if (playerRangeRect.Contains(npc.Position))
                {
                    interactableNPCs.Add(npc);
                }
            }

            if (inputManager.IsRightClicked())
            {
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
                            player.inventory.RemoveItem(player.inventory.CurrentSelectedItem);
                        }
                        else
                        {
                            npc.Talk();
                        }
                    }
                }
            }
        }

        public void HandlePlayerEntityInteractions(GameTime gameTime, InputManager inputManager, List<Entity> sceneEntities, Player player)
        {
            List<Entity> interactableEntities = new List<Entity>();
            Rectangle playerRangeRect = new Rectangle(
                (int)(player.Position.X - player.Width / 2f - InteractionDistance / 2f),
                (int)(player.Position.Y - player.Height / 2f - InteractionDistance / 2f),
                player.Width + InteractionDistance,
                player.Height + InteractionDistance
                );

            //Get All Entities Inside of the Players Interaction Range
            foreach (Entity entity in sceneEntities)
            {
                if (playerRangeRect.Contains(entity.Position))
                {
                    interactableEntities.Add(entity);
                }
            }

            if (inputManager.IsRightClicked())
            {
                foreach (Entity entity in interactableEntities)
                {
                    Rectangle entityRect = new Rectangle(
                        (int)entity.Position.X - entity.Width/2,
                        (int)entity.Position.Y - entity.Height/2,
                        entity.Width,
                        entity.Height
                        );
                    if (entityRect.Contains(inputManager.GetMousePosition()))
                    {
                        if (entity.HarvestAction == HarvestActionEnum.HAND)
                        {
                            Item harvestedItem = entity.ToItem();
                            player.inventory.AddItem(entity.ToItem());
                            sceneEntities.Remove(entity);
                        }
                    }
                }
            }
        }
    }
}
