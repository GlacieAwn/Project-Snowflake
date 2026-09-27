using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

using TiledCS;

public class Tilemap
{
    private TiledMap _map;
    private TiledLayer _collisionLayer;
    private TiledLayer _objectLayer;
    private Texture2D _mapRender;

    public Tilemap(string mapPath, String mapRenderPath, GraphicsDevice graphicsDevice)
    {
        _map = new TiledMap(mapPath);
        _mapRender = Texture2D.FromFile(graphicsDevice, mapRenderPath);

        _collisionLayer = _map.Layers.First(layer => layer.name == "Collision");
        _objectLayer = _map.Layers.First(layer => layer.name == "Objects");
    }

    public bool IsSolid(int x, int y)
    {
        if (x < 0 ||  y < 0 || x >= _collisionLayer.width * _map.TileWidth || y >= _collisionLayer.height * _map.TileHeight)
        {
            return true;
        }

        int tileX = x / _map.TileWidth;
        int tileY = y / _map.TileHeight;

        int index = tileY * _collisionLayer.width + tileX;
        int gid = _collisionLayer.data[index];

        return gid != 0;
    }

    public TiledObject GetObject(string name)
    {
        return _objectLayer.objects.First(obj => obj.name == name);
    }

    public Vector2 GetObjectPosition(string name)
    {
        TiledObject obj = GetObject(name);
        return new Vector2(obj.x, obj.y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_mapRender, Vector2.Zero, Color.White);
    }

    public void Dispose()
    {
        _mapRender?.Dispose();
        _mapRender = null;
        _collisionLayer = null;
        _map = null;
    }
}