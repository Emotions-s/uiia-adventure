using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;

namespace uiia_adventure.Factories;

public static class ProjectileFactory
{
    private static Dictionary<string, GameObject> _templates = new();

    public static void RegisterTemplate(string name, GameObject template)
    {
        _templates[name] = template;
    }

    public static GameObject Create(string templateName, Vector2 position)
    {
        if (!_templates.TryGetValue(templateName, out var template))
            throw new ArgumentException($"Projectile template '{templateName}' not registered.");

        var clone = template.Clone();
        clone.Position = position;
        clone.Name = templateName;

        return clone;
    }
}