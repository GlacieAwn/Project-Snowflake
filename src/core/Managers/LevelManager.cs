using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Snowflake
{
    public static class LevelManager
    {
        public static int _currentLevel;
        public static Tilemap _currentTilemap;

        public static string _mapPath;
        public static string _mapRenderPath;

        public static void LoadLevel(int level)
        {
            _currentLevel = level;
            _mapPath = $"Content/TILEMAPS/MAP_{level}.tmx";
            _mapRenderPath = $"Content/GFX/MAPS/MAP_{level}.png";
            _currentTilemap = new Tilemap(_mapPath, _mapRenderPath, Global._graphics_device);
            _currentTilemap.Draw(new SpriteBatch(Global._graphics_device));
        }

        public static void UnloadLevel()
        {
            _currentTilemap?.Dispose();
            _currentTilemap = null;
            _mapPath = null;
            _mapRenderPath = null;
        }

        public static void ReloadLevel()
        {
            UnloadLevel();
            LoadLevel(_currentLevel);
        }
    }
}
