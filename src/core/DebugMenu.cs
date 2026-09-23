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

        const int MENU_SPACING = 16;

        public DebugMenu(string[] options, GraphicsDevice graphicsDevice)
        {
            _options = options;
            _selectedIndex = 0;
            _arrow = Texture2D.FromFile(graphicsDevice, "Content/GFX/SPR_ARROW.png");
        }

        public void Update(InputManager inputManager, Global.GameState gameState)
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
                // Handle the action for the first menu option
                gameState = Global.GameState.Playing;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                Color color = Color.White;
                spriteBatch.DrawString(font, _options[i], position + new Vector2(0, i * MENU_SPACING), color);
                if (i == _selectedIndex)
                {
                    spriteBatch.Draw(_arrow, position + new Vector2(-16, i * MENU_SPACING), Color.White);
                }
            }
        }
    }
}