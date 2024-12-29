using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Base;
using SkyTown.Entities.Items;
using SkyTown.Logic;
using SkyTown.LogicManagers;

namespace SkyTown.Entities.Characters
{
    public class Player : NPC
    {
        public InventoryManager inventory;
        float RunningSpeed = 1.2f;
        public Vector2 hitboxOffset = new(0, 16);
        public int HitboxWidth = 24;
        public int HitboxHeight = 32;
        
        private Vector2 _minPos, _maxPos; 
        public Player(string ID) : base(ID)
        {
            string testItemID = "Assets.Sprites.TestItem";
            inventory = new();
            inventory.AddItem(new Item(testItemID));
            inventory.AddItem(new Item(testItemID));
            inventory.AddItem(new Item(testItemID));
            inventory.AddItem(new Item(testItemID));
        }

        new public void LoadContent(ContentManager content)
        {
            animationManager.AddAnimation(NPCState.IdleForward, new Animation(ID, 9, 4, 1, 3, [2, 3, 4, 5, 6, 7, 8]));
            animationManager.AddAnimation(NPCState.IdleLeft, new Animation(ID, 9, 4, 1, 2, [1, 2, 3, 4, 5, 6, 7]));
            animationManager.AddAnimation(NPCState.IdleBackward, new Animation(ID, 9, 4, 1, 1, [1, 2, 3, 4, 5, 6, 7]));
            animationManager.AddAnimation(NPCState.IdleRight, new Animation(ID, 9, 4, 1, 4, [1, 2, 3, 4, 5, 6, 7]));

            animationManager.AddAnimation(NPCState.WalkForward, new Animation(ID, 9, 4, 0.1f, 3));
            animationManager.AddAnimation(NPCState.WalkLeft, new Animation(ID, 9, 4, 0.1f, 2));
            animationManager.AddAnimation(NPCState.WalkBackward, new Animation(ID, 9, 4, 0.1f, 1));
            animationManager.AddAnimation(NPCState.WalkRight, new Animation(ID, 9, 4, 0.1f, 4));

            animationManager.AddAnimation(NPCState.RunForward, new Animation(ID, 9, 4, 0.1f, 3, [0, 2, 4, 6, 8]));
            animationManager.AddAnimation(NPCState.RunLeft, new Animation(ID, 9, 4, 0.1f, 2, [0, 2, 4, 6, 8]));
            animationManager.AddAnimation(NPCState.RunBackward, new Animation(ID, 9, 4, 0.1f, 1, [0, 2, 4, 6, 8]));
            animationManager.AddAnimation(NPCState.RunRight, new Animation(ID, 9, 4, 0.1f, 4, [0, 2, 4, 6, 8]));

            inventory.LoadContent(content);
        }

        public void SetBounds(Point mapSize, Point tileSize)
        {
            _minPos = new Vector2(tileSize.X / 2, tileSize.X / 2);
            _maxPos = new Vector2(mapSize.X * tileSize.X - tileSize.X / 2, mapSize.Y * tileSize.Y - tileSize.X / 2);
        }

        public void Update(GameTime gameTime, InputManager input, CollisionManager collisionManager)
        {
            if (input == null)
            {
                return;
            }

            //Handle Input Update
            int displacementScalar = (int)(_speed * gameTime.ElapsedGameTime.TotalSeconds);
            UpdateVelocity(input);
            vel *= displacementScalar;

            base.Update(gameTime);
        }

        public void UpdatePosition()
        {
            //Update Player Position
            Position += vel;
            Position = Vector2.Clamp(Position, 
                _minPos - new Vector2((Width - HitboxWidth) / 2, (Height - HitboxHeight) / 2), 
                _maxPos + new Vector2((Width - HitboxWidth) / 2, (Height - HitboxHeight) / 2));
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Draw platyer current held item
            if (inventory.CurrentItem != null)
            {
                inventory.CurrentItem.Draw(spriteBatch, new Vector2(Position.X - Width/4, Position.Y - Height/4), 0.25f);
            }
        }

        public void UpdateVelocity(InputManager input)
        {
            vel = Vector2.Zero;

            if (input.IsKeyDown(Keys.W))
            {
                vel.Y -= 1;
                AnimationState = NPCState.WalkBackward;
            }
            else if (input.IsKeyDown(Keys.S))
            {
                vel.Y += 1;
                AnimationState = NPCState.WalkForward;
            }
            if (input.IsKeyDown(Keys.A))
            {
                vel.X -= 1;
                AnimationState = NPCState.WalkLeft;
            }
            else if (input.IsKeyDown(Keys.D))
            {
                vel.X += 1;
                AnimationState = NPCState.WalkRight;
            }

            if (!vel.Equals(Vector2.Zero))
            {
                vel.Normalize();
            }
            if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
            {
                AnimationState += 4;
                vel *= RunningSpeed;
            }
            //Transition To Idle in Same Direction
            if (vel.Equals(Vector2.Zero))
            {
                AnimationState = (NPCState)((int)AnimationState % 4);
            }
            
        }
    }
}
