using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharaoh.UI
{
    internal class Slider : GameObjectList
    {
        // The sprites for the background and foreground of this slider
        private SpriteGameObject _back, _front;

        // The minimum and maximum value associated with this slider
        private float _minValue, _maxValue;

        // The value that the slider is currently storing
        private float _currentValue;

        // The value that the slider had in the previous frame.
        private float _previousValue;

        // The number of pixels that the foreground block should stay away from the border
        private float _padding;

        // The difference between the minimum and maximum value that the slider can store
        private float Range
        { get { return _maxValue - _minValue; } }

        // The smallest X coordinate that the front image may have
        private float MinimumLocalX
        { get { return _padding + _front.Width / 2; } }

        // The largest X coordinate that the front image may have
        private float MaximumLocalX
        { get { return _back.Width - _padding - _front.Width / 2; } }

        // The total pixel width that is available for the front image
        private float AvailableWidth
        { get { return MaximumLocalX - MinimumLocalX; } }

        /// <summary>
        /// Returns whether the slider's value has changed in the last frame of the game loop.
        /// </summary>
        public bool ValueChanged
        {
            get { return _currentValue != _previousValue; }
        }

        /// <summary>
        /// Gets or sets the current numeric value that's stored in this slider.
        /// When you set this value, the foreground image will move to the correct position.
        /// </summary>
        public float Value
        {
            get { return _currentValue; }
            set
            {
                // store the value
                _currentValue = MathHelper.Clamp(value, _minValue, _maxValue);

                // calculate the new position of the foreground image
                float fraction = (_currentValue - _minValue) / Range;
                float newX = MinimumLocalX + fraction * AvailableWidth;
                _front.LocalPosition = new Vector2(newX, _padding);
            }
        }

        public Slider(string backgroundSprite, string forgroundSprite, float minValue, float maxValue, float padding)
        {
            // add the background image
            _back = new SpriteGameObject(backgroundSprite, 0.9f);
            AddChild(_back);

            // add the foreground image, with a custom origin
            _front = new SpriteGameObject(forgroundSprite, 0.95f);
            _front.Origin = new Vector2(_front.Width / 2, 0);
            AddChild(_front);

            // store the other values
            _minValue = minValue;
            _maxValue = maxValue;
            _padding = padding;

            // by default, start at the minimum value
            _previousValue = _minValue;
            Value = _previousValue;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (!Visible) { return; }

            Vector2 mousePos = inputHelper.MousePositionWorld;

            // store the previous slider value as a back-up
            _previousValue = Value;

            if (inputHelper.MouseLeftButtonDown() && _back.BoundingBox.Contains(mousePos))
            {
                // translate the mouse position to a number between 0 (left) and 1 (right)
                float correctedX = mousePos.X - GlobalPosition.X - MinimumLocalX;
                float newFraction = correctedX / AvailableWidth;

                // convert that to a new slider value
                Value = newFraction * Range + _minValue;
            }
        }
    }
}