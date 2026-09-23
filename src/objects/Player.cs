using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Project_Snowflake;


public sealed class Player
{
    private Vector2 _position;
    private Texture2D _texture;
    private Tilemap _tilemap;

    public Player(Vector2 startPosition, Texture2D startTexture, Tilemap tilemap)
    {
        _position = startPosition;
        _texture = startTexture;
        _tilemap = tilemap;
    }

public void Update(GameTime gameTime, InputManager inputManager)
{
    if (inputManager.IsKeyPressed(Keys.W))
    {
        int targetX = (int)_position.X;
        int targetY = (int)_position.Y - Global.TILE_SIZE;

        if (!_tilemap.IsSolid(targetX, targetY))
        {
            _position.Y -= Global.TILE_SIZE;
        }
    }

    if (inputManager.IsKeyPressed(Keys.S))
    {
        int targetX = (int)_position.X;
        int targetY = (int)_position.Y + Global.TILE_SIZE;

        if (!_tilemap.IsSolid(targetX, targetY))
        {
            _position.Y += Global.TILE_SIZE;
        }
    }

    if (inputManager.IsKeyPressed(Keys.A))
    {
        int targetX = (int)_position.X - Global.TILE_SIZE;
        int targetY = (int)_position.Y;

        if (!_tilemap.IsSolid(targetX, targetY))
        {
            _position.X -= Global.TILE_SIZE;
        }
    }

    if (inputManager.IsKeyPressed(Keys.D))
    {
        int targetX = (int)_position.X + Global.TILE_SIZE;
        int targetY = (int)_position.Y;

        if (!_tilemap.IsSolid(targetX, targetY))
        {
            _position.X += Global.TILE_SIZE;
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