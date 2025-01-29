using Microsoft.Xna.Framework;
using SkyTown.Entities.Characters;
using SkyTown.Map;
using System.Collections.Generic;

namespace SkyTown.LogicManagers
{
    public class CollisionManager
    {
        int tileSize = 32;

        public CollisionManager()
        {
        }

        public void Update(GameTime gameTime, MapScene scene, Player player)
        {
            HandlePlayerMapCollisions(gameTime, scene.TileMapLayers, player);
            //HandlePlayerNPCCollisions(gameTime, scene.NpcManager, player); Handle this somewhere - need to have some messaging between current scene and game state to get NPCs
            HandlePlayerEntityCollisions(gameTime, scene.SceneEntities, player);

            player.UpdatePosition();
        }

        public void HandlePlayerMapCollisions(GameTime gameTime, List<Dictionary<Vector2, string>> TileMapLayers, Player player)
        {
            foreach (Dictionary<Vector2, string> TileMapLayer in TileMapLayers)
            {
                var futurePlayerRectX = new Rectangle(
                    (int)(player.Position.X + player.vel.X) - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                    (int)player.Position.Y - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
                List<Vector2> localCollidablesX = GetCollidableTilesAroundPlayer(futurePlayerRectX, TileMapLayer);
                foreach (var tilePos in localCollidablesX)
                {
                    if (TileManager.GetTile(TileMapLayer[tilePos]).CollisionRectangle is Rectangle tileHitbox)
                    {

                        Rectangle collisionRect = new(
                            (int)tilePos.X * TileManager.BASE_TILESIZE + tileHitbox.Left,
                            (int)tilePos.Y * TileManager.BASE_TILESIZE + tileHitbox.Top,
                            tileHitbox.Width,
                            tileHitbox.Height
                        );

                        ResolveXSlidingCollision(futurePlayerRectX, collisionRect, player);
                    }
                }

                var futurePlayerRectY = new Rectangle(
                    (int)player.Position.X - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                    (int)(player.Position.Y + player.vel.Y) - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                    player.CollisionRectangle.Value.Width,
                    player.CollisionRectangle.Value.Height
                );
                List<Vector2> localCollidablesY = GetCollidableTilesAroundPlayer(futurePlayerRectY, TileMapLayer);
                foreach (var tilePos in localCollidablesY)
                {
                    if (TileManager.GetTile(TileMapLayer[tilePos]).CollisionRectangle is Rectangle tileHitbox)
                    {

                        Rectangle collisionRect = new(
                            (int)tilePos.X * TileManager.BASE_TILESIZE + tileHitbox.Left,
                            (int)tilePos.Y * TileManager.BASE_TILESIZE + tileHitbox.Top,
                            tileHitbox.Width,
                            tileHitbox.Height
                        );

                        ResolveYSlidingCollision(futurePlayerRectY, collisionRect, player);
                    }
                }
            }
        }

        public void HandlePlayerEntityCollisions(GameTime gameTime, List<Entity> SceneEntities, Player player)
        {
            var futurePlayerRectX = new Rectangle(
                (int)(player.Position.X + player.vel.X) - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)player.Position.Y - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (Entity entity in SceneEntities)
            {
                Rectangle collisionRect = new(
                    (int)entity.Position.X - entity.Width / 2 + tileSize / 2,
                    (int)entity.Position.Y - entity.Height / 2 + tileSize / 2,
                    entity.Width,
                    entity.Height
                );

                //Is Sliding Collision for Player
                if (futurePlayerRectX.Intersects(collisionRect))
                {
                    ResolveXSlidingCollision(futurePlayerRectX, collisionRect, player);
                }
            }

            var futurePlayerRectY = new Rectangle(
                (int)player.Position.X - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)(player.Position.Y + player.vel.Y) - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (Entity entity in SceneEntities)
            {
                Rectangle collisionRect = new(
                    (int)entity.Position.X - entity.Width / 2 + tileSize / 2,
                    (int)entity.Position.Y - entity.Height / 2 + tileSize / 2,
                    entity.Width,
                    entity.Height
                );

                //Is Sliding Collision for Player
                if (futurePlayerRectY.Intersects(collisionRect))
                {
                    ResolveYSlidingCollision(futurePlayerRectY, collisionRect, player);
                }
            }
        }

        public void HandlePlayerNPCCollisions(GameTime gameTime, NPCManager npcManager, Player player)
        {
            var futurePlayerRectX = new Rectangle(
                (int)(player.Position.X + player.vel.X) - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)player.Position.Y - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (NPC npc in npcManager.NPCs)
            {
                Rectangle collisionRect = new(
                    (int)npc.Position.X - npc.Width / 2 + tileSize / 2,
                    (int)npc.Position.Y - npc.Height / 2 + tileSize / 2,
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
                (int)player.Position.X - tileSize / 2 + player.Width / 2 - player.CollisionRectangle.Value.Width / 2 + (int)player.CollisionRectangle.Value.X,
                (int)(player.Position.Y + player.vel.Y) - tileSize / 2 + player.Height / 2 - player.CollisionRectangle.Value.Height / 2 + (int)player.CollisionRectangle.Value.Y,
                player.CollisionRectangle.Value.Width,
                player.CollisionRectangle.Value.Height
            );

            foreach (NPC npc in npcManager.NPCs)
            {
                Rectangle collisionRect = new(
                    (int)npc.Position.X - npc.Width / 2 + tileSize / 2,
                    (int)npc.Position.Y - npc.Height / 2 + tileSize / 2,
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
        private void ResolveYSlidingCollision(Rectangle futurePlayerRectY, Rectangle collisionRect, Player player)
        {
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

        private void ResolveXSlidingCollision(Rectangle futurePlayerRectX, Rectangle collisionRect, Player player)
        {
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
        #endregion

        #region Collision Tile Identification
        private List<Vector2> GetCollidableTilesAroundPlayer(Rectangle playerRect, Dictionary<Vector2, string> collisionMap)
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
        #endregion
    }
}
