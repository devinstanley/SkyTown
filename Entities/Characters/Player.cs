using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Base;
using SkyTown.LogicManagers;
using System;

namespace SkyTown.Entities.Characters
{
    public class Player : GameObject
    {
        public InventoryManager inventory;

        //Movement Related Variables
        float RunningSpeed = 1.2f;
        public Vector2 vel;

        private Vector2 _minPos, _maxPos;
        public Player(string ID) : base(ID)
        {
            inventory = new InventoryManager();
        }

        new public void LoadContent(ContentManager content)
        {
            inventory.LoadContent(content);
        }

        public void SetBounds(Point mapSize, Point tileSize)
        {
            _minPos = new Vector2(tileSize.X / 2, tileSize.X / 2);
            _maxPos = new Vector2(mapSize.X * tileSize.X - tileSize.X / 2, mapSize.Y * tileSize.Y - tileSize.X / 2);
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (input == null)
            {
                return;
            }

            //Handle Input Update
            int displacementScalar = (int)(RunningSpeed * gameTime.ElapsedGameTime.TotalSeconds);
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
                _maxPos + new Vector2((Width - HitboxWidth) / 2, (Height - HitboxHeight) / 2)
            );
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Draw platyer current held item
            if (inventory.CurrentItem != null)
            {
                inventory.CurrentItem.Draw(spriteBatch, new Vector2(Position.X - Width / 4, Position.Y - Height / 4), 0.25f);
            }
        }

        public void UpdateVelocity(InputManager input)
        {
            vel = Vector2.Zero;

            //Update Y Velocity and AnimationState
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

            //Update X Velocity and AnimationState
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

            //Normalize Vector if it is Nonzero
            if (!vel.Equals(Vector2.Zero))
            {
                vel.Normalize();
            }

            //If Running Increase Change AnimationState
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
