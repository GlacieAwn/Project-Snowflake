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
    private Texture2D _testTexture;
    private Texture2D _playerTexture;
    private InputManager _inputManager;
    private MidiSynth _midiSynth;
    private MidiSequencer _midiSequencer;
    private Player _player;
    private DebugMenu _debugMenu;
    private SpriteFont _font;

    private Tilemap _test_tilemap;


    private Global.GameState _currentState = Global.GameState.DebugMenu;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.HardwareModeSwitch = false;

        if(_graphics.IsFullScreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = Global.WINDOW_WIDTH * 4; 
            _graphics.PreferredBackBufferHeight = Global.WINDOW_HEIGHT * 4;
        }
        
        _graphics.ApplyChanges();

        _renderTarget = new RenderTarget2D(GraphicsDevice, Global.SCREEN_WIDTH, Global.SCREEN_HEIGHT);
        _inputManager = new InputManager();
        _debugMenu = new DebugMenu(GraphicsDevice);
        _font = Content.Load<SpriteFont>("FONTS/silkscreen");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _testTexture = Content.Load<Texture2D>("GFX/SPR_BG_TEST");
        _playerTexture = Content.Load<Texture2D>("GFX/SPR_SNOWFLAKE_STATIC");
        _test_tilemap = new Tilemap("Content/TILEMAPS/MAP_COLLISION_TEST.tmx", "Content/GFX/MAPS/MAP_COLLISION_TEST.png", GraphicsDevice);    
        _player = new Player(_test_tilemap.GetObjectPosition("Player_Start"), _playerTexture, _test_tilemap); // Construct player after texture is loaded to avoid null reference


        // string bgmDirectory = System.IO.Path.Combine(Content.RootDirectory, "BGM");
        // _midiSynth = new MidiSynth(System.IO.Path.Combine(bgmDirectory, "SOUNDBANK.sf2"));
        // _midiSequencer = new MidiSequencer(_midiSynth);

        // var song = MidiFileLoader.Load(System.IO.Path.Combine(bgmDirectory, "smg2_honeybloom_galaxy.mid"));
        // _midiSequencer.Play(song, loop: true);
    }

    protected override void UnloadContent()
    {
        // _midiSynth?.Dispose();
        _player?.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();
        // _midiSequencer.Update();
        
        if (_inputManager.IsButtonPressed(Buttons.Back) || _inputManager.IsKeyPressed(Keys.Escape))
            Exit();

        if(_inputManager.IsKeyPressed(Keys.F1))
            _currentState = Global.GameState.DebugMenu;

        // TODO: Add your update logic here
        switch (_currentState)
        {
            case Global.GameState.Title:
                // Handle title screen logic
                // Console.WriteLine("Title Screen");
                break;
            case Global.GameState.Menu:
                // Handle menu logic
                // Console.WriteLine("Menu Screen");
                break;
            case Global.GameState.Playing:
                // Handle gameplay logic
                // Console.WriteLine("Playing Screen");
                _player.Update(gameTime, _inputManager);
                break;
            case Global.GameState.Paused:
                // Handle paused logic
                // Console.WriteLine("Paused Screen");
                break;
            case Global.GameState.DebugMenu:
                // Handle debug menu logic
                // Console.WriteLine("Debug Menu Screen");
                _debugMenu.Update(_inputManager, ref _currentState);
                break;
        }

        base.Update(gameTime);
        Console.WriteLine(_currentState);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);


        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        // _spriteBatch.Draw(_testTexture, Vector2.Zero, Color.White);
        switch (_currentState)
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
                _test_tilemap.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
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

        scaleX = (float)_graphics.PreferredBackBufferWidth / Global.SCREEN_WIDTH;
        scaleY = (float)_graphics.PreferredBackBufferHeight / Global.SCREEN_HEIGHT;
        float scale = Math.Min(scaleX, scaleY);
        
        dst = new Rectangle((GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(Global.SCREEN_WIDTH * scale)) / 2, (GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(Global.SCREEN_HEIGHT * scale)) / 2, (int)(Global.SCREEN_WIDTH * scale), (int)(Global.SCREEN_HEIGHT * scale));
        GraphicsDevice.Clear(Color.Black);

        dst = new Rectangle((GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(Global.SCREEN_WIDTH * scale)) / 2, (GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(Global.SCREEN_HEIGHT * scale)) / 2, (int)(Global.SCREEN_WIDTH * scale), (int)(Global.SCREEN_HEIGHT * scale));


        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        // Rectangle screenBounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        _spriteBatch.Draw(_renderTarget, dst, Color.White);
        switch (_currentState)
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
                _debugMenu.Draw(_spriteBatch, _font, new Vector2(10, 10));
                break;
        }  
        _spriteBatch.End();
        // Console.WriteLine($"{_graphics.PreferredBackBufferWidth}x{_graphics.PreferredBackBufferHeight}, scaleX {scaleX}, scaleY {scaleY}, dst {dst}");

        base.Draw(gameTime);
    }
}
