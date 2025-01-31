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
    internal class ContainerObject: GameObject, IInteractor
    {
        public ContainerObject(string ID) : base(ID)
        {

        }
        public void Interact(Player player)
        {

        }
    }
}
