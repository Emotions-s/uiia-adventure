using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;

namespace uiia_adventure.Globals;

public static class SystemHelper
{
    public static List<GameObject> GetPlayerGameObject(List<GameObject> gameObjects)
    {
        List<GameObject> playerGameObjects = new List<GameObject>();
        foreach (var obj in gameObjects)
        {
            if (obj.Name == GameConstants.MeowBowName || obj.Name == GameConstants.MeowSwordName)
            {
                playerGameObjects.Add(obj);
            }
        }
        return playerGameObjects.Count > 0 ? playerGameObjects : null;
    }

    public static GameObject GetTriggerGameObjectById(List<GameObject> gameObjects, string trigger)
    {
        foreach (var obj in gameObjects)
        {
            var triggerTileMapComponent = obj.GetComponent<TriggerableTileMapComponent>();
            if (triggerTileMapComponent == null)
                continue;

            if (triggerTileMapComponent.TriggerId == trigger)
            {
                return obj;
            }
        }
        return null;
    }

    public static List<GameObject> GetTriggerGameObjectByIds(List<GameObject> gameObjects, List<string> trigger)
    {
        List<GameObject> triggerGameObjects = new List<GameObject>();
        foreach (var obj in gameObjects)
        {
            var triggerTileMapComponent = obj.GetComponent<TriggerableTileMapComponent>();
            if (triggerTileMapComponent == null)
                continue;

            trigger.ForEach(t =>
            {
                if (triggerTileMapComponent.TriggerId == t)
                {
                    triggerGameObjects.Add(obj);
                }
            });
        }
        return triggerGameObjects.Count > 0 ? triggerGameObjects : null;
    }

    public static GameObject GetGameObjectByName(List<GameObject> gameObjects, string name)
    {
        foreach (var obj in gameObjects)
        {
            if (obj.Name == name)
            {
                return obj;
            }
        }
        return null;
    }
}