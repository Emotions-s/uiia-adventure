using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;

namespace uiia_adventure.Managers;

public static class RespawnManager
{
    public static Vector2 SpawnPoint { get; set; }

    public static void RespawnPlayers(List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            if (obj.Name == "MeowBow")
                obj.Position = SpawnPoint;

            if (obj.Name == "MeowSword")
                obj.Position = SpawnPoint + new Vector2(64, 0);
        }
    }
}