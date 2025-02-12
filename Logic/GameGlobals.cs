using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SkyTown.Logic
{
    public static class GameGlobals
    {
        public static double ElapsedGameTime;

        public static void Update(GameTime gameTime)
        {
            ElapsedGameTime = gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
