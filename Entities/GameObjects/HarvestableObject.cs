using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.GameObjects
{
    public class HarvestableObject: GameObject, IInteractor
    {
        //Dispensor Information
        public string DispensedItemID;
        public int NumberDispensed;

        //Tool Information
        public string RequiredToolType;
        public int RequiredToolLevel;
        public int HP;

        //Local
        private bool _destroying;

        public HarvestableObject(
            string id,
            IAnimator animator,
            string dispensedItem,
            int numDispensed,
            string reqToolType,
            int reqToolLevel,
            int hp,
            Rectangle? collisionRect): base(id, collisionRect)
        {
            AnimationHandler = animator;
            DispensedItemID = dispensedItem;
            NumberDispensed = numDispensed;
            RequiredToolType = reqToolType;
            RequiredToolLevel = reqToolLevel;
            HP = hp;
        }

        public void Interact(Player player, MapScene mapScene)
        {
            if (player.HeldItem is Tool t &&
                t.ObjectID == RequiredToolType &&
                t.ToolUpgradeLevel <= RequiredToolLevel)
            {
                if (HP < t.ToolDamage && !_destroying)
                {
                    _destroying = true;
                    if (AnimationHandler is AnimationManager animation)
                    {
                        animation.UpdateAnimationSequence(2, true);
                        animation.OnAnimationCompleted += () =>
                        {
                            Dispense(mapScene);
                            mapScene.SceneObjects.Remove(this);
                        };
                    }
                }
                else
                {
                    HP -= t.ToolDamage;
                    if (AnimationHandler is AnimationManager animation)
                    {
                        animation.UpdateAnimationSequence(1);
                        animation.OnAnimationCompleted += () =>
                        {
                            animation.UpdateAnimationSequence(0);
                        };
                    }
                }
            }
        }

        public void Dispense(MapScene mapScene)
        {
            var rand = new Random();

            for (int i = 0; i < NumberDispensed; i++)
            {
                var spawned_item = ItemManager.GetItem(DispensedItemID);
                spawned_item.Position = Position + new Vector2(rand.Next(3), rand.Next(3));
                mapScene.SceneObjects.Add(spawned_item);
            }
        }
    }
    public class HarvestableObjectConstructor : GameObjectConstructor
    {
        string DispensedItem { get; set; }
        int NumDispensed { get; set; }
        string ReqToolType {  get; set; }
        int ReqToolLevel {  get; set; }
        int HP { get; set; }
        public HarvestableObjectConstructor(string ID, IAnimator animator, Rectangle? collisionRect, string DispensedItem, int NumDispensed, string ReqToolType, int ReqToolLevel, int HP) : base(ID, animator, collisionRect)
        {
            this.DispensedItem = DispensedItem;
            this.NumDispensed = NumDispensed;
            this.ReqToolType = ReqToolType;
            this.ReqToolLevel = ReqToolLevel;
            this.HP = HP;
        }

        public override GameObject Construct()
        {
            return new HarvestableObject(base.FullID, base.Animator.Copy(), DispensedItem, NumDispensed, ReqToolType, ReqToolLevel, HP, base.CollisionRect);
        }
    }
}
