using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Tool : Item
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        public Tool(string ID, AnimationManager animation) : base(ID, 1, animation)
        {

        }

        public void Interact(Player player)
        {

        }
    }
}
