using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.Items
{
    public class HarvestableObject : GameObject, IInteractor
    {
        public bool DirectHarvest { get; set; }
        public int HarvestCount { get; set; } //HP of Sorts
        public Item DroppedItem { get; set; }
        public HarvestableObject(string id) : base(id)
        {

        }

        public HarvestableObject(string id, int harvestCount): base(id)
        {
            DirectHarvest = harvestCount == -1;
            HarvestCount = harvestCount;
        }

        public void Interact(Player player)
        {
            if (HarvestCount <= 0)
            {
                Harvest(player);
            }
        }

        public void Harvest(Player player)
        {
            if (DirectHarvest)
            {
                player.inventory.AddItem(DroppedItem);
            }
            else
            {
                //Need to add the items to the game scene
            }
        }
    }
}
