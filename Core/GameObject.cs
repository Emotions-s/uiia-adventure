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
    public T GetComponent<T>() where T : IComponent
    {
        if (Components.ContainsKey(typeof(T))) return (T)Components[typeof(T)];
        return default;
    }

    public T GetComponent<T>(Type type) where T : IComponent
    {
        if (Components.ContainsKey(type)) return (T)Components[type];
        return default;
    }

    public void RemoveComponent<T>() where T : IComponent => Components.Remove(typeof(T));

    public bool HasComponent<T>() where T : IComponent => Components.ContainsKey(typeof(T));
    public bool HasComponent(Type type) => Components.ContainsKey(type);

    public GameObject Clone()
    {
        GameObject clone = new GameObject();
        clone.Name = this.Name;
        clone.Position = this.Position;

        foreach (var kv in Components)
        {
            var component = kv.Value;

            // You can customize this if you want deep copy
            var clonedComponent = (IComponent)Activator.CreateInstance(component.GetType());
            foreach (var field in component.GetType().GetFields())
            {
                field.SetValue(clonedComponent, field.GetValue(component));
            }

            clone.Components[kv.Key] = clonedComponent;
        }

        return clone;
    }
}