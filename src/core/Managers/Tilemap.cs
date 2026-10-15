using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Reflection.Metadata;
using TiledCS;

namespace Project_Snowflake
{

    public class Tilemap
    {
        private TiledMap _map;
        private TiledLayer _collisionLayer;
        public TiledLayer _objectLayer;
        private Texture2D _mapRender;

        public Tilemap(string mapPath, string mapRenderPath, GraphicsDevice graphicsDevice)
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

        public void LoadObjects(ObjectManager objectManager, ContentManager contentManager)
        {
            foreach (var obj in _objectLayer.objects)
            {
                // Here you would typically create the appropriate game object based on the TiledObject
                // For example, if you have a MoveableBlock class:
                if (obj.type == "Block" || obj.@class == "Block") // TiledCS 3.3.3 reads the .tmx "type" attribute into obj.type
                {
                    MoveableBlock block = new MoveableBlock(new Vector2(obj.x, obj.y), this, objectManager, contentManager);
                }
            }
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

        public bool IsObjectAtPosition(string name, int x, int y)
        {
            return _objectLayer.objects.Any(obj => obj.name == name && obj.x == x && obj.y == y);
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
}