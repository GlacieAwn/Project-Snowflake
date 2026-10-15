using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


using System;

namespace Project_Snowflake;


public sealed class Player
{
    private Vector2 _position;
    private Texture2D _texture;
    private Tilemap _tilemap;
    private Vector2 _direction;
    private ObjectManager _objectManager;
    private ContentManager _contentManager;

    public Player(Vector2 startPosition, Tilemap tilemap, ObjectManager objectManager, ContentManager contentManager)
    {
        _position = startPosition; 
        _tilemap = tilemap;
        _direction = Vector2.Zero;
        _objectManager = objectManager;
        _contentManager = contentManager;
        _texture = contentManager.Load<Texture2D>("GFX/SPR_SNOWFLAKE_STATIC");
    }

    public void Update(GameTime gameTime, InputManager inputManager)
{

    _direction = Vector2.Zero;

    if (inputManager.IsKeyPressed(Keys.W))
    {
        _direction = new Vector2(0, -1);
    }
    else if (inputManager.IsKeyPressed(Keys.S))
    {
        _direction = new Vector2(0, 1);
    }
    else if (inputManager.IsKeyPressed(Keys.A))
    {
        _direction = new Vector2(-1, 0);
    }
    else if (inputManager.IsKeyPressed(Keys.D))
    {
        _direction = new Vector2(1, 0);
    }
    // handle movement based on direction *and* collision checks
    if (_direction != Vector2.Zero)
    {
        int targetX = (int)(_position.X + _direction.X * Global.TILE_SIZE);
        int targetY = (int)(_position.Y + _direction.Y * Global.TILE_SIZE);

        Vector2 targetPosition = new Vector2(targetX, targetY);

        GameObject gameObject = _objectManager.GetObjectAtPosition(targetPosition);

        if (gameObject is MoveableBlock block)
        {
            if (block.Move(_direction))
            {
                _position = targetPosition;
            }
        }
        else if (!_tilemap.IsSolid(targetX, targetY))
        {
            _position += _direction * Global.TILE_SIZE;
            
        }
        
    }
}
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, Color.White);
    }

    public void Dispose()
    {
        _texture?.Dispose();
    }
}