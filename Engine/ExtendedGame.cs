using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Pharaoh
{
    public class ExtendedGame : Game
    {
        // Standard MonoGame objects for graphics and sprites
        protected GraphicsDeviceManager _graphics;

        protected SpriteBatch _spriteBatch;

        // An object for handling keyboard and mouse input
        protected InputHelper _inputHelper;

        /// <summary>
        /// The width and height of the game world, in game units.
        /// </summary>
        protected Point _worldSize;

        /// <summary>
        /// The width and height of the window, in pixels.
        /// </summary>
        protected Point _windowSize;

        /// <summary>
        /// A matrix used for scaling the game world so that it fits inside the window.
        /// </summary>
        protected Matrix _spriteScale;

        /// <summary>
        /// The object that manages all game states, one of which is the active state.
        /// </summary>
        public static GameStateManager GameStateManager { get; private set; }

        /// <summary>
        /// An object for generating random numbers throughout the game.
        /// </summary>
        public static Random Random { get; private set; }

        /// <summary>
        /// An object for loading assets throughout the game.
        /// </summary>
        public static AssetManager AssetManager { get; private set; }

        /// <summary>
        /// Gets or sets whether the game is running in full-screen mode.
        /// </summary>
        public bool FullScreen
        {
            get { return _graphics.IsFullScreen; }
            protected set { ApplyResolutionSettings(value); }
        }

        public static string ContentRootDirectory
        { get { return "Content"; } }

        /// <summary>
        /// Creates a new ExtendedGame object.
        /// </summary>
        protected ExtendedGame()
        {
            // MonoGame preparations
            Content.RootDirectory = ContentRootDirectory;
            _graphics = new GraphicsDeviceManager(this);

            // create the input helper and random number generator
            _inputHelper = new InputHelper(this);
            Random = new Random();

            // default window and world size
            _windowSize = new Point(1024, 768);
            _worldSize = new Point(1024, 768);
        }

        /// <summary>
        /// Does the initialization tasks that involve assets, and then prepares the game world.
        /// Override this method to do your own specific things when your game starts.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // store a static reference to the Content Manager
            AssetManager = new AssetManager(Content);

            // by default, we're not running in full-screen mode
            FullScreen = false;

            // create an empty game world
            GameStateManager = new GameStateManager();
        }

        /// <summary>
        /// Updates all objects in the game world, by first calling HandleInput and then Update.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            GameStateManager.Update(gameTime);
        }

        /// <summary>
        /// Performs basic input handling and then calls HandleInput for the entire game world.
        /// </summary>
        protected virtual void HandleInput()
        {
            _inputHelper.Update();

            // quit the game when the player presses Esc
            if (_inputHelper.KeyPressed(Keys.Escape))
            {
                Exit();
            }

            // toggle full-screen mode when the player presses F5
            if (_inputHelper.KeyPressed(Keys.F5))
            {
                FullScreen = !FullScreen;
            }

            GameStateManager.HandleInput(_inputHelper);
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // start drawing sprites, applying the scaling matrix
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, _spriteScale);

            // let the game world draw itself
            GameStateManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }

        /// <summary>
        /// Scales the window to the desired size, and calculates how the game world should be scaled to fit inside that window.
        /// </summary>
        private void ApplyResolutionSettings(bool fullScreen)
        {
            Point screenSize;

            // make the game full-screen or not
            _graphics.IsFullScreen = fullScreen;

            // get the size of the screen to use: either the window size or the full screen size
            if (fullScreen)
            {
                screenSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else
            {
                screenSize = _windowSize;
            }

            // scale the window to the desired size
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.PreferredBackBufferWidth = screenSize.X;

            _graphics.ApplyChanges();

            // calculate and set the viewport to use
            GraphicsDevice.Viewport = CalculateViewPort(screenSize);

            // calculate how the graphics should be scaled, , so that the game world fits inside the window
            _spriteScale = Matrix.CreateScale((float)GraphicsDevice.Viewport.Width / _worldSize.X,
                (float)GraphicsDevice.Viewport.Height / _worldSize.Y, 1);
        }

        /// <summary>
        ///  Calculates and returns the viewport to use, so that the game world fits on the screen while preserving its aspect ratio
        /// </summary>
        /// <param name="windowSize">The size of the screen on which the world should be drawn.</param>
        /// <returns>A Viewport object that will show the game world as large as possible while preserving its aspect ratio.</returns>
        private Viewport CalculateViewPort(Point windowSize)
        {
            // create a Viewport object
            Viewport viewport = new Viewport();

            // calculate the two aspect ratios
            float gameAspectRatio = (float)_worldSize.X / _worldSize.Y;
            float windowAspectRatio = (float)windowSize.X / windowSize.Y;

            // if the window is relative wide, use full window height
            if (windowAspectRatio > gameAspectRatio)
            {
                viewport.Width = (int)(windowSize.Y * gameAspectRatio);
                viewport.Height = windowSize.Y;
            }
            // if the window is relatively high, use the full window width
            else
            {
                viewport.Width = windowSize.X;
                viewport.Height = (int)(windowSize.X / gameAspectRatio);
            }

            // calculate and store the top-left corner of the viewport
            viewport.X = (windowSize.X - viewport.Width) / 2;
            viewport.Y = (windowSize.Y - viewport.Height) / 2;

            return viewport;
        }

        /// <summary>
        /// Converts a position in screen coordinates to a position in world coordinates.
        /// </summary>
        /// <param name="screenPosition">A position in screen coordinates.</param>
        /// <returns>The corresponding position in world coordinates.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector2 viewportTopLeft = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);

            float screenToWorldScale = _worldSize.X / (float)GraphicsDevice.Viewport.Width;

            return (screenPosition - viewportTopLeft) * screenToWorldScale;
        }
    }
}