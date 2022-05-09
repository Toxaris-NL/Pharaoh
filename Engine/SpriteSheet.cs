using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pharaoh
{
    /// <summary>
    /// A class that can represent a sprite sheet: an image containing a grid of sprites.
    /// </summary>
    public class SpriteSheet
    {
        private Texture2D _sprite;
        private Rectangle _spriteRectangle;
        private int _sheetIndex, _sheetColumns, _sheetRows;
        private float _depth;
        private bool[,] _pixelTransparency;

        /// <summary>
        /// Gets or sets whether the displayed sprite should be mirrored.
        /// </summary>
        public bool Mirror { get; set; }

        /// <summary>
        /// Gets the height of a single sprite in this sprite sheet.
        /// </summary>
        public int Height
        { get { return _sprite.Height / _sheetRows; } }

        /// <summary>
        /// Gets the width of a single sprite in this sprite sheet.
        /// </summary>
        public int Width
        { get { return _sprite.Width / _sheetColumns; } }

        /// <summary>
        /// Gets a vector that represents the center of a single sprite in this sprite sheet.
        /// </summary>
        public Vector2 Center
        { get { return new Vector2(Width, Height) / 2; } }

        /// <summary>
        /// Gets a Rectangle that represents the bounds of a single sprite in this sprite sheet.
        /// </summary>
        public Rectangle Bounds
        { get { return new Rectangle(0, 0, Width, Height); } }

        /// <summary>
        /// Gets the total number of elements in this sprite sheet.
        /// </summary>
        public int NumberOfSheetElements
        { get { return _sheetColumns * _sheetRows; } }

        /// <summary>
        /// Gets or sets the sprite index within this sprite sheet to use.
        /// If you set a new index, the object will recalculate which part of the sprite should be drawn.
        /// </summary>
        public int SheetIndex
        {
            get { return _sheetIndex; }
            set
            {
                if (value >= 0 && value < NumberOfSheetElements)
                {
                    _sheetIndex = value;

                    // recalculate the part of the sprite to draw
                    int columnIndex = _sheetIndex % _sheetColumns;
                    int rowIndex = _sheetIndex / _sheetColumns;
                    _spriteRectangle = new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
                }
            }
        }

        /// <summary>
        /// Creates a new SpriteSheet with the given details.
        /// </summary>
        /// <param name="assetname">The name of the asset to load.
        /// The number of sprites in the sheet will be extracted from this filename.</param>
        /// <param name="depth">The depth at which the sprite should be drawn.</param>
        /// <param name="sheetIndex">The sprite sheet index to use initially.</param>
        public SpriteSheet(string assetName, float depth, int sheetIndex = 0)
        {
            _depth = depth;

            // retrieve the sprite
            _sprite = ExtendedGame.AssetManager.LoadSprite(assetName);

            // for all pixels, check if they are transparent
            Color[] colorData = new Color[_sprite.Width * _sprite.Height];
            _sprite.GetData(colorData);
            _pixelTransparency = new bool[_sprite.Width, _sprite.Height];
            for (int i = 0; i < colorData.Length; ++i)
                _pixelTransparency[i % _sprite.Width, i / _sprite.Width] = colorData[i].A == 0;

            _sheetColumns = 1;
            _sheetRows = 1;

            // see if we can extract the number of sheet elements from the assetname
            string[] assetSplit = assetName.Split('@');
            if (assetSplit.Length >= 2)
            {
                // behind the last '@' symbol, there should be a number.
                // This number can be followed by an 'x' and then another number.
                string sheetNrData = assetSplit[assetSplit.Length - 1];
                string[] columnAndRow = sheetNrData.Split('x');
                _sheetColumns = int.Parse(columnAndRow[0]);

                if (columnAndRow.Length == 2) { _sheetRows = int.Parse(columnAndRow[1]); }
            }

            // apply the sheet index; this will also calculate spriteRectangle
            SheetIndex = sheetIndex;
        }

        /// <summary>
        /// Draws the sprite (or the appropriate part of it) at the desired position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used for drawing sprites.</param>
        /// <param name="position">A position in the game world.</param>
        /// <param name="origin">An origin that should be subtracted from the drawing position.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
        {
            // mirror the sprite?
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Mirror)
                spriteEffects = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(_sprite, position, _spriteRectangle, Color.White, 0.0f, origin, 1.0f, spriteEffects, _depth);
        }

        /// <summary>
        /// Returns whether or not the pixel at a given coordinate is transparent.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <returns>true if the given pixel is fully transparent; false if it is not.</returns>
        public bool IsPixelTransparent(int x, int y)
        {
            int column = _sheetIndex % _sheetColumns;
            int row = _sheetIndex / _sheetColumns % _sheetRows;

            return _pixelTransparency[column * Width + x, row * Height + y];
        }
    }
}