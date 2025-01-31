using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Item : GameObject, IInteractor
    {
        public int MaxStack;
        public Item(string id, int maxStack) : base(id)
        {
            MaxStack = maxStack;
            Animation animation = new Animation(id.Split("::")[0], 1, new List<Rectangle>([new Rectangle(0, 0, 32, 32)]));
            AnimationHandler = animation;
        }

        public void Interact(Player player)
        {

        }
    }
}
