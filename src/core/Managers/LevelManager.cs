using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Snowflake
{
    public class LevelManager
    {
        public enum Level
        {
            LEVEL_1,
            LEVEL_2,
            LEVEL_3,
            LEVEL_4,
            LEVEL_5,
            LEVEL_6,
            BLOCK_TEST,
            COLLISION_TEST,
            PATROL_TEST

        }
        public Level _currentLevel;
        public Tilemap _currentTilemap;

        public string _mapPath;
        public string _tilesetPath;

        public  void LoadLevel(Level level)
        {
            LoadLevel(level, Global._graphics_device);
        }

        public void LoadLevel(Level level, GraphicsDevice graphicsDevice)
        {
            UnloadLevel();
            _currentLevel = level;
            _mapPath = $"Content/TILEMAPS/MAP_{level}.tmx";
            _tilesetPath = $"Content/TILEMAPS/test_tileset.png";
            _currentTilemap = new Tilemap(_mapPath, _tilesetPath, graphicsDevice);
        }

        public  void UnloadLevel()
        {
            _currentTilemap?.Dispose();
            _currentTilemap = null;
            _mapPath = null;
            _tilesetPath = null;
        }

        public void ReloadLevel()
        {
            UnloadLevel();
            LoadLevel(_currentLevel);
        }
    }
}
