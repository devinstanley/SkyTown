using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkyTown.Map
{
    internal class MapTile
    {
        public Rectangle sourceRectangle;
        public bool Collide {  get; set; }

        public MapTile(Point tileSheetLoc, Point tileSheetDim)
        {
            sourceRectangle = new Rectangle(tileSheetLoc, tileSheetDim);
            Collide = false;
        }
    }
}
