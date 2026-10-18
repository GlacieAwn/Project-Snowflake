using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_Snowflake
{
    public class GameManager
    {
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly InputManager _inputManager;
        public LevelManager.Level _currentLevel = LevelManager.Level.PATROL_TEST;

        public LevelManager levelManager { get; private set; }
        public ObjectManager objectManager { get; private set; }
        public DebugMenu debugMenu { get; private set; }
        public Global.GameState State { get; private set; } = Global.GameState.DebugMenu;
        public TrackerMusicPlayer trackerMusicPlayer { get; private set; }

        public GameManager(ContentManager content, GraphicsDevice graphicsDevice, InputManager inputManager)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _inputManager = inputManager;
            trackerMusicPlayer = new TrackerMusicPlayer();
            levelManager = new LevelManager();
            objectManager = new ObjectManager();
            debugMenu = new DebugMenu(graphicsDevice);

        }

        public void Update(GameTime gameTime)
        {
            if (_inputManager.IsKeyPressed(Keys.F1))
                State = Global.GameState.DebugMenu;

            switch (State)
            {
                case Global.GameState.Playing:
                    objectManager.Update(gameTime);
                    break;
                case Global.GameState.DebugMenu:
                    var state = State;
                    debugMenu.Update(_inputManager, ref state, ref _currentLevel);
                    State = state;
                    break;
            }

            if (State == Global.GameState.Playing
                && (levelManager._currentTilemap == null || levelManager._currentLevel != _currentLevel))
            {
                StartLevel(_currentLevel);
            }
        }

        public void StartLevel(LevelManager.Level level)
        {
            _currentLevel = level;
            objectManager.DisposeObjects();
            // ObjectManager has no clear method, so a fresh one is used per level.
            objectManager = new ObjectManager();
            levelManager.LoadLevel(level, _graphicsDevice);
            levelManager._currentTilemap.LoadObjects(objectManager, _content);
            new Player(levelManager._currentTilemap.GetObjectPosition("Player_Start"), levelManager._currentTilemap, objectManager, _content, _inputManager);
        }

        // Draws gameplay into the currently bound render target.
        public void Draw(SpriteBatch spriteBatch)
        {
            if (State != Global.GameState.Playing || levelManager._currentTilemap == null)
                return;

            levelManager._currentTilemap.Draw(spriteBatch);
            objectManager.Draw(spriteBatch);
        }

        public void Unload()
        {
            objectManager.DisposeObjects();
            levelManager.UnloadLevel();
        }
    }
}
