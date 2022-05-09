﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Pharaoh
{
    /// <summary>
    /// A non-visual game object that has a list of game objects as its children.
    /// </summary>
    public class GameObjectList : GameObject
    {
        /// <summary>
        /// The child objects of this game object.
        /// </summary>
        private List<GameObject> _children;

        /// <summary>
        /// Creates a new GameObjectList with an empty list of children.
        /// </summary>
        public GameObjectList()
        {
            _children = new List<GameObject>();
        }

        /// <summary>
        /// Adds an object to this GameObjectList, and sets this GameObjectList as the parent of that object.
        /// </summary>
        /// <param name="obj">The game object to add.</param>
        public void AddChild(GameObject child)
        {
            child.Parent = this;
            _children.Add(child);
        }

        /// <summary>
        /// Performs input handling for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="inputHelper">An object required for handling player input.</param>
        public override void HandleInput(InputHelper inputHelper)
        {
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                _children[i].HandleInput(inputHelper);
            }
        }

        /// <summary>
        /// Performs the Update method for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (GameObject obj in _children)
            {
                obj.Update(gameTime);
            }
        }

        /// <summary>
        /// Performs the Draw method for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            foreach (GameObject obj in _children)
            {
                obj.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Performs the Reset method for all game objects in this GameObjectList.
        /// </summary>
        public override void Reset()
        {
            foreach (GameObject obj in _children)
            {
                obj.Reset();
            }
        }
    }
}