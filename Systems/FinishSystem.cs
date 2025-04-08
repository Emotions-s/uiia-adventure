using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class FinishSystem(SceneFlowController flowController) : SystemBase
{
    private readonly SceneFlowController _flowController = flowController;
    private bool _finished = false;
    private static SoundEffect? _finishSound = Game1.finishSound;


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        if (_finished) return;

        var finishObj = gameObjects.Find(g => g.HasComponent<FinishTileComponent>());
        if (finishObj == null) return;

        var debugComponent = gameObjects.Find(g => g.HasComponent<DebugComponent>());


        var finishArea = finishObj.GetComponent<FinishTileComponent>().Area;

        if (debugComponent != null)
        {
            var debugTile = debugComponent?.GetComponent<DebugComponent>();
            if (debugTile != null)
            {
                debugTile.Rectangles.Add(finishArea);
            }
        }
        var players = SystemHelper.GetPlayerGameObjects(gameObjects);

        int insideCount = 0;
        foreach (var player in players)
        {
            var sprite = player.GetComponent<SpriteComponent>();
            if (sprite == null) continue;

            Rectangle playerRect = new(
                (int)player.Position.X + sprite.SourceRect.X,
                (int)player.Position.Y + sprite.SourceRect.Y,
                sprite.SourceRect.Width,
                sprite.SourceRect.Height
            );

            if (finishArea.Intersects(playerRect))
            {
                insideCount++;
            }
        }

        if (insideCount >= 2)
        {
            _finished = true;
            _finishSound?.Play();
            _flowController.GoToNextScene();
        }
    }
}