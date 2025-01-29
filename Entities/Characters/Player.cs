using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Base;
using SkyTown.Entities.Interfaces;
using SkyTown.LogicManagers;
using System;

namespace SkyTown.Entities.Characters
{
    public class Player : GameObject
    {
        //TODO: FIX ANIMATION LOGIC
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

            //Needs to be responsible for loading spritesheet, animation register, inventory status?
            
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
                _minPos - new Vector2(Width, Height),
                _maxPos + new Vector2(Width, Height)
            );
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Draw platyer current held item
            if (inventory.CurrentItem != null)
            {
                inventory.CurrentItem.Draw(spriteBatch, new Vector2(Position.X, Position.Y), 0.25f);
            }
        }

        //Update animation here depending on velocity and held item
        public void UpdateAnimation()
        {

        }

        public void UpdateVelocity(InputManager input)
        {
            vel = Vector2.Zero;
            var animationManager = (AnimationManager)AnimationHandler;

            //Update Y Velocity and AnimationState
            if (input.IsKeyDown(Keys.W))
            {
                vel.Y -= 1;
                animationManager.UpdateAnimationSequence(0); //Walk Away
            }
            else if (input.IsKeyDown(Keys.S))
            {
                vel.Y += 1;
                animationManager.UpdateAnimationSequence(1); //Walk Toward
            }

            //Update X Velocity and AnimationState
            if (input.IsKeyDown(Keys.A))
            {
                vel.X -= 1;
                animationManager.UpdateAnimationSequence(2); //Walk Left
            }
            else if (input.IsKeyDown(Keys.D))
            {
                vel.X += 1;
                animationManager.UpdateAnimationSequence(3); //Walk Right
            }

            //Normalize Vector if it is Nonzero
            if (!vel.Equals(Vector2.Zero))
            {
                vel.Normalize();
            }

            //If Running Increase Change AnimationState
            if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
            {
                vel *= RunningSpeed;
            }
            //Transition To Idle in Same Direction
            if (vel.Equals(Vector2.Zero))
            {
                animationManager.UpdateAnimationSequence(4); //Stationary
            }
        }
    }
}
