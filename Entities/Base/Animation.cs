using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkyTown.Entities.Base
{
    public class Animation
    {
        private readonly Texture2D _texture2D;
        private readonly List<Rectangle> _sourceRects = new();
        private readonly int _frames;
        private int _frame;
        private int _skipFramesLen;
        private readonly double _frameTime;
        private double _frameTimeLeft;
        private bool _active = true;
        public int CurrentWidth { get; private set; }
        public int CurrentHeight { get; private set; }

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1, int[] skipList = null)
        {
            _texture2D = texture;
            _frameTime = frameTime;
            _frameTimeLeft = _frameTime;
            _frames = framesX;

            _skipFramesLen = skipList != null ? skipList.Length : 0;
            var frameWidth = _texture2D.Width / framesX;
            var frameHeight = _texture2D.Height / framesY;
            CurrentHeight = frameHeight;
            CurrentWidth = frameWidth;

            for (int i = 0; i < _frames; i++)
            {
                if (skipList != null && skipList.Contains(i))
                {
                    continue;
                }
                _sourceRects.Add(new Rectangle(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
            }
        }

        public void Stop()
        {
            _active = false;
        }

        public void Start()
        {
            _active |= true;
        }

        public void Reset()
        {
            _frame = 0;
            _frameTimeLeft = _frameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!_active)
            {
                return;
            }

            _frameTimeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTimeLeft < 0)
            {
                _frameTimeLeft = _frameTime;
                _frame = (_frame + 1) % (_frames - _skipFramesLen);
                CurrentHeight = _sourceRects[_frame].Height;
                CurrentWidth = _sourceRects[_frame].Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale = -1)
        { 
            spriteBatch.Draw(
                _texture2D,
                pos,
                _sourceRects[_frame],
                Color.White, 0f, new Vector2(CurrentWidth / 2, CurrentHeight / 2),
                scale < 0 ? 1 : scale, //Draw as normal scale unless specified
                SpriteEffects.None,
                0f
                );
        }
    }
}
