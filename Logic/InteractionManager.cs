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
    public static class InteractionManager
    {

        static int InteractionDistance = 80;
        static InteractionManager()
        {

        }

        public static void Update(InputManager inputManager, MapScene mapScene, Player player)
        {
            //HandlePlayerNPCInteractions(gameTime, inputManager, scene.NpcManager, player);
            HandlePlayerItemInteractions(mapScene, player);
            HandlePlayerDispenserInteractions(inputManager, mapScene, player);


        }

        public static bool ObjectClicked(InputManager inputManager, GameObject gameObject)
        {
            if (!inputManager.IsLeftClicked())
            {
                return false;
            }
            Rectangle interactionRect;
            if (gameObject.CollisionRectangle != null)
            {
                Rectangle hitbox = (Rectangle)gameObject.CollisionRectangle;
                interactionRect = new(
                            (int)gameObject.Position.X + hitbox.Left - gameObject.Width / 2,
                            (int)gameObject.Position.Y + hitbox.Top - gameObject.Height / 2,
                            gameObject.Width,
                            gameObject.Height
                        );
            }
            else
            {
                interactionRect = new Rectangle(
                    (int)gameObject.Position.X, 
                    (int)gameObject.Position.X, 
                    gameObject.Width, 
                    gameObject.Height);
            }

            if (interactionRect.Contains(inputManager.GetMousePosition()))
            {
                return true;
            }
            return false;
        }

        public static void HandlePlayerDispenserInteractions(InputManager inputManager, MapScene mapScene, Player player)
        {
            List<DispensableObject> DispensableCopy = mapScene.SceneObjects.OfType<DispensableObject>().ToList();

            foreach (DispensableObject dispensable in DispensableCopy)
            {
                float dist = (player.Position - dispensable.Position).Length();
                if (dist < InteractionDistance &&
                    ObjectClicked(inputManager, dispensable))
                {
                    {
                        dispensable.Interact(player, mapScene);
                        continue;
                    }
                }
            }
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

        public static void HandlePlayerItemInteractions(MapScene mapScene, Player player)
        {
            List<Item> ItemsCopy = mapScene.SceneObjects.OfType<Item>().ToList();

            foreach (Item item in ItemsCopy)
            {
                item.Interact(player, mapScene);
            }
        }
    }
}
