using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.Items
{
    internal class HarvestableObject : GameObject, IInteractor
    {
        public bool DirectHarvest { get; set; }
        public int HarvestCount { get; set; } //HP of Sorts
        public HarvestableObject(string id) : base(id)
        {

        }

        public void Interact(Player player)
        {

        }
    }
}
