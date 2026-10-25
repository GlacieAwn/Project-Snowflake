using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


using System;

namespace Project_Snowflake;


public sealed class Player : GameObject
{
    private Tilemap _tilemap;
    private Vector2 _direction;
    private ObjectManager _objectManager;
    private InputManager _inputManager;

    public Player(Vector2 startPosition, Tilemap tilemap, ObjectManager objectManager, ContentManager contentManager, InputManager inputManager)
    {
        Position = startPosition;
        _tilemap = tilemap;
        _direction = Vector2.Zero;
        _objectManager = objectManager;
        _inputManager = inputManager;
        Content = contentManager;
        TexturePath = "GFX/SPR_SNOWFLAKE_STATIC";
        ReloadTexture();

        _objectManager.AddObject(this);
    }

    public override void Update(GameTime gameTime)
{

    _direction = Vector2.Zero;

    if (_inputManager.IsKeyPressed(Keys.W) || _inputManager.IsButtonPressed(Buttons.DPadUp))
    {
        _direction = new Vector2(0, -1);
    }
    else if (_inputManager.IsKeyPressed(Keys.S) || _inputManager.IsButtonPressed(Buttons.DPadDown))
    {
        _direction = new Vector2(0, 1);
    }
    else if (_inputManager.IsKeyPressed(Keys.A) || _inputManager.IsButtonPressed(Buttons.DPadLeft))
    {
        _direction = new Vector2(-1, 0);
    }
    else if (_inputManager.IsKeyPressed(Keys.D) || _inputManager.IsButtonPressed(Buttons.DPadRight))
    {
        _direction = new Vector2(1, 0);
    }
    // handle movement based on direction *and* collision checks
    if (_direction != Vector2.Zero)
    {
        int targetX = (int)(Position.X + _direction.X * Global.TILE_SIZE);
        int targetY = (int)(Position.Y + _direction.Y * Global.TILE_SIZE);

        Vector2 targetPosition = new Vector2(targetX, targetY);

        GameObject gameObject = _objectManager.GetObjectAtPosition(targetPosition);

        if (gameObject is MoveableBlock block)
        {
            if (block.Move(_direction))
            {
                Position = targetPosition;
            }
        }
        else if (!_tilemap.IsSolid(targetX, targetY))
        {
            Position += _direction * Global.TILE_SIZE;
            
        }
        
    }
}
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}