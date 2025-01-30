using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.Items
{
    public class HarvestableObject : GameObject, IInteractor
    {
        public bool DirectHarvest { get; set; }
        public int HarvestCount { get; set; } //HP of Sorts
        public HarvestableObject(string id) : base(id)
        {

        }

        public HarvestableObject(string id, bool directHarvest, int harvestCount): base(id)
        {
            DirectHarvest = directHarvest;
            HarvestCount = harvestCount;
        }

        public void Interact(Player player)
        {

        }
    }
}
