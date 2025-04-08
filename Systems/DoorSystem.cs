namespace uiia_adventure.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Components;
using uiia_adventure.Core;

public class DoorSystem : SystemBase
{

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        var doorObject = gameObjects.Find(obj => obj.HasComponent<DoorComponent>());
        if (doorObject == null)
            return;

        var doorComponent = doorObject.GetComponent<DoorComponent>();

        if (doorComponent.IsOpen)
        {
            return;
        }

        var keyInventoryObj = gameObjects.Find(obj => obj.HasComponent<KeyInventoryComponent>());
        if (keyInventoryObj == null)
            return;

        var keyInventoryComponent = keyInventoryObj.GetComponent<KeyInventoryComponent>();
        if (keyInventoryComponent == null)
            return;

        if (keyInventoryComponent.HaveToCollect == 0)
        {
            // add sound here
            doorComponent.IsOpen = true;
            doorObject.GetComponent<SpriteComponent>().RenderSource = new Rectangle(0, 64, 64, 64);
        }
    }
}