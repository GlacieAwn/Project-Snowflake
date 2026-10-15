using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;

public static class Global
{
    public const int TILE_SIZE = 16;
    public const int SCREEN_WIDTH = 288;
    public const int SCREEN_HEIGHT = 160;

    public const int WINDOW_WIDTH = 256;
    public const int WINDOW_HEIGHT = 144;

    public static GraphicsDevice _graphics_device;

    public enum GameState
    {
        Title,
        Menu,
        Playing,
        Paused,
        DebugMenu
    }

}