using Microsoft.Xna.Framework;

namespace Pharaoh
{
    /// <summary>
    /// An object that can make another object visible for a certain amount of time
    /// <summary>
    public class VisibilityTimer : GameObject
    {
        protected GameObject _target;
        protected float _timeLeft;

        /// <summary>
        /// Creates a new VisibilityTimer with the given target object.
        /// </summary>
        /// <param name="target">The game object whose visibility you want to manage.</param>
        public VisibilityTimer(GameObject target)
        {
            _timeLeft = 0;
            _target = target;
        }

        /// <summary>
        /// Updates the  VisibilityTimer.
        /// </summary>
        /// <param name="target">An object containing information about the time that has passed in the game.</param>
        public override void Update(GameTime gameTime)
        {
            // if the timer has already passed earlier, don't do anything
            if (_timeLeft <= 0)
            {
                return;
            }

            // decrease the timer by the time that has passed since the last frame
            _timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if enough time has passed, make the target object invisible
            if (_timeLeft <= 0)
            {
                _target.Visible = false;
            }
        }

        /// <summary>
        /// Makes the target object visible and starts a timer for the specified number of seconds.
        /// </summary>
        /// <param name="seconds">How long the target object should be visible.</param>
        public void StartVisible(float seconds)
        {
            _timeLeft = seconds;
            _target.Visible = true;
        }
    }
}