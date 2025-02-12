using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Entities.GameObjects;
using SkyTown.Map;
using System.Collections.Generic;

namespace SkyTown.LogicManagers
{
    public static class CollisionManager
    {

        static CollisionManager()
        {
        }

        public static void Update(MapScene scene, Player player)
        {
            HandlePlayerMapCollisions(scene.TileMapLayers, player);
            //HandlePlayerNPCCollisions(gameTime, scene.NpcManager, player); Handle this somewhere - need to have some messaging between current scene and game state to get NPCs
            HandlePlayerObjectCollisions(scene.SceneObjects, player);

            player.UpdatePosition();
        }

        public static void  HandlePlayerObjectCollisions(List<GameObject> gameObjects, Player player)
        {
            var futurePlayerRectX = new Rectangle(
                    (int)(player.Position.X + player.Velocity.X + player.CollisionRectangle.Value.X - player.Width / 2),
                    (int)(player.Position.Y + player.CollisionRectangle.Value.Y - player.Height / 2),
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
            foreach ( var obj in gameObjects)
            {
                if (obj.CollisionRectangle is Rectangle hitbox)
                {
                    Rectangle collisionRect = new(
                            (int)obj.Position.X + hitbox.Left - obj.Width / 2,
                            (int)obj.Position.Y + hitbox.Top - obj.Height / 2,
                            hitbox.Width,
                            hitbox.Height
                        );
                    ResolveXSlidingCollision(futurePlayerRectX, collisionRect, player);
                }
            }

            var futurePlayerRectY = new Rectangle(
                    (int)(player.Position.X + player.CollisionRectangle.Value.X - player.Width / 2),
                    (int)(player.Position.Y + player.Velocity.Y + player.CollisionRectangle.Value.Y - player.Height / 2),
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
            foreach (var obj in gameObjects)
            {
                if (obj.CollisionRectangle is Rectangle hitbox)
                {
                    Rectangle collisionRect = new(
                            (int)obj.Position.X + hitbox.Left - obj.Width / 2,
                            (int)obj.Position.Y + hitbox.Top - obj.Height / 2,
                            hitbox.Width,
                            hitbox.Height
                        );
                    ResolveYSlidingCollision(futurePlayerRectY, collisionRect, player);
                }
            }
        }

        public static void HandlePlayerMapCollisions(List<Dictionary<Vector2, string>> TileMapLayers, Player player)
        {
            foreach (Dictionary<Vector2, string> TileMapLayer in TileMapLayers)
            {
                var futurePlayerRectX = new Rectangle(
                    (int)(player.Position.X + player.Velocity.X + player.CollisionRectangle.Value.X - player.Width / 2),
                    (int)(player.Position.Y + player.CollisionRectangle.Value.Y - player.Height / 2),
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
                List<Vector2> localCollidablesX = GetCollidableTilesAroundPlayer(futurePlayerRectX, TileMapLayer);
                foreach (var tilePos in localCollidablesX)
                {
                    if (TileManager.GetTile(TileMapLayer[tilePos]).CollisionRectangle is Rectangle tileHitbox)
                    {

                        Rectangle collisionRect = new(
                            (int)tilePos.X * TileManager.BASE_TILESIZE + tileHitbox.Left - TileManager.BASE_TILESIZE / 2,
                            (int)tilePos.Y * TileManager.BASE_TILESIZE + tileHitbox.Top - TileManager.BASE_TILESIZE / 2,
                            tileHitbox.Width,
                            tileHitbox.Height
                        );

                        ResolveXSlidingCollision(futurePlayerRectX, collisionRect, player);
                    }
                }

                var futurePlayerRectY = new Rectangle(
                    (int)(player.Position.X + player.CollisionRectangle.Value.X - player.Width / 2),
                    (int)(player.Position.Y + player.Velocity.Y + player.CollisionRectangle.Value.Y - player.Height / 2),
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
                List<Vector2> localCollidablesY = GetCollidableTilesAroundPlayer(futurePlayerRectY, TileMapLayer);
                foreach (var tilePos in localCollidablesY)
                {
                    if (TileManager.GetTile(TileMapLayer[tilePos]).CollisionRectangle is Rectangle tileHitbox)
                    {

                        Rectangle collisionRect = new(
                            (int)tilePos.X * TileManager.BASE_TILESIZE + tileHitbox.Left - TileManager.BASE_TILESIZE / 2,
                            (int)tilePos.Y * TileManager.BASE_TILESIZE + tileHitbox.Top - TileManager.BASE_TILESIZE / 2,
                            tileHitbox.Width,
                            tileHitbox.Height
                        );

                        ResolveYSlidingCollision(futurePlayerRectY, collisionRect, player);
                    }
                }
            }
        }

        public static void HandlePlayerNPCCollisions(GameTime gameTime, NPCManager npcManager, Player player)
        {
            var futurePlayerRectX = new Rectangle(
                (int)(player.Position.X + player.Velocity.X) - TileManager.BASE_TILESIZE / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)player.Position.Y - TileManager.BASE_TILESIZE / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (NPC npc in npcManager.NPCs)
            {
                Rectangle collisionRect = new(
                    (int)npc.Position.X - npc.Width / 2 + TileManager.BASE_TILESIZE / 2,
                    (int)npc.Position.Y - npc.Height / 2 + TileManager.BASE_TILESIZE / 2,
                    npc.Width,
                    npc.Height
                );

                //Is Sliding Collision for Player
                if (futurePlayerRectX.Intersects(collisionRect))
                {
                    ResolveXSlidingCollision(futurePlayerRectX, collisionRect, player);
                }
            }

            var futurePlayerRectY = new Rectangle(
                (int)player.Position.X - TileManager.BASE_TILESIZE / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)(player.Position.Y + player.Velocity.Y) - TileManager.BASE_TILESIZE / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (NPC npc in npcManager.NPCs)
            {
                Rectangle collisionRect = new(
                    (int)npc.Position.X - npc.Width / 2 + TileManager.BASE_TILESIZE / 2,
                    (int)npc.Position.Y - npc.Height / 2 + TileManager.BASE_TILESIZE / 2,
                    npc.Width,
                    npc.Height
                );

                //Is Sliding Collision for Player
                if (futurePlayerRectY.Intersects(collisionRect))
                {
                    ResolveYSlidingCollision(futurePlayerRectY, collisionRect, player);
                }
            }
        }


        #region Sliding Collision Resolvers
        private static void ResolveYSlidingCollision(Rectangle futurePlayerRectY, Rectangle collisionRect, Player player)
        {
            while (futurePlayerRectY.Intersects(collisionRect))
            {
                if (player.Velocity.Y < 1 && player.Velocity.Y > -1)
                {
                    player.Velocity.Y = 0;
                    break;
                }
                //Check player direction
                if (player.Velocity.Y > 0)
                {
                    futurePlayerRectY.Y -= 1;
                    player.Velocity.Y -= 1;
                }
                else if (player.Velocity.Y < 0)
                {
                    futurePlayerRectY.Y += 1;
                    player.Velocity.Y += 1;
                }
            }
        }

        private static void ResolveXSlidingCollision(Rectangle futurePlayerRectX, Rectangle collisionRect, Player player)
        {
            while (futurePlayerRectX.Intersects(collisionRect))
            {
                if (player.Velocity.X < 1 && player.Velocity.X > -1)
                {
                    player.Velocity.X = 0;
                    break;
                }
                //Check player direction
                if (player.Velocity.X > 0)
                {
                    futurePlayerRectX.X -= 1;
                    player.Velocity.X -= 1;
                }
                else if (player.Velocity.X < 0)
                {
                    futurePlayerRectX.X += 1;
                    player.Velocity.X += 1;
                }
            }
        }
        #endregion

        #region Collision Tile Identification
        private static List<Vector2> GetCollidableTilesAroundPlayer(Rectangle playerRect, Dictionary<Vector2, string> collisionMap)
        {
            List<Vector2> collidables = new();


            int leftTile = (int)(playerRect.Left / TileManager.BASE_TILESIZE);
            int rightTile = (int)(playerRect.Right / TileManager.BASE_TILESIZE);
            int topTile = (int)(playerRect.Top / TileManager.BASE_TILESIZE);
            int bottomTile = (int)(playerRect.Bottom / TileManager.BASE_TILESIZE);

            // Loop through the bounding box to collect collidable tiles
            for (int x = leftTile; x <= rightTile + 1; x++)
            {
                for (int y = topTile; y <= bottomTile + 1; y++)
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
        #endregion
    }
}
