using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_Snowflake
{ 
    public class DebugMenu
    {
        private string[] _options;
        private int _selectedIndex;
        private Texture2D _arrow;

        const int MENU_SPACING = 32;

        public DebugMenu(GraphicsDevice graphicsDevice)
        {
            _options = new string[] {"MAP_COLLISION_TEST", "TITLE", "MUSIC_TEST", "SFX_TEST", "MIDI_TEST"};
            _selectedIndex = 0;
            _arrow = Texture2D.FromFile(graphicsDevice, "Content/GFX/SPR_ARROW.png");
        }

        public void Update(InputManager inputManager, ref Global.GameState gameState, ref LevelManager.Level currentLevel)
        {
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _selectedIndex = (_selectedIndex - 1 + _options.Length) % _options.Length;
            }
            else if (inputManager.IsKeyPressed(Keys.S))
            {
                _selectedIndex = (_selectedIndex + 1) % _options.Length;
            }

            if (_selectedIndex == 0 && inputManager.IsKeyPressed(Keys.Enter))
            {
                gameState = Global.GameState.Playing;
            }
            if (_selectedIndex == 1 && inputManager.IsKeyPressed(Keys.Enter))
            {
                // Handle the action for the second menu option
                gameState = Global.GameState.Title;
            }
            if (_selectedIndex == 2 && inputManager.IsKeyPressed(Keys.Enter))
            {
                var music = new TrackerMusicPlayer();
                music.Play("Content/BGM/Snowflake house.xm", loop: true);
            }
            if (_selectedIndex == 3 && inputManager.IsKeyPressed(Keys.Enter))
            {
                // Handle the action for the fourth menu option
            }
            if (_selectedIndex == 4 && inputManager.IsKeyPressed(Keys.Enter))
            {
                // Handle the action for the fifth menu option
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                Color color = Color.White;
                spriteBatch.DrawString(font, _options[i], position + new Vector2(32, i * MENU_SPACING), color);
                if (i == _selectedIndex)
                {
                    spriteBatch.Draw(_arrow, position + new Vector2(0, i * MENU_SPACING), Color.White);
                }
            }
        }
    }
}