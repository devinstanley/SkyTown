using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.Interfaces;
using System.Collections.Generic;

namespace SkyTown.Entities.GameObjects.Items
{
    public class Item : GameObject, IInteractor
    {
        public int MaxStack;
        public Item(string id, int maxStack, IAnimator animation) : base(id)
        {
            MaxStack = maxStack;
            AnimationHandler = animation;
        }

        public void Interact(Player player)
        {

        }
    }
}
