namespace uiia_adventure.Systems;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
public class ButtonSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            var ButtonComponent = gameObject.GetComponent<ButtonComponent>();
            var SpriteComponent = gameObject.GetComponent<SpriteComponent>();
            if (ButtonComponent == null || SpriteComponent == null)
                continue;

            List<GameObject> players = SystemHelper.GetPlayerGameObject(gameObjects);
            if (players == null)
                continue;

            if (ButtonComponent.IsPressed)
            {
                continue;
            }
            foreach (var player in players)
            {
                var playerSprite = player.GetComponent<SpriteComponent>();
                if (playerSprite == null)
                    break;

                if (!CollisionHelper.ObjectVsObject(player, gameObject))
                {
                    break;
                }

                ButtonComponent.IsPressed = true;

                var triggerObj = SystemHelper.GetTriggerGameObjectByIds(gameObjects, ButtonComponent.targetIds);
                if (triggerObj == null)
                    break;

                triggerObj.ForEach(target =>
                    {
                        var triggerableTileMapComponent = target.GetComponent<TriggerableTileMapComponent>();
                        if (triggerableTileMapComponent != null)
                        {
                            TriggerTileMap(triggerableTileMapComponent, gameObjects);
                        }

                    });
            }
        }
    }

    private void TriggerTileMap(TriggerableTileMapComponent tileMaps, List<GameObject> gameObjects)
    {
        GameObject tileMapObj = SystemHelper.GetGameObjectByName(gameObjects, GameConstants.TileMapName);
        if (tileMapObj == null) return;

        var tileMapComponent = tileMapObj.GetComponent<TileMapComponent>();
        var ladderComponent = tileMapObj.GetComponent<LadderComponent>();
        if (tileMapComponent == null || ladderComponent == null) return;

        foreach (var tile in tileMaps.Grid)
        {
            tileMapComponent.Grid[tile.Key] = tile.Value;
            ladderComponent.Tiles.Add(tile.Key);
        }
    }
}