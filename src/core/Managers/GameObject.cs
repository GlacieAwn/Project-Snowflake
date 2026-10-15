using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
public abstract class GameObject
{
    public Vector2 Position { get; protected set; }

    public abstract void Draw(SpriteBatch spriteBatch);
}