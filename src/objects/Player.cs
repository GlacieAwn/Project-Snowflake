using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Project_Snowflake;


public sealed class Player
{
    private Vector2 position;
    private Texture2D texture;

    public Player(Vector2 startPosition, Texture2D startTexture)
    {
        position = startPosition;
        texture = startTexture;
    }

    public void Update(GameTime gameTime, InputManager inputManager)
    {

        if(inputManager.IsKeyPressed(Keys.W))
            position.Y -= Global.TILE_SIZE;
        if(inputManager.IsKeyPressed(Keys.S))
            position.Y += Global.TILE_SIZE;
        if(inputManager.IsKeyPressed(Keys.A))
            position.X -= Global.TILE_SIZE;
        if(inputManager.IsKeyPressed(Keys.D))
            position.X += Global.TILE_SIZE;
            
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }
}