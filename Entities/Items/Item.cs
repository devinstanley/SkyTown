using SkyTown.Entities.Base;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.Items
{
    public class Item : GameObject, IInteractor
    {
        public int MaxStack;
        public Item(string id) : base(id)
        {

        }

        public void Interact(Player player)
        {

        }
    }
}
