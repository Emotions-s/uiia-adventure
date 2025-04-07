using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Managers;

public static class RespawnManager
{
    public static Vector2 SpawnPoint { get; set; }

    public static void RespawnPlayers(List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            if (obj.Name == GameConstants.MeowBowName)
                obj.Position = SpawnPoint;

            if (obj.Name == GameConstants.MeowSwordName)
                obj.Position = SpawnPoint + new Vector2(64, 0);
        }
    }
}