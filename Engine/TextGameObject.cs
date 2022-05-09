using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pharaoh
{
    /// <summary>
    /// Creates a new TextGameObject with the given details.
    /// </summary>
    /// <param name="fontName">The name of the font to use.</param>
    /// <param name="depth">The depth at which the text should be drawn.</param>
    /// <param name="color">The color with which the text should be drawn.</param>
    /// <param name="alignment">The horizontal alignment to use.</param>
    public class TextGameObject : GameObject
    {
        /// <summary>
        /// The color to use when drawing the text.
        /// </summary>
        public Color Color { get; set; }

        protected SpriteFont _font;

        /// <summary>
        /// The text that this object should draw on the screen.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The depth (between 0 and 1) at which this text should be drawn.
        /// A larger value means that the text will be drawn on top.
        /// </summary>
        protected float _depth;

        /// <summary>
        /// An enum that describes the different ways in which a text can be aligned horizontally.
        /// </summary>
        public enum Alignment
        {
            Left, Right, Center
        }

        /// <summary>
        /// The horizontal alignment of this text.
        /// </summary>
        protected Alignment _alignment;

        /// <summary>
        /// Draws this TextGameObject on the screen.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        private float OriginX
        {
            get
            {
                if (_alignment == Alignment.Left)
                { // left-aligned
                    return 0;
                }

                if (_alignment == Alignment.Right)
                { // right-aligned
                    return _font.MeasureString(Text).X;
                }

                return _font.MeasureString(Text).X / 2.0f; // centered
            }
        }

        /// <summary>
        /// Creates a new TextGameObject with the given details.
        /// </summary>
        /// <param name="fontName">The name of the font to use.</param>
        /// <param name="depth">The depth at which the text should be drawn.</param>
        /// <param name="color">The color with which the text should be drawn.</param>
        /// <param name="alignment">The horizontal alignment to use.</param>
        public TextGameObject(string fontName, float depth, Color color, Alignment alignment = Alignment.Left)
        {
            _font = ExtendedGame.AssetManager.LoadFont(fontName);
            Color = color;
            _alignment = alignment;
            _depth = depth;

            Text = "";
        }

        /// <summary>
        /// Draws this TextGameObject on the screen.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            // calculate the origin
            Vector2 origin = new Vector2(OriginX, 0);

            // draw the text
            spriteBatch.DrawString(_font, Text, GlobalPosition, Color, 0f, origin, 1, SpriteEffects.None, _depth);
        }
    }
}