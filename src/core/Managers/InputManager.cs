using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project_Snowflake
{
    public class InputManager
    {
        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;

        private GamePadState _currentGamepad;
        private GamePadState _previousGamepad;

        public void Update()
        {
            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            _previousGamepad = _currentGamepad;
            _currentGamepad = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key)
                && !_previousKeyboard.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return !_currentKeyboard.IsKeyDown(key)
                && _previousKeyboard.IsKeyDown(key);
        }

        public bool IsButtonDown(Buttons button)
        {
            return _currentGamepad.IsButtonDown(button);
        }

        public bool IsButtonPressed(Buttons button)
        {
            return _currentGamepad.IsButtonDown(button)
                && !_previousGamepad.IsButtonDown(button);
        }

        public bool IsButtonReleased(Buttons button)
        {
            return !_currentGamepad.IsButtonDown(button)
                && _previousGamepad.IsButtonDown(button);
        }
        public bool IsAnyKeyReleased()
        {
            return _previousKeyboard.GetPressedKeys().Length > 0 && _currentKeyboard.GetPressedKeys().Length == 0;
        }
    }
}