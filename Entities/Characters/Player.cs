using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.LogicManagers;
using SkyTown.Map;
using System.Collections.Generic;

namespace SkyTown.Entities.Characters
{
    public class Player : GameObject
    {
        public InventoryManager inventory;

        //Movement Related Variables
        float WalkingSpeed = 100f;
        float RunningSpeed = 1.2f;
        bool isRunning = false;
        public Vector2 Velocity;
        private Vector2 _minPos, _maxPos;

        private int AnimationSequence;
        private bool AnimationLock = false;

        public Player(string ID) : base(ID)
        {
            inventory = new InventoryManager();
            inventory.AddItem(ItemManager.GetItem("berries"));
            inventory.AddItem(ItemManager.GetItem("pickaxe"));
            CollisionRectangle = new Rectangle(16, 16, 32, 48);
        }

        new public void LoadContent(ContentManager content)
        {
            inventory.LoadContent(content);

            //Needs to be responsible for loading spritesheet, animation register, inventory status?
            AnimationManager animations = new AnimationManager();
            //Idle
            animations.AddAnimation(0, new Animation(TextureID, 1, new List<Rectangle>([new Rectangle(0, 0, 64, 64)])));
            animations.AddAnimation(1, new Animation(TextureID, 1, new List<Rectangle>([new Rectangle(0, 128, 64, 64)])));
            animations.AddAnimation(2, new Animation(TextureID, 1, new List<Rectangle>([new Rectangle(0, 64, 64, 64)])));
            animations.AddAnimation(3, new Animation(TextureID, 1, new List<Rectangle>([new Rectangle(0, 192, 64, 64)])));
            //Walking
            animations.AddAnimation(4, new Animation(TextureID, .2d, new List<Rectangle>([
                new Rectangle(0, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(128*2, 0, 64, 64),
                new Rectangle(128*3, 0, 64, 64)
                ])));
            animations.AddAnimation(5, new Animation(TextureID, .2d, new List<Rectangle>([
                new Rectangle(0, 128, 64, 64),
                new Rectangle(128, 128, 64, 64),
                new Rectangle(128*2, 128, 64, 64),
                new Rectangle(128*3, 128, 64, 64)
                ])));
            animations.AddAnimation(6, new Animation(TextureID, .2d, new List<Rectangle>([
                new Rectangle(0, 64, 64, 64),
                new Rectangle(128, 64, 64, 64),
                new Rectangle(128*2, 64, 64, 64),
                new Rectangle(128*3, 64, 64, 64)
                ])));
            animations.AddAnimation(7, new Animation(TextureID, .2d, new List<Rectangle>([
                new Rectangle(0, 192, 64, 64),
                new Rectangle(128, 192, 64, 64),
                new Rectangle(128*2, 192, 64, 64),
                new Rectangle(128*3, 192, 64, 64)
                ])));
            //Runnin
            animations.AddAnimation(8, new Animation(TextureID, .1d, new List<Rectangle>([
                new Rectangle(0, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(128*2, 0, 64, 64),
                new Rectangle(128*3, 0, 64, 64)
                ])));
            animations.AddAnimation(9, new Animation(TextureID, .1d, new List<Rectangle>([
                new Rectangle(0, 128, 64, 64),
                new Rectangle(128, 128, 64, 64),
                new Rectangle(128*2, 128, 64, 64),
                new Rectangle(128*3, 128, 64, 64)
                ])));
            animations.AddAnimation(10, new Animation(TextureID, .1d, new List<Rectangle>([
                new Rectangle(0, 64, 64, 64),
                new Rectangle(128, 64, 64, 64),
                new Rectangle(128*2, 64, 64, 64),
                new Rectangle(128*3, 64, 64, 64)
                ])));
            animations.AddAnimation(11, new Animation(TextureID, .1d, new List<Rectangle>([
                new Rectangle(0, 192, 64, 64),
                new Rectangle(128, 192, 64, 64),
                new Rectangle(128*2, 192, 64, 64),
                new Rectangle(128*3, 192, 64, 64)
                ])));
            AnimationHandler = animations;
        }

        public void SetBounds(Point mapSize)
        {
            _minPos = new Vector2(-TileManager.BASE_TILESIZE / 2 + Width / 2 - CollisionRectangle.Value.X, -TileManager.BASE_TILESIZE / 2 + Height / 2 - CollisionRectangle.Value.Y);
            _maxPos = new Vector2(
                -TileManager.BASE_TILESIZE / 2 + (mapSize.X + 1) * TileManager.BASE_TILESIZE - Width / 2 + (Width - CollisionRectangle.Value.X - CollisionRectangle.Value.Width),
                -TileManager.BASE_TILESIZE / 2 + (mapSize.Y + 1) * TileManager.BASE_TILESIZE - Height / 2 + (Height - CollisionRectangle.Value.Y - CollisionRectangle.Value.Height)
                );
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
            UpdateAnimation();

            base.Update(gameTime);
        }

        public void UpdatePosition()
        {
            //Update Player Position
            Position += Velocity;
            Position = Vector2.Clamp(Position, _minPos, _maxPos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Draw platyer current held item
            if (inventory.CurrentItem != null)
            {
                inventory.CurrentItem.Draw(spriteBatch, new Vector2(Position.X, Position.Y), 0.25f);
            }
        }

        public void StartSpecialAnimation(int animationRoot)
        {
            int directionKey = AnimationSequence % 4;
            int animationKey = animationRoot + directionKey;
            if (inventory.CurrentItem.AnimationHandler is AnimationManager animator)
            {
                AnimationLock = true;
                animator.UpdateAnimationSequence(animationKey);
            }
            if (AnimationHandler is AnimationManager animationManager)
            {
                animationManager.UpdateAnimationSequence(animationKey);
            }
        }

        //Update animation here depending on velocity and held item
        public void UpdateAnimation()
        {
            //Transition To Idle in Same Direction
            if (Velocity.Equals(Vector2.Zero))
            {
                AnimationSequence = AnimationSequence % 4; //Stationary
            }
            else
            {
                double angle = System.Math.Atan2(Velocity.X, Velocity.Y);

                if (angle <= 3 * MathHelper.PiOver4 + 0.1 && angle >= MathHelper.PiOver4 - 0.1)
                {
                    AnimationSequence = 7;
                }
                else if (angle > -3 * MathHelper.PiOver4 - 0.1 && angle < -MathHelper.PiOver4 + 0.1)
                {
                    AnimationSequence = 6;
                }
                else if (angle > -MathHelper.PiOver4 && angle < MathHelper.PiOver4)
                {
                    AnimationSequence = 5;
                }
                else if (angle < -3 * MathHelper.PiOver4 || angle > 3 * MathHelper.PiOver4)
                {
                    AnimationSequence = 4;
                }

                if (isRunning)
                {
                    AnimationSequence += 4;
                }
            }

            var animationManager = (AnimationManager)AnimationHandler;
            animationManager.UpdateAnimationSequence(AnimationSequence);
        }


        public void UpdateVelocity(InputManager input)
        {
            
            Velocity = Vector2.Zero;
            if (AnimationLock)
            {
                return;
            }

            //Update Y Velocity and AnimationState
            if (input.IsKeyDown(Keys.W))
            {
                Velocity.Y -= 1;
            }
            else if (input.IsKeyDown(Keys.S))
            {
                Velocity.Y += 1;
            }
            //Update X Velocity and AnimationState
            if (input.IsKeyDown(Keys.A))
            {
                Velocity.X -= 1;
            }
            else if (input.IsKeyDown(Keys.D))
            {
                Velocity.X += 1;
            }

            //Normalize Vector if it is Nonzero
            if (!Velocity.Equals(Vector2.Zero))
            {
                Velocity.Normalize();

                //If Running Increase Change AnimationState
                if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
                {
                    Velocity *= RunningSpeed;
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                }
            }
        }
    }
}
