using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.LogicManagers;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Logic
{
    public class InteractionManager
    {
        
        public InteractionManager()
        {

        }

        public void Update(InputManager inputManager, MapScene mapScene, Player player)
        {
            //HandlePlayerNPCInteractions(gameTime, inputManager, scene.NpcManager, player);
            HandlePlayerItemInteractions(mapScene, player);
        }
        /*
        public void HandlePlayerNPCInteractions(GameTime gameTime, InputManager inputManager, NPCManager npcManager, Player player)
        {
            List<NPC> interactableNPCs = new List<NPC>();
            Rectangle playerRangeRect = new Rectangle(
                (int)(player.Position.X - player.Width / 2f - InteractionDistance / 2f),
                (int)(player.Position.Y - player.Height / 2f - InteractionDistance / 2f),
                player.Width + InteractionDistance,
                player.Height + InteractionDistance
                );


            
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
        }
        */

        public void HandlePlayerItemInteractions(MapScene mapScene, Player player)
        {
            List<IInteractor> ItemsCopy = mapScene.SceneObjects.OfType<IInteractor>().ToList();

            foreach (IInteractor interactable in ItemsCopy)
            {
                interactable.Interact(player, mapScene);
            }
        }
    }
}
