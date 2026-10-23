using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Project_Snowflake;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private float scaleX;
    private float scaleY;
    private Rectangle dst;
    private InputManager _inputManager;
    // private MidiSynth _midiSynth;
    // private MidiSequencer _midiSequencer;
    private GameManager _gameManager;
    private SpriteFont _font;
    private TrackerMusicPlayer trackerMusicPlayer;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = true;
        _graphics.HardwareModeSwitch = false;

        if(_graphics.IsFullScreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = Global.WINDOW_WIDTH * 6; 
            _graphics.PreferredBackBufferHeight = Global.WINDOW_HEIGHT * 6;
        }
        
        _graphics.ApplyChanges();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Global._graphics_device = GraphicsDevice;
        _renderTarget = new RenderTarget2D(GraphicsDevice, Global.SCREEN_WIDTH, Global.SCREEN_HEIGHT);
        _inputManager = new InputManager();
        _gameManager = new GameManager(Content, GraphicsDevice, _inputManager);
        _font = Content.Load<SpriteFont>("FONTS/silkscreen");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // NOT USED, BUT REQUIRED BY MONOGAME FRAMEWORK
    }

    protected override void UnloadContent()
    {
        _gameManager?.Unload();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();
        _gameManager.Update(gameTime);
        
        if (_inputManager.IsButtonPressed(Buttons.Back) || _inputManager.IsKeyPressed(Keys.Escape))
            Exit();


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);


        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        switch (_gameManager.State)
        {
            case Global.GameState.Title:
                // Draw title screen
                break;
            case Global.GameState.Menu:
                // Draw menu
                break;
            case Global.GameState.Playing:
                // Draw gameplay
                GraphicsDevice.SetRenderTarget(_renderTarget);
                GraphicsDevice.Clear(Color.Black);
                _gameManager.Draw(_spriteBatch);
                break;
            case Global.GameState.Paused:
                // Draw paused screen
                break;
            case Global.GameState.DebugMenu:
                // Draw debug menu
                GraphicsDevice.Clear(Color.Black);
                // _debugMenu.Draw(_spriteBatch, _font, new Vector2(10, 10));
                break;
        }   
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        int scaleX = _graphics.PreferredBackBufferWidth / Global.SCREEN_WIDTH;
        int scaleY = _graphics.PreferredBackBufferHeight / Global.SCREEN_HEIGHT;
        int scale = Math.Min(scaleX, scaleY);
        
        dst = new Rectangle((GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(Global.SCREEN_WIDTH * scale)) / 2, (GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(Global.SCREEN_HEIGHT * scale)) / 2, (int)(Global.SCREEN_WIDTH * scale), (int)(Global.SCREEN_HEIGHT * scale));
        GraphicsDevice.Clear(Color.Black);
        dst = new Rectangle((GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(Global.SCREEN_WIDTH * scale)) / 2, (GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(Global.SCREEN_HEIGHT * scale)) / 2, (int)(Global.SCREEN_WIDTH * scale), (int)(Global.SCREEN_HEIGHT * scale));


        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        // Rectangle screenBounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        _spriteBatch.Draw(_renderTarget, dst, Color.White);
        switch (_gameManager.State)
        {
            case Global.GameState.Title:
                //NOT USED OUTSIDE OF RENDER TARGET
                break;
            case Global.GameState.Menu:
                //NOT USED OUTSIDE OF RENDER TARGET

                break;
            case Global.GameState.Playing:
                //NOT USED OUTSIDE OF RENDER TARGET
                break;
            case Global.GameState.Paused:
                //NOT USED OUTSIDE OF RENDER TARGET
                break;
            case Global.GameState.DebugMenu:
                // Draw debug menu
                GraphicsDevice.Clear(Color.Black);
                _gameManager.debugMenu.Draw(_spriteBatch, _font, new Vector2(10, 10), _gameManager._currentLevel);
                break;
        }  
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
