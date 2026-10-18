using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledCS;
public abstract class GameObject
{
    public Vector2 Position { get; protected set; }

    protected Texture2D Texture { get; set; }
    protected ContentManager Content { get; set; }
    protected string TexturePath { get; set; }

    // Unloading the asset (instead of disposing the texture directly) clears ContentManager's cache,
    // otherwise a later Load would hand back the already-disposed texture.
    public void DisposeTexture()
    {
        Content.UnloadAsset(TexturePath);
        Texture = null;
    }

    public void ReloadTexture()
    {
        Texture = Content.Load<Texture2D>(TexturePath);
    }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}