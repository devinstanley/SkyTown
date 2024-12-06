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

    public enum CollisionSide
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8
    }

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

        public void HandlePlayerMapCollisions(Vector2 displacement)
        {
            var futPlayerPos = player.Position + displacement;
            var futPlayerRect = new Rectangle(
                (int)futPlayerPos.X - tileSize / 2,
                (int)futPlayerPos.Y - tileSize / 2,
                player.Width,
                player.Height
            );

            List<Vector2> localCollidables = GetCollidableTilesAroundPlayer(futPlayerRect);
            foreach (var tilePos in localCollidables)
            {
                Rectangle collisionRect = new(
                    (int)tilePos.X * tileSize,
                    (int)tilePos.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (futPlayerRect.Intersects(collisionRect))
                {
                    player.vel = new Vector2(0, 0);
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

        public void HandleXPlayerMapCollisions(List<Vector2> localCollidables, Rectangle playerRect)
        {
            foreach (Vector2 tilePos in localCollidables)
            {
                Rectangle collisionRect = new(
                    (int)tilePos.X * tileSize,
                    (int)tilePos.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (playerRect.Intersects(collisionRect))
                {
                    if (playerRect.Right > collisionRect.Left && playerRect.Left < collisionRect.Left)
                    {
                        // Player is colliding from the left
                        player.Position.X = collisionRect.Left - player.Width / 2 - tileSize/2;
                    }
                    else if (playerRect.Left < collisionRect.Right && playerRect.Right > collisionRect.Right)
                    {
                        // Player is colliding from the right
                        player.Position.X = collisionRect.Right + player.Width / 2 - tileSize/2;
                    }

                    // Update player rectangle after position adjustment
                    playerRect.X = (int)player.Position.X - tileSize/2;
                }
            }
        }

        public void HandleYPlayerMapCollisions(List<Vector2> localCollidables, Rectangle playerRect)
        {
            foreach (Vector2 tilePos in localCollidables)
            {
                Rectangle collisionRect = new(
                    (int)tilePos.X * tileSize,
                    (int)tilePos.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (playerRect.Intersects(collisionRect))
                {
                    if (playerRect.Bottom > collisionRect.Top && playerRect.Top < collisionRect.Top)
                    {
                        // Player is colliding from the top
                        player.Position.Y = collisionRect.Top - player.Height / 2 - tileSize/2;
                    }
                    else if (playerRect.Top < collisionRect.Bottom && playerRect.Bottom > collisionRect.Bottom)
                    {
                        // Player is colliding from the bottom
                        player.Position.Y = collisionRect.Bottom + player.Height / 2 - tileSize/2;
                    }

                    // Update player rectangle after position adjustment
                    playerRect.Y = (int)player.Position.Y - tileSize/2;
                }
            }
        }
    }
}
