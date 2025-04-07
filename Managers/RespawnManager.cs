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
<<<<<<< HEAD
<<<<<<< HEAD
            {
                obj.Position = SpawnPoint;
            }
=======
                obj.Position = SpawnPoint;

>>>>>>> 6a9ae56 (add death and respawn system)
=======
            {
                obj.Position = SpawnPoint;
            }
>>>>>>> 5e259bf (add death sound)
            if (obj.Name == "MeowSword")
                obj.Position = SpawnPoint + new Vector2(64, 0);
        }
    }
}