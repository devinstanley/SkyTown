using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.GameObjects
{
    public class HarvestableObject : GameObject, IInteractor
    {
        public bool DirectHarvest { get; set; }
        public string RequiredToolType { get; set; }
        public int RequiredToolUpgrade { get; set; }    
        public int HarvestCount { get; set; } //HP of Sorts
        public Item DroppedItem { get; set; }
        public HarvestableObject(string id, string dropped_item_id) : base(id)
        {

        }

        public HarvestableObject(string id, int harvestCount) : base(id)
        {
            DirectHarvest = harvestCount == -1;
            HarvestCount = harvestCount;
        }

        public void Interact(Player player)
        {
            if (HarvestCount == -1)
            {
                Harvest(player);
            }
            else
            {
                Tool HeldTool = player.inventory.CurrentItem as Tool;
                if (HeldTool != null)
                {
                    if (RequiredToolType == HeldTool.ToolType && RequiredToolUpgrade == HeldTool.ToolUpgradeLevel)
                    {
                        HarvestCount =- HeldTool.ToolUpgradeLevel;
                    }
                }
                else if (RequiredToolType == null)
                {

                }
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
