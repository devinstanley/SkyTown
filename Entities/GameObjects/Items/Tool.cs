using SkyTown.Entities.Characters;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Tool : Item
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        public Tool(string ID) : base(ID, 1)
        {

        }

        public void Interact(Player player)
        {

        }
    }
}
