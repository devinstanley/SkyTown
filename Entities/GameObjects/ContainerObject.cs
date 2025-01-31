using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.GameObjects
{
    internal class ContainerObject : GameObject, IInteractor
    {
        public ContainerObject(string ID) : base(ID)
        {

        }
        public void Interact(Player player)
        {

        }
    }
}
