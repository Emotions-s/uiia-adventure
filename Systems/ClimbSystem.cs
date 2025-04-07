namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

public class ClimbSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var ladderObj = gameObjects.Find(obj => obj.HasComponent<LadderComponent>());
        if (ladderObj == null) return;

        var ladder = ladderObj.GetComponent<LadderComponent>();
        if (ladder == null) return;

        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();
            var stats = obj.GetComponent<StatsComponent>();

            if (input == null || physics == null || stats == null)
                continue;

            if (!input.WantsToJump)
                continue;

            Point topLeft = new((int)obj.Position.X / GameConstants.TileSize, (int)obj.Position.Y / GameConstants.TileSize);
            Point topRight = new(((int)(obj.Position.X + GameConstants.TileSize - 1)) / GameConstants.TileSize, (int)obj.Position.Y / GameConstants.TileSize);
            Point bottomLeft = new((int)obj.Position.X / GameConstants.TileSize, ((int)(obj.Position.Y + GameConstants.TileSize - 1)) / GameConstants.TileSize);
            Point bottomRight = new(((int)(obj.Position.X + GameConstants.TileSize - 1)) / GameConstants.TileSize, ((int)(obj.Position.Y + GameConstants.TileSize - 1)) / GameConstants.TileSize);

            bool isOnLadder =
                ladder.Tiles.Contains(topLeft) ||
                ladder.Tiles.Contains(topRight) ||
                ladder.Tiles.Contains(bottomLeft) ||
                ladder.Tiles.Contains(bottomRight);

            if (isOnLadder)
            {
                physics.Velocity.Y = -stats.MoveSpeed * 1.2f;
                physics.IsGrounded = true;
            }
        }
    }
}