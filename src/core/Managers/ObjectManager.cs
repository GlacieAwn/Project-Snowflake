using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project_Snowflake
{
    public class ObjectManager
    {
        private List<GameObject> _gameObjects;

        public ObjectManager()
        {
            _gameObjects = new List<GameObject>();
        }

        public void AddObject(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }
        }

        public GameObject GetObjectAtPosition(Vector2 position)
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.Position == position)
                {
                    return gameObject;
                }
            }
            return null;
        }
    }
}