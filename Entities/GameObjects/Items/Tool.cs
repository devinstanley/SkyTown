using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Tool : Item
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        public Tool(string ID, IAnimator animation, string toolType, int toolUpgradeLevel) : base(ID, 1, animation)
        {
            AnimationHandler = animation;
            ToolType = toolType;
            ToolUpgradeLevel = toolUpgradeLevel;
        }

        public void Interact(Player player)
        {
            if (AnimationHandler is AnimationManager animation)
            {
                //animation.UpdateAnimationSequence()
            }
        }
    }

    public class ToolConstructor: ItemConstructor
    {
        public string ToolType;
        public int ToolUpgradeLevel;
        
        public ToolConstructor(string ID, IAnimator animation, string toolType, int toolUpgradeLevel): base(ID, animation, 1)
        {
            ToolType = toolType;
            ToolUpgradeLevel = toolUpgradeLevel;
        }

        public override Tool Construct()
        {
            return new Tool(base.FullID, base.Animator, ToolType, ToolUpgradeLevel);
        }
    }
}
