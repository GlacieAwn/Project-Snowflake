using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;

namespace ProjectSnowflake
{
    public class PatrolEnemy
    {
        private Vector2 _direction;
        private Vector2 _position;
        private int _stepCount = 0;
        private int _maxSteps = 2;
        private Texture2D _texture;
        private ContentManager _contentManager;

        private float _moveTimer = 0.0f;
        private float _moveInterval = 1f;

        public PatrolEnemy(Vector2 startPos, Vector2 direction, int maxSteps, ContentManager contentManager)
        {
            _direction = direction;
            _maxSteps = maxSteps;
            _position = startPos;
            _contentManager = contentManager;
            _texture = _contentManager.Load<Texture2D>("GFX/SPR_SNOWFLAKE_STATIC");
        }

        public void Update(GameTime gameTime)
        {
            _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_moveTimer >= _moveInterval)
            {
                _moveTimer = 0.0f;
                _stepCount++;
                _position += _direction * Global.TILE_SIZE;
                if (_stepCount >= _maxSteps)
                {
                    _stepCount = 0;
                    _direction = -_direction;
                }
            }

        }

        private void TryStep()
    {
        // Grid logic: check next tile position in _direction
        // If clear & _stepCount < _maxSteps: move 1 tile, _stepCount++
        // If blocked or _stepCount >= _maxSteps: _direction *= -1, _stepCount = 0
    }

    }
}