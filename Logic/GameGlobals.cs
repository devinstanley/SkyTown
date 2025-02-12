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
