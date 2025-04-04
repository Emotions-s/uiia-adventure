using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace uiia_adventure.Managers;
public static class ResolutionManager
{
    public static int VirtualWidth = 1920;
    public static int VirtualHeight = 1080;

    public static Matrix GetScaleMatrix(GraphicsDevice device)
    {
        float scaleX = device.PresentationParameters.BackBufferWidth / (float)VirtualWidth;
        float scaleY = device.PresentationParameters.BackBufferHeight / (float)VirtualHeight;
        float scale = MathF.Min(scaleX, scaleY);

        return Matrix.CreateScale(scale, scale, 1f);
    }

    public static Rectangle GetDestinationRectangle(GraphicsDevice device)
    {
        float scaleX = device.PresentationParameters.BackBufferWidth / (float)VirtualWidth;
        float scaleY = device.PresentationParameters.BackBufferHeight / (float)VirtualHeight;
        float scale = MathF.Min(scaleX, scaleY);

        int width = (int)(VirtualWidth * scale);
        int height = (int)(VirtualHeight * scale);

        int x = (device.PresentationParameters.BackBufferWidth - width) / 2;
        int y = (device.PresentationParameters.BackBufferHeight - height) / 2;

        return new Rectangle(x, y, width, height);
    }
}