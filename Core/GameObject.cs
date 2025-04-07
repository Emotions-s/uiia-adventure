namespace uiia_adventure.Core;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using uiia_adventure.Components;

public class GameObject
{
    public string Name { get; set; } = "Unnamed";
    public Vector2 Position;
    public Dictionary<Type, IComponent> Components = new();

    public void AddComponent<T>(T component) where T : IComponent => Components[typeof(T)] = component;
    public T GetComponent<T>() where T : IComponent {
        if (Components.ContainsKey(typeof(T))) return (T)Components[typeof(T)];
        return default;

    }

    public void RemoveComponent<T>() where T : IComponent => Components.Remove(typeof(T));

    public bool HasComponent<T>() where T : IComponent => Components.ContainsKey(typeof(T));
}