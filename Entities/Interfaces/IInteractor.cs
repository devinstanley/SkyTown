using SkyTown.Entities.Characters;

namespace SkyTown.Entities.Interfaces
{
    public interface IInteractor
    {
        public abstract void Interact(Player player);
    }

    public interface IHarvestable : IInteractor
    {
        public void Harvest();
    }

    public interface IGrowable : IInteractor
    {
        public void Grow();
    }
}
