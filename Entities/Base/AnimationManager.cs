using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.Base
{
    public class AnimationManager
    {
        private readonly Dictionary<object, Animation> _animations = new();
        private object _lastKey;
        public int AnimationWidth {get;set;}
        public int AnimationHeight {get;set; }
        public bool DEBUG_COLLISIONS;

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            _lastKey = key;
        }

        public void Update(object key, GameTime gameTime)
        {
            if (_animations.ContainsKey(key))
            {
                _animations[key].Start();
                _animations[key].Update(gameTime);
                AnimationWidth = _animations[key].CurrentWidth;
                AnimationHeight = _animations[key].CurrentHeight;
                _lastKey = key;
            }
            else
            {
                if (_lastKey == null)
                {
                    return;
                }
                _animations[_lastKey].Stop();
                _animations[_lastKey].Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale=-1)
        {
            _animations[_lastKey].Draw(spriteBatch, pos, scale);
        }
    }
}
