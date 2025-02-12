using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{
    public static class ItemManager
    {
        //
        static Dictionary<string, ItemConstructor> ItemManifest = new();
        static ItemManager()
        {
            ItemManifest = ResourceManager.LoadItems($"Assets\\Items\\ItemsManifest");
        }

        public static Item GetItem(string ItemID)
        {
            return ItemManifest[ItemID].Construct();
        }
    }

    public class ItemConstructor
    {
        public string FullID { get; set; }
        public IAnimator Animator { get; set; }
        public Animation InventoryAnimation { get; set; }
        public int MaxStack { get; set; }

        public ItemConstructor(string FullID, IAnimator Animator, int MaxStack, Animation InventoryAnimation)
        {
            this.FullID = FullID;
            this.Animator = Animator;
            this.MaxStack = MaxStack;
            this.InventoryAnimation = InventoryAnimation;
        }

        public virtual Item Construct()
        {
            return new Item(FullID, MaxStack, Animator.Copy(), InventoryAnimation);
        }
    }
}
