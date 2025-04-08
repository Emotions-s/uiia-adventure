namespace uiia_adventure.Systems;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
public class ButtonSystem : SystemBase
{
    private static SoundEffect? _buttonSound = Game1.buttonSound;
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            var ButtonComponent = gameObject.GetComponent<ButtonComponent>();
            var SpriteComponent = gameObject.GetComponent<SpriteComponent>();
            if (ButtonComponent == null || SpriteComponent == null)
                continue;

            if (ButtonComponent.IsPressed)
            {
                SpriteComponent.RenderSource = ButtonComponent.PressedFrame;
                continue;
            }
            else
            {
                SpriteComponent.RenderSource = ButtonComponent.NormalFrame;
            }

            List<GameObject> otherSpriteObj = SystemHelper.getGameObjectsByType<SpriteComponent>(gameObjects);

            if (otherSpriteObj == null)
                continue;

            foreach (var obj in otherSpriteObj)
            {
                if (obj == gameObject)
                    continue;

                var playerSprite = obj.GetComponent<SpriteComponent>();
                if (playerSprite == null)
                    continue;

                if (!CollisionHelper.ObjectVsObject(obj, gameObject))
                {
                    continue;
                }

                ButtonComponent.IsPressed = true;

                _buttonSound?.Play();

                var triggerObj = SystemHelper.GetTriggerGameObjectByIds(gameObjects, ButtonComponent.TargetIds);
                if (triggerObj == null)
                    continue;

                // add sound here
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

        foreach (var tile in tileMaps.Tiles)
        {
            tileMapComponent.Tiles[tile.Key] = tile.Value;
            ladderComponent.Tiles.Add(tile.Key);
        }
    }
}