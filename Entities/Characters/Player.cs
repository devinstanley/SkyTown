using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.Base;
using SkyTown.Entities.Interfaces;
using SkyTown.LogicManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyTown.Entities.Characters
{
    public class Player : GameObject
    {
        //TODO: FIX ANIMATION LOGIC
        public InventoryManager inventory;

        //Movement Related Variables
        float WalkingSpeed = 100f;
        float RunningSpeed = 1.2f;
        public Vector2 Velocity;
        private Vector2 _minPos, _maxPos;
        private int AnimationSequence;

        public Player(string ID) : base(ID)
        {
            inventory = new InventoryManager();
            CollisionRectangle = new Rectangle(16, 16, 32, 48);
        }

        new public void LoadContent(ContentManager content)
        {
            inventory.LoadContent(content);

            //Needs to be responsible for loading spritesheet, animation register, inventory status?
            AnimationManager animations = new AnimationManager();
            //Idle
            animations.AddAnimation(0, new Animation(ID, 1, new List<Rectangle>([new Rectangle(0, 0, 64, 64)])));
            animations.AddAnimation(1, new Animation(ID, 1, new List<Rectangle>([new Rectangle(0, 128, 64, 64)])));
            animations.AddAnimation(2, new Animation(ID, 1, new List<Rectangle>([new Rectangle(0, 64, 64, 64)])));
            animations.AddAnimation(3, new Animation(ID, 1, new List<Rectangle>([new Rectangle(0, 192, 64, 64)])));
            //Walking
            animations.AddAnimation(4, new Animation(ID, .2d, new List<Rectangle>([
                new Rectangle(0, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(128*2, 0, 64, 64),
                new Rectangle(128*3, 0, 64, 64)
                ])));
            animations.AddAnimation(5, new Animation(ID, .2d, new List<Rectangle>([
                new Rectangle(0, 128, 64, 64),
                new Rectangle(128, 128, 64, 64),
                new Rectangle(128*2, 128, 64, 64),
                new Rectangle(128*3, 128, 64, 64)
                ])));
            animations.AddAnimation(6, new Animation(ID, .2d, new List<Rectangle>([
                new Rectangle(0, 64, 64, 64),
                new Rectangle(128, 64, 64, 64),
                new Rectangle(128*2, 64, 64, 64),
                new Rectangle(128*3, 64, 64, 64)
                ])));
            animations.AddAnimation(7, new Animation(ID, .2d, new List<Rectangle>([
                new Rectangle(0, 192, 64, 64),
                new Rectangle(128, 192, 64, 64),
                new Rectangle(128*2, 192, 64, 64),
                new Rectangle(128*3, 192, 64, 64)
                ])));
            //Runnin
            animations.AddAnimation(8, new Animation(ID, .1d, new List<Rectangle>([
                new Rectangle(0, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(128*2, 0, 64, 64),
                new Rectangle(128*3, 0, 64, 64)
                ])));
            animations.AddAnimation(9, new Animation(ID, .1d, new List<Rectangle>([
                new Rectangle(0, 128, 64, 64),
                new Rectangle(128, 128, 64, 64),
                new Rectangle(128*2, 128, 64, 64),
                new Rectangle(128*3, 128, 64, 64)
                ])));
            animations.AddAnimation(10, new Animation(ID, .1d, new List<Rectangle>([
                new Rectangle(0, 64, 64, 64),
                new Rectangle(128, 64, 64, 64),
                new Rectangle(128*2, 64, 64, 64),
                new Rectangle(128*3, 64, 64, 64)
                ])));
            animations.AddAnimation(11, new Animation(ID, .1d, new List<Rectangle>([
                new Rectangle(0, 192, 64, 64),
                new Rectangle(128, 192, 64, 64),
                new Rectangle(128*2, 192, 64, 64),
                new Rectangle(128*3, 192, 64, 64)
                ])));
            AnimationHandler = animations;
        }

        public void SetBounds(Point mapSize, Point tileSize)
        {
            _minPos = new Vector2(tileSize.X / 2 + Width/2 + 32 - CollisionRectangle.Value.X, tileSize.X / 2 + Height/2 + 32 - CollisionRectangle.Value.Y);
            _maxPos = new Vector2(mapSize.X * tileSize.X - tileSize.X / 2, mapSize.Y * tileSize.Y - tileSize.X / 2);
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (input == null)
            {
                return;
            }

            //Handle Input Update
            float displacementScalar = WalkingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateVelocity(input);
            Velocity *= displacementScalar;

            base.Update(gameTime);
        }

        public void UpdatePosition()
        {
            //Update Player Position
            Position += Velocity;
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
            Velocity = Vector2.Zero;

            //Update Y Velocity and AnimationState
            if (input.IsKeyDown(Keys.W))
            {
                Velocity.Y -= 1;
                AnimationSequence = 4; //Walk Away
            }
            else if (input.IsKeyDown(Keys.S))
            {
                Velocity.Y += 1;
                AnimationSequence = 5; //Walk Toward
            }
            //Update X Velocity and AnimationState
            if (input.IsKeyDown(Keys.A))
            {
                Velocity.X -= 1;
                AnimationSequence = 6; //Walk Left
            }
            else if (input.IsKeyDown(Keys.D))
            {
                Velocity.X += 1;
                AnimationSequence = 7; //Walk Right
            }

            //Normalize Vector if it is Nonzero
            if (!Velocity.Equals(Vector2.Zero))
            {
                Velocity.Normalize();

                //If Running Increase Change AnimationState
                if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
                {
                    Velocity *= RunningSpeed;
                    AnimationSequence += 4;
                }
            }

            //Transition To Idle in Same Direction
            if (Velocity.Equals(Vector2.Zero))
            {
                AnimationSequence = AnimationSequence % 4; //Stationary
            }

            var animationManager = (AnimationManager)AnimationHandler;
            animationManager.UpdateAnimationSequence(AnimationSequence);
        }
    }
}
