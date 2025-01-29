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
    public class Item : GameObject, IInteractor
    {
        public int MaxStack;
        public Item(string id): base(id)
        {

        }

        public void Interact(Player player)
        {

        }
    }
}
