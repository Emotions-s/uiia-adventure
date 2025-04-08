using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

public static class ResourceCache
{
    private static readonly Dictionary<string, Texture2D> _texture = new();

    private static readonly Dictionary<string, Song> _sound = new();

    private static readonly Dictionary<string, SoundEffect> _soundEffect = new();

    public static Texture2D GetTexture2D(string path, ContentManager content)
    {
        if (!_texture.TryGetValue(path, out var texture))
        {
            texture = content.Load<Texture2D>(path);
            _texture[path] = texture;
        }
        return texture;
    }

    public static Song GetSong(string path, ContentManager content)
    {
        if (!_sound.TryGetValue(path, out var song))
        {
            song = content.Load<Song>(path);
            _sound[path] = song;
        }
        return song;
    }

    public static SoundEffect GetSoundEffect(string path, ContentManager content)
    {
        if (!_soundEffect.TryGetValue(path, out var soundEffect))
        {
            soundEffect = content.Load<SoundEffect>(path);
            _soundEffect[path] = soundEffect;
        }
        return soundEffect;
    }
}