using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int MaxStack { get; set; }

        public ItemConstructor(string FullID, IAnimator Animator, int MaxStack)
        {
            this.FullID = FullID;
            this.Animator = Animator;
            this.MaxStack = MaxStack;
        }

        public virtual Item Construct()
        {
            return new Item(FullID, MaxStack, Animator);
        }
    }
}
