using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pharaoh
{
    /// <summary>
    /// A class that manages mouse and keyboard input.
    /// </summary>
    public class InputHelper
    {
        // the current and previous mouse state
        private MouseState _currentMouseState, _previousMouseState;

        // the current and previous keyboard state
        private KeyboardState _currentKeyboardState, _previousKeyboardState;

        // A reference to the game
        private ExtendedGame _game;

        /// <summary>
        /// Gets the current position of the mouse in screen coordinates.
        /// </summary>
        public Vector2 MousePositionScreen
        {
            get { return new Vector2(_currentMouseState.X, _currentMouseState.Y); }
        }

        /// <summary>
        /// Gets the current position of the mouse in world coordinates.
        /// </summary>
        public Vector2 MousePositionWorld
        {
            get { return _game.ScreenToWorld(MousePositionScreen); }
        }

        public InputHelper(ExtendedGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Updates this InputHelper object for one frame of the game loop.
        /// This method retrievse the current the mouse and keyboard state, and stores the previous states as a backup.
        /// </summary>
        public void Update()
        {
            _previousMouseState = _currentMouseState;
            _previousKeyboardState = _currentKeyboardState;
            _currentMouseState = Mouse.GetState();
            _currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks and returns whether the player has started pressing a certain keyboard key in the last frame of the game loop.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>true if the given key is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool KeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks and returns whether the player has stopped pressing a certain keyboard key in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is no longer pressed but was still pressed in the previous frame; false otherwise.</returns>
        public bool KeyReleased(Keys k)
        {
            return _currentKeyboardState.IsKeyUp(k) && _previousKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Checks and returns whether the player is currently holding a certain keyboard key down.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is currently being held down; false otherwise.</returns>
        public bool KeyDown(Keys k)
        {
            return _currentKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Checks and returns whether the player has started pressing the left mouse button in the last frame of the game loop.
        /// </summary>
        /// <returns>true if the left mouse button is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool MouseLeftButtonPressed()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Checks and returns whether the left mouse button is currently being held down.
        /// </summary>
        /// <returns>true if the left mouse button is currently being held down; false otherwise.</returns>
        public bool MouseLeftButtonDown()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}