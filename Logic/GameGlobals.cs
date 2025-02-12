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
        public static Point InGameResolution = new Point(480, 270);

        public static void Update(GameTime gameTime)
        {
            ElapsedGameTime = gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
