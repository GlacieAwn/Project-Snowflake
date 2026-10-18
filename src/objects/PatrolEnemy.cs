using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;

namespace Project_Snowflake;
public class PatrolEnemy : GameObject
{
    private Vector2 _direction;
    private Vector2 _position;
    private int _stepCount = 0;
    private int _maxSteps = 2;
    private Texture2D _texture;
    private ContentManager _contentManager;
    private static ObjectManager _objectManager; // reference to the object, not actually using it here


    private float _moveTimer = 0.0f;
    private float _moveInterval = 1f;

    public PatrolEnemy(Vector2 startPos, Vector2 direction, int maxSteps, ContentManager contentManager)
    {
        _direction = direction;
        _maxSteps = maxSteps;
        _position = startPos;
        _contentManager = contentManager;
        _texture = _contentManager.Load<Texture2D>("GFX/SPR_SNOWFLAKE_STATIC");

        _objectManager.AddObject(this);
    }

    public override void Update(GameTime gameTime)
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

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, Color.White);
    }

    private void TryStep()
    {
        // Grid logic: check next tile position in _direction
        // If clear & _stepCount < _maxSteps: move 1 tile, _stepCount++
        // If blocked or _stepCount >= _maxSteps: _direction *= -1, _stepCount = 0
    }

}