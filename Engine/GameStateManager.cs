using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Pharaoh
{
    /// <summary>
    /// A class that contains all game states of the game, and that makes sure
    /// that the currently active game state updates and draws itself.
    /// </summary>
    public class GameStateManager : IGameLoopObject
    {
        // The collection of all game states.
        private Dictionary<string, GameState> _gameStates;

        // A reference to the game state that is currently active.
        private GameState _currentGameState;

        /// <summary>
        /// Creates a new GameStateManager object.
        /// </summary>
        public GameStateManager()
        {
            _gameStates = new Dictionary<string, GameState>();
            _currentGameState = null;
        }

        /// <summary>
        /// Adds a game state to the collection.
        /// </summary>
        /// <param name="name">The name by which this game state should be indexed.</param>
        /// <param name="state">The game state to add.</param>
        public void AddGameState(string name, GameState state)
        {
            _gameStates[name] = state;
        }

        /// <summary>
        /// Gets the game state with the given name, if it exists.
        /// </summary>
        /// <param name="name">The name of the game state to find.</param>
        /// <returns>The GameState with that name, or null if it could not be found.</returns>
        public GameState GetGameState(string name)
        {
            if (_gameStates.ContainsKey(name))
            {
                return _gameStates[name];
            }

            return null;
        }

        /// <summary>
        /// Switches to a different active game state.
        /// </summary>
        /// <param name="name">The name of the game state to set as the new active one.</param>
        public void SwitchTo(string name)
        {
            if (_gameStates.ContainsKey(name))
            {
                _currentGameState = _gameStates[name];
            }
        }

        /// <summary>
        /// Makes sure that the currently active game state calls Update.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        public void Update(GameTime gameTime)
        {
            if (_currentGameState != null)
            {
                _currentGameState.Update(gameTime);
            }
        }

        /// <summary>
        /// Makes sure that the currently active game state calls Draw.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_currentGameState != null)
            {
                _currentGameState.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Makes sure that the currently active game state calls HandleInput.
        /// </summary>
        /// <param name="inputHelper">The input helper to use.</param>
        public void HandleInput(InputHelper inputHelper)
        {
            if (_currentGameState != null)
            {
                _currentGameState.HandleInput(inputHelper);
            }
        }

        /// <summary>
        /// Makes sure that the currently active game state calls Reset.
        /// </summary>
        public void Reset()
        {
            if (_currentGameState != null)
            {
                _currentGameState?.Reset();
            }
        }
    }
}