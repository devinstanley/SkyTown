using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using SkyTown.Logic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpFont.Cache;
using SkyTown.Entities.Items;

namespace SkyTown.Entities.Base
{
    public class Entity
    {
        public string ID { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        protected AnimationManager animationManager = new();
        protected NPCState AnimationState = NPCState.IdleForward;

        //Set default collision and interaction behavior
        public CollisionActionEnum CollisionAction { get; set; } = CollisionActionEnum.SLIDING;
        public HarvestActionEnum HarvestAction { get; set; } = HarvestActionEnum.HAND;


        public Entity(string ID)
        {
            this.ID = ID;
        }

        public void LoadContent(ContentManager content)
        {
            animationManager.AddAnimation(NPCState.IdleForward, new Animation(ID, 1, 1, 1));
        }

        public void Update(GameTime gameTime)
        {
            animationManager.Update(AnimationState, gameTime);

            Width = animationManager.AnimationWidth;
            Height = animationManager.AnimationHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Draw(spriteBatch, Position);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        {
            animationManager.Draw(spriteBatch, pos, scale);
        }


        //Upcast entity to item
        public Item ToItem()
        {
            Item item = new Item(ID);
            item.Position = Position;
            item.AnimationState = AnimationState;
            item.animationManager = animationManager;

            return item;
        }
    }

    public enum CollisionActionEnum{
        PASS,       //Can pass through
        SLIDING,    //Can slide up against
        COLLECT,    //Can be collected
    }

    public enum HarvestActionEnum
    {
        NULL,       //Cannot be harvested
        HAND,       //Can just be picked up and added to inventory
        AXE,        //Harvested with Axe
        PICKAXE,    //Harvested with Pickaxe
    }
}
