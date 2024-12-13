using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.LogicManagers
{
    public class CollisionManager
    {
        Player player;
        NPCManager npcManager;
        Dictionary<Vector2, int> collisionMap;
        int tileSize;

        public CollisionManager(Player player, NPCManager npcManager, Dictionary<Vector2, int> collisionMap, int tileSize)
        {
            this.player = player;
            this.npcManager = npcManager;
            this.collisionMap = collisionMap;
            this.tileSize = tileSize;
        }

        public void HandlePlayerMapCollisions()
        {
            var futurePlayerRectX = new Rectangle(
                (int)(player.Position.X + player.vel.X) - tileSize / 2 + player.Width/2 - player.HitboxWidth/2 + (int)player.hitboxOffset.X,
                (int)player.Position.Y - tileSize / 2 + player.Height/2 - player.HitboxHeight / 2 + (int)player.hitboxOffset.Y,
                player.HitboxWidth,
                player.HitboxHeight
            );
            List<Vector2> localCollidablesX = GetCollidableTilesAroundPlayer(futurePlayerRectX);
            foreach (var tilePos in localCollidablesX)
            {
                Rectangle collisionRect = new(
                    (int)tilePos.X * tileSize,
                    (int)tilePos.Y * tileSize,
                    tileSize,
                    tileSize
                );

                while (futurePlayerRectX.Intersects(collisionRect))
                {
                    if (player.vel.X < 1 && player.vel.X > -1)
                    {
                        player.vel.X = 0;
                        break;
                    }
                    //Check player direction
                    if (player.vel.X > 0)
                    {
                        futurePlayerRectX.X -= 1;
                        player.vel.X -= 1;
                    }
                    else if (player.vel.X < 0)
                    {
                        futurePlayerRectX.X += 1;
                        player.vel.X += 1;
                    }
                }
            }

            var futurePlayerRectY = new Rectangle(
                (int)player.Position.X - tileSize / 2 + player.Width / 2 - player.HitboxWidth/ 2 + (int)player.hitboxOffset.X,
                (int)(player.Position.Y + player.vel.Y) - tileSize / 2 + player.Height / 2 - player.HitboxHeight/ 2 + (int)player.hitboxOffset.Y,
                player.HitboxWidth,
                player.HitboxHeight
            );
            List<Vector2> localCollidablesY = GetCollidableTilesAroundPlayer(futurePlayerRectY);
            foreach (var tilePos in localCollidablesY)
            {
                Rectangle collisionRect = new(
                    (int)tilePos.X * tileSize,
                    (int)tilePos.Y * tileSize,
                    tileSize,
                    tileSize
                );

                while (futurePlayerRectY.Intersects(collisionRect))
                {
                    if (player.vel.Y < 1 && player.vel.Y > -1)
                    {
                        player.vel.Y = 0;
                        break;
                    }
                    //Check player direction
                    if (player.vel.Y > 0)
                    {
                        futurePlayerRectY.Y -= 1;
                        player.vel.Y -= 1;
                    }
                    else if (player.vel.Y < 0)
                    {
                        futurePlayerRectY.Y += 1;
                        player.vel.Y += 1;
                    }
                }
            }
        }

        private List<Vector2> GetCollidableTilesAroundPlayer(Rectangle playerRect)
        {
            List<Vector2> collidables = new();


            int leftTile = (int)(playerRect.Left / tileSize);
            int rightTile = (int)(playerRect.Right / tileSize);
            int topTile = (int)(playerRect.Top / tileSize);
            int bottomTile = (int)(playerRect.Bottom / tileSize);

            // Loop through the bounding box to collect collidable tiles
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    Vector2 tilePosition = new(x, y);

                    // Check if the tile exists in the collision map
                    if (collisionMap.ContainsKey(tilePosition))
                    {
                        collidables.Add(tilePosition);
                    }
                }
            }

            return collidables;
        }
    }
}
