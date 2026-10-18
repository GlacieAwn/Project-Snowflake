using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Project_Snowflake
{ 
    public class DebugMenu
    {
        private string[] _options;
        private int _selectedIndex;
        private Texture2D _arrow;
        private TrackerMusicPlayer _music;
        private bool isPlaying = false;

        const int MENU_SPACING = 32;

        public DebugMenu(GraphicsDevice graphicsDevice)
        {
            _options = new string[] {"MAP_COLLISION_TEST", "TITLE", "MUSIC_TEST", "SFX_TEST"};
            _selectedIndex = 0;
            _arrow = Texture2D.FromFile(graphicsDevice, "Content/GFX/SPR_ARROW.png");
            _music = new TrackerMusicPlayer();
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

            int levelCount = Enum.GetValues(typeof(LevelManager.Level)).Length;
            if (inputManager.IsKeyPressed(Keys.Left))
            {
                currentLevel = (LevelManager.Level)(((int)currentLevel - 1 + levelCount) % levelCount);
            }
            else if (inputManager.IsKeyPressed(Keys.Right))
            {
                currentLevel = (LevelManager.Level)(((int)currentLevel + 1) % levelCount);
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
                if (!isPlaying)
                {
                    isPlaying = true; 
                    _music.Play("Content/Audio/BGM/Snowflake house.xm", loop: true);
                }
                else
                {
                    isPlaying = false;
                    _music.Stop();
                }
            }
            if (_selectedIndex == 3 && inputManager.IsKeyPressed(Keys.Enter))
            {
                // Handle the action for the fourth menu option
                SoundEffect effect = SoundEffect.FromFile("Content/Audio/SFX/SFX_TEST.wav");
                effect.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, LevelManager.Level currentLevel)
        {
            spriteBatch.DrawString(font, "< " + currentLevel.ToString() + " >", position + new Vector2(32, 0), Color.White);
            for (int i = 0; i < _options.Length; i++)
            {
                Color color = Color.White;
                spriteBatch.DrawString(font, _options[i], position + new Vector2(32, (i + 1) * MENU_SPACING), color);
                if (i == _selectedIndex)
                {
                    spriteBatch.Draw(_arrow, position + new Vector2(0, (i + 1) * MENU_SPACING), Color.White);
                }
            }
        }
    }
}