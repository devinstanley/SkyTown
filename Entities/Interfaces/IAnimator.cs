using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SkyTown.Entities.Interfaces
{
    public interface IAnimator
    {
        bool AnimationLocked { get; }
        int Height { get; }
        int Width { get; }
        void Update();
        void Draw(SpriteBatch spriteBatch, Vector2 position, float scale = -1);
        IAnimator Copy();
    }

    public class AnimationManager : IAnimator
    {
        private readonly Dictionary<object, Animation> _animations = new();
        private object CurrentAnimationKey;
        private object PreviousAnimationKey;
        public bool AnimationLocked { get { return _animations[CurrentAnimationKey].AnimationLocked; } }

        public int Height
        {
            get { return _animations[CurrentAnimationKey].Height; }
        }
        public int Width
        {
            get { return _animations[CurrentAnimationKey].Width; }
        }

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            CurrentAnimationKey = key;
            PreviousAnimationKey = key; //Ensures previous animation key exists as fall back
        }

        public void UpdateAnimationSequence(object key, bool animationLock = false)
        {
            if (AnimationLocked)
            {
                return;
            }
            if (_animations.ContainsKey(key))
            {
                PreviousAnimationKey = CurrentAnimationKey;
                CurrentAnimationKey = key;
            }
            else
            {
                _animations[PreviousAnimationKey].Reset();
                CurrentAnimationKey = PreviousAnimationKey;
            }

            if (animationLock)
            {
                _animations[CurrentAnimationKey].StartLockedAnimation();
            }
        }

        public void Update()
        {
            if (_animations.ContainsKey(CurrentAnimationKey))
            {
                _animations[CurrentAnimationKey].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        {
            _animations[CurrentAnimationKey].Draw(spriteBatch, pos, scale);
        }

        public IAnimator Copy()
        {
            var animationCopy = new AnimationManager();
            foreach (var anim in _animations)
            {
                animationCopy.AddAnimation(anim.Key, anim.Value.Copy() as Animation);
            }
            return animationCopy;
        }
    }

    public class Animation : IAnimator
    {
        private readonly string TextureID;
        private readonly List<Rectangle> FrameSources = new();
        private readonly int TotalFrames;
        private readonly double TimePerFrame;
        private int CurrentFrame;
        private double RemainingTimeOnFrame;
        private bool IsAnimated = true;

        private double RemainingLock { get; set; }
        private double TotalDuration
        {
            get { return TotalFrames * TimePerFrame; }
        }
        public bool AnimationLocked {  get; set; }
        public int Height
        {
            get { return FrameSources[CurrentFrame].Height; }
        }
        public int Width
        {
            get { return FrameSources[CurrentFrame].Width; }
        }

        public Animation(string textureID, double frameTime, List<Rectangle> frameSources)
        {
            TextureID = textureID;
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

        public void StartLockedAnimation()
        {
            if (IsAnimated)
            {
                Reset();
                AnimationLocked = true;
                RemainingLock = TotalDuration;
            }
        }

        public void Update()
        {
            if (!IsAnimated)
            {
                return;
            }

            if (RemainingLock <= 0)
            {
                AnimationLocked = false;
                RemainingLock = 0;
            }
            else
            {
                RemainingLock -= GameGlobals.ElapsedGameTime;
            }

            RemainingTimeOnFrame -= GameGlobals.ElapsedGameTime;
            if (RemainingTimeOnFrame < 0)
            {
                RemainingTimeOnFrame = TimePerFrame;
                CurrentFrame = (CurrentFrame + 1) % TotalFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        {
            spriteBatch.Draw(
                ResourceManager.LoadTexture(TextureID),
                pos,
                FrameSources[CurrentFrame],
                Color.White, 0f, new Vector2(Width / 2, Height / 2),
                scale < 0 ? 1 : scale, //Draw as normal scale unless specified
                SpriteEffects.None,
                0f
            );
        }

        public IAnimator Copy()
        {
            var newFrameSources = new List<Rectangle>();
            foreach ( var frameSource in FrameSources)
            {
                newFrameSources.Add(
                    new Rectangle(
                        frameSource.X,
                        frameSource.Y,
                        frameSource.Width,
                        frameSource.Height)
                    );
            }
            return new Animation(TextureID, TimePerFrame, newFrameSources);
        }
    }
}
