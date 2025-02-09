using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using SkyTown.Map;

namespace SkyTown.Entities.GameObjects
{
    public class DispensableObject: GameObject, IInteractor
    {
        public string DispensedItemID;
        public int NumberDispensed;
        public bool DispenseReady = true;
        public double CoolDown;
        private double RemainingCoolDown;
        public DispensableObject(string ID, IAnimator animator, string dispencedItem, int numDispensed, double coolDown) : base(ID)
        {
            AnimationHandler = animator;
            DispensedItemID = dispencedItem;
            NumberDispensed = numDispensed;
            CoolDown = coolDown;
        }
        public override void Update()
        {
            AnimationHandler.Update();
            if (DispenseReady)
            {
                return;
            }
            RemainingCoolDown -= GameGlobals.ElapsedGameTime;
            if (RemainingCoolDown <= 0)
            {
                DispenseReady = true;
                RemainingCoolDown = 0;
                if (AnimationHandler is AnimationManager animator)
                {
                    animator.UpdateAnimationSequence("0");
                }
            }
            
        }
        public void Interact(Player player, MapScene mapScene)
        {
            if (!DispenseReady)
            {
                return;
            }
            else
            {
                Dispense(mapScene);
            }
            
        }

        public void Dispense(MapScene mapScene)
        {
            RemainingCoolDown = CoolDown;
            DispenseReady = false;
            if (AnimationHandler is AnimationManager animator)
            {
                animator.UpdateAnimationSequence("1");
            }

            var rand = new Random();

            for (int i = 0; i < NumberDispensed; i++)
            {
                var spawned_item = ItemManager.GetItem(DispensedItemID);
                spawned_item.Position = Position + new Vector2(rand.Next(3), rand.Next(3));
                mapScene.SceneObjects.Add(spawned_item);
            }
        }
    }
}
