using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using SkyTown.Map;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Tool : Item
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        public int ToolDamage;
        public Tool(string ID, IAnimator animation, IAnimator inventoryAnimation, string toolType, int toolUpgradeLevel, int toolDmg) : base(ID, 1, animation, inventoryAnimation)
        {
            ToolType = toolType;
            ToolUpgradeLevel = toolUpgradeLevel;
            ToolDamage = toolDmg;
        }

        public void Interact(Player player, MapScene mapScene)
        {
            
        }
    }

    public class ToolConstructor: ItemConstructor
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        public int ToolDamage;
        
        public ToolConstructor(string ID, IAnimator animation, IAnimator inventoryAnimation, string toolType, int toolUpgradeLevel, int toolDmg): base(ID, animation, 1, inventoryAnimation)
        {
            ToolType = toolType;
            ToolUpgradeLevel = toolUpgradeLevel;
            ToolDamage = toolDmg;
        }

        public override Tool Construct()
        {
            return new Tool(base.FullID, base.Animator.Copy(), base.InventoryAnimation.Copy(), ToolType, ToolUpgradeLevel, ToolDamage);
        }
    }
}
