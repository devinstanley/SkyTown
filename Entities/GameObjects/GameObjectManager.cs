using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.Logic;

namespace SkyTown.Entities.GameObjects
{
    public static class GameObjectManager
    {
        //
        static Dictionary<string, GameObjectConstructor> GameObjectManifest = new();
        static GameObjectManager()
        {
            GameObjectManifest = ResourceManager.LoadGameObjects($"Assets\\Items\\GameObjectsManifest");
        }

        public static GameObject GetItem(string ItemID)
        {
            return GameObjectManifest[ItemID].Construct();
        }
    }

    public class GameObjectConstructor
    {
        public string FullID { get; set; }
        public Rectangle? CollisionRect { get; set; }
        public IAnimator Animator { get; set; }

        public GameObjectConstructor(string FullID, IAnimator Animator, Rectangle? CollisionRect)
        {
            this.FullID = FullID;
            this.Animator = Animator;
            this.CollisionRect = CollisionRect;
        }

        public virtual GameObject Construct()
        {
            GameObject obj = new GameObject(FullID, CollisionRect);
            obj.AnimationHandler = Animator.Copy();
            return obj;
        }
}
}
