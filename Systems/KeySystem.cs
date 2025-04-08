namespace uiia_adventure.Systems;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
using uiia_adventure.Globals;
using System;

public class KeySystem : SystemBase
{
    private float animationTimer = 0f;
    private float animationSpeed = 0.3f;
    private bool toggleFrame = false;
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            toggleFrame = !toggleFrame;
        }
        
        var keys = gameObjects.Where(o => o.HasComponent<KeyComponent>()).ToList();
        var players = SystemHelper.GetPlayerGameObjects(gameObjects);
        var inventory = gameObjects.Find(o => o.HasComponent<KeyInventoryComponent>());
        if (inventory == null) return;
        var inventoryComp = inventory.GetComponent<KeyInventoryComponent>();

        foreach (var key in keys)
        {
            var keyComp = key.GetComponent<KeyComponent>();
            if (keyComp == null || keyComp.IsCollected) continue;
            var spriteKey = key.GetComponent<SpriteComponent>();
            if (keyComp == null || spriteKey == null) continue;
            // Animate key
            spriteKey.RenderSource = toggleFrame ? keyComp.frame1 : keyComp.frame2;

            foreach (var player in players)
            {
                var playerSprite = player.GetComponent<SpriteComponent>();
                if (playerSprite == null) continue;

                if (CollisionHelper.ObjectVsObject(player, key))
                {
                    inventoryComp.HaveToCollect--;
                    spriteKey.RenderSource = Rectangle.Empty;
                    keyComp.IsCollected = true;
                    break;
                }
            }
        }
    }
}