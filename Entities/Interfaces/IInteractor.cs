using SkyTown.Entities.Characters;
using SkyTown.Map;

namespace SkyTown.Entities.Interfaces
{
    public interface IInteractor
    {
        public abstract void Interact(Player player, MapScene mapScene);
    }
}
