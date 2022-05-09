using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Pharaoh
{
    /// <summary>
    /// Displays a number of particles around an image
    /// </summary>
    public class ParticleField : GameObject
    {
        // The image of a single particle
        private Texture2D _particle;

        // The target image on which the particle effect should be applied
        private Texture2D _target;

        // The rectangle withing the target image that should receive particles
        private Rectangle _targetRectangle;

        // The random positions of particles in the field
        private List<Vector2> _positions;

        // The current scales of the particles; these are numbers between 0 and 2
        private List<float> _scales;

        public ParticleField(Texture2D target, int numberOfParticles, Rectangle targetRectangle, string spriteName)
        {
            // load the particle sprite
            _particle = ExtendedGame.AssetManager.LoadSprite(spriteName);

            // initialize some member variables
            _target = target;
            _targetRectangle = targetRectangle;
            _positions = new List<Vector2>();
            _scales = new List<float>();

            // create random particles
            for (int i = 0; i < numberOfParticles; i++)
            {
                _positions.Add(CreateRandomPosition());
                _scales.Add(0f);
            }
        }

        private Vector2 CreateRandomPosition()
        {
            // keep trying random positions until a valid one is found
            while (true)
            {
                // draw a random position within the target rectangle
                Point randomPos = new Point(
                    ExtendedGame.Random.Next(_targetRectangle.Width),
                    ExtendedGame.Random.Next(_targetRectangle.Height))
                    + _targetRectangle.Location;

                // get pixel data at the position
                Rectangle rect = new Rectangle(randomPos, new Point(1, 1));
                Color[] retrieveColor = new Color[1];
                _target.GetData(0, rect, retrieveColor, 0, 1);

                // if the pixel is fully opaque, accept it as the answer
                if (retrieveColor[0].A == 255)
                {
                    return randomPos.ToVector2();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update each particle
            for (int i = 0; i < _positions.Count; i++)
            {
                // let the particle grow. If the particle is currently invisible,
                // it has a small chance to start growing
                if (_scales[i] > 0 || ExtendedGame.Random.NextDouble() < 0.001)
                {
                    _scales[i] += 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    // if the particle has reached scale 2, initialize a new random particle.
                    if (_scales[i] >= 2.0f)
                    {
                        _scales[i] = 0f;
                        _positions[i] = CreateRandomPosition();
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 particleCenter = new Vector2(_particle.Width, _particle.Height) / 2;
            for (int i = 0; i < _scales.Count; i++)
            {
                float scale = _scales[i];
                // a scale between 1 and 2 means the particle is shrinking again
                if (_scales[i] > 1)
                {
                    scale = 2 - _scales[i];
                }

                // draw the particle at its current scale
                spriteBatch.Draw(_particle, GlobalPosition + _positions[i], null, Color.White, 0f, particleCenter, scale, SpriteEffects.None, 0);
            }
        }
    }
}