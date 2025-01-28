using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Entities.Base
{
    public interface IAnimationProvider
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Vector2 position, float scale = -1);
    }

    public class AnimationManager : IAnimationProvider
    {
        private readonly Dictionary<object, Animation> _animations = new();
        private object CurrentAnimationKey;
        private object PreviousAnimationKey;

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            PreviousAnimationKey = key; //Ensures previous animation key exists as fall back
        }

        public void UpdateAnimationSequence(object key)
        {
            if (_animations.ContainsKey(key))
            {
                PreviousAnimationKey = CurrentAnimationKey;
                CurrentAnimationKey = key;
            }
            else
            {
                _animations[PreviousAnimationKey].Reset();
                PreviousAnimationKey = CurrentAnimationKey;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_animations.ContainsKey(key))
            {
                _animations[CurrentAnimationKey].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        {
            _animations[PreviousAnimationKey].Draw(spriteBatch, pos, scale);
        }
    }

    public class Animation : IAnimationProvider
    {
        private readonly string _textureID;
        private readonly List<Rectangle> FrameSources = new();
        private readonly int TotalFrames;
        private int CurrentFrame;
        private readonly double TimePerFrame;
        private double RemainingTimeOnFrame;
        private bool IsAnimated = true;
        public int CurrentWidth { get; private set; }
        public int CurrentHeight { get; private set; }

        public Animation(string textureID, int frameTime, List<Rectangle> frameSources)
        {
            _textureID = textureID;
            TimePerFrame = frameTime;
            RemainingTimeOnFrame = TimePerFrame;
            FrameSources = frameSources;
            TotalFrames = frameSources.Count();

            if (TotalFrames == 1)
            {
                IsAnimated = false;
            }
        }

        public void Reset()
        {
            CurrentFrame = 0;
            RemainingTimeOnFrame = TimePerFrame;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsAnimated)
            {
                return;
            }

            RemainingTimeOnFrame -= gameTime.ElapsedGameTime.TotalSeconds;
            if (RemainingTimeOnFrame < 0)
            {
                RemainingTimeOnFrame = TimePerFrame;
                CurrentFrame = (CurrentFrame + 1) % (TotalFrames);
                CurrentHeight = FrameSources[CurrentFrame].Height;
                CurrentWidth = FrameSources[CurrentFrame].Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        {
            spriteBatch.Draw(
                ResourceManager.LoadTexture(_textureID),
                pos,
                FrameSources[CurrentFrame],
                Color.White, 0f, new Vector2(CurrentWidth / 2, CurrentHeight / 2),
                scale < 0 ? 1 : scale, //Draw as normal scale unless specified
                SpriteEffects.None,
                0f
            );
        }
    }
}
