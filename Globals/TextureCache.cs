using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class TextureCache
{
    private static readonly Dictionary<string, Texture2D> _cache = new();

    public static Texture2D Get(string path, ContentManager content)
    {
        if (!_cache.TryGetValue(path, out var texture))
        {
            texture = content.Load<Texture2D>(path);
            _cache[path] = texture;
        }
        return texture;
    }
}