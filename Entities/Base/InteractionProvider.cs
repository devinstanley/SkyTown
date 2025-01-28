using SkyTown.Entities.Characters;

namespace SkyTown.Entities.Base
{
    public interface IInteractionProvider
    {
        public abstract void Interact(Player player);
    }
}
