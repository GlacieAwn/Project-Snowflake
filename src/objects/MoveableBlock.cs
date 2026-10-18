using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledCS;

using System;

namespace Project_Snowflake;

public class MoveableBlock : GameObject
{
    private Tilemap _tilemap;
    private static ObjectManager _objectManager; // reference to the object, not actually using it here

    public MoveableBlock(Vector2 startPosition, Tilemap tilemap, ObjectManager objectManager, ContentManager contentManager)
    {
        Position = startPosition;
        _tilemap = tilemap;
        _objectManager = objectManager;
        Content = contentManager;
        TexturePath = "GFX/SPR_ICE_BLOCK";
        ReloadTexture();

        _objectManager.AddObject(this);

    }

    public bool Move(Vector2 direction)
    {
        int targetX = (int)(Position.X + direction.X * Global.TILE_SIZE);
        int targetY = (int)(Position.Y + direction.Y * Global.TILE_SIZE);

        if (!_tilemap.IsSolid(targetX, targetY))
        {
            Position += direction * Global.TILE_SIZE;
            return true;
        }
        return false;
    }

    public override void Update(GameTime gameTime)
    {
        // NOT USED, ONLY HERE FOR COMPATIBILITY WITH GAMEOBJECT CLASS THINGY
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

    public void Add(TiledObject obj, Tilemap tilemap)
    {
    }
}