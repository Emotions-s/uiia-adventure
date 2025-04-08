using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

public static class ObjectFactory
{
    public static GameObject CreateFrom(LevelObjectData data, ContentManager content)
    {
        GameObject obj = new();
        obj.Name = data.Type;

        // Set position
        if (data.Position != null && data.Position.Length == 2)
            obj.Position = new Vector2(data.Position[0], data.Position[1]);

        Texture2D texture = null;

        if (data.Type != "triggerLadder" && (data.Size == null || data.Size.Length != 2))
        {
            throw new ArgumentException("Size must be an array of two integers.");
        }

        // Add components based on type
        switch (data.Type)
        {
            case "box":
                obj.AddComponent(new PushableComponent());
                if (data.Texture == null)
                {
                    texture = ResourceCache.GetTexture2D("sprite/box", content);
                }
                obj.AddComponent(new PhysicsComponent());
                break;

            case "breakableBox":
                if (data.Health > 0)
                    obj.AddComponent(new BreakableComponent()
                    {
                        Health = data.Health
                    });
                else
                    obj.AddComponent(new BreakableComponent());

                if (data.Texture == null)
                {
                    texture = ResourceCache.GetTexture2D("sprite/box", content);
                }
                break;

            case "button":
                if (data.Targets == null || data.Targets.Count == 0)
                    throw new ArgumentException("Button must have at least one target.");
                obj.AddComponent(new ButtonComponent
                {
                    IsPressed = false,
                    TargetIds = data.Targets
                });
                if (data.Texture == null)
                {
                    texture = ResourceCache.GetTexture2D("sprite/button", content);
                }
                break;

            case "triggerLadder":
                if (data.TriggerId == null)
                    throw new ArgumentException("TriggerLadder must have a TriggerId.");

                if (data.Tiles == null || data.Tiles.Count == 0)
                    throw new ArgumentException("TriggerLadder must have at least one tile.");
                obj.AddComponent(new TriggerableTileMapComponent
                {
                    TriggerId = data.TriggerId,
                    Tiles = data.Tiles.ToDictionary(k => ParsePoint(k.Key), k => k.Value)
                });
                break;
        }

        if (texture == null && !string.IsNullOrEmpty(data.Texture))
        {
            texture = ResourceCache.GetTexture2D(data.Texture, content);
            Rectangle source = new(0, 0, data.Size?[0] ?? 64, data.Size?[1] ?? 64);
        }

        // Sprite
        if (!string.IsNullOrEmpty(data.Texture))
        {
            texture = ResourceCache.GetTexture2D(data.Texture, content);

            obj.AddComponent(new SpriteComponent
            {
                Texture = texture,
                SourceRect = new(0, 0, data.Size[0], data.Size[1]),
                RenderSource = new(0, 0, data.Size[0], data.Size[1])
            });
        }

        return obj;
    }

    private static Point ParsePoint(string s)
    {
        var parts = s.Split(',');
        return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}