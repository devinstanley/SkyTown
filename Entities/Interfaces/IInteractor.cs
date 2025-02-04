using SkyTown.Entities.Characters;

namespace SkyTown.Entities.Interfaces
{
    public interface IInteractor
    {
        public abstract void Interact(Player player);
    }

    public interface IHarvestable : IInteractor
    {
        public string RequiredToolType { get; set; }
        public int ToolUpgradeLevel { get; set; }
        public void Harvest();
    }

    public interface IGrowable : IHarvestable
    {
        public int DaysToGrow { get; set; }
        public bool Renewable { get; set; }

        public void Grow();
    }
}
