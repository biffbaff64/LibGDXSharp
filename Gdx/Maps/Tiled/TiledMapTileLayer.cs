namespace LibGDXSharp.Maps.Tiled
{
    public class TiledMapTileLayer : MapLayer
    {
        public int       Width      { get; private set; }
        public int       Height     { get; private set; }
        public int       TileWidth  { get; private set; }
        public int       TileHeight { get; private set; }
        public Cell[ , ] Cells      { get; private set; }

        /// <summary>
        /// Creates TiledMap layer
        /// @param width layer width in tiles
        /// @param height layer height in tiles
        /// @param tileWidth tile width in pixels
        /// @param tileHeight tile height in pixels
        /// </summary>
        public TiledMapTileLayer( int width, int height, int tileWidth, int tileHeight )
        {
            this.Width      = width;
            this.Height     = height;
            this.TileWidth  = tileWidth;
            this.TileHeight = tileHeight;
            this.Cells      = new Cell[ width, height ];
        }

        /// <summary>
        /// @param x X coordinate
        /// @param y Y coordinate
        /// @return {@link Cell} at (x, y)
        /// </summary>
        public Cell? GetCell( int x, int y )
        {
            if ( x < 0 || x >= Width ) return null;
            if ( y < 0 || y >= Height ) return null;

            return Cells[ x, y ];
        }

        /// <summary>
        /// Sets the {@link Cell} at the given coordinates.
        /// @param x X coordinate
        /// @param y Y coordinate
        /// @param cell the {@link Cell} to set at the given coordinates.
        /// </summary>
        public void SetCell( int x, int y, Cell cell )
        {
            if ( x < 0 || x >= Width ) return;
            if ( y < 0 || y >= Height ) return;

            Cells[ x, y ] = cell;
        }

        /// <summary>
        /// @brief represents a cell in a TiledLayer: TiledMapTile, flip and rotation properties.
        /// </summary>
        public class Cell
        {
            private ITiledMapTile? _tile;
            private bool         _flipHorizontally;
            private bool         _flipVertically;
            private int          _rotation;

            /// <summary>
            /// @return The tile currently assigned to this cell.
            /// </summary>
            public ITiledMapTile? GetTile()
            {
                return _tile;
            }

            /// <summary>
            /// Sets the tile to be used for this cell.
            /// @param tile the {@link TiledMapTile} to use for this cell. 
            /// @return this, for method chaining
            /// </summary>
            public Cell SetTile( ITiledMapTile tile )
            {
                this._tile = tile;

                return this;
            }

            /// <summary>
            /// @return Whether the tile should be flipped horizontally.
            /// </summary>
            public bool GetFlipHorizontally()
            {
                return _flipHorizontally;
            }

            /// <summary>
            /// Sets whether to flip the tile horizontally.
            /// @param flipHorizontally whether or not to flip the tile horizontally. 
            /// @return this, for method chaining
            /// </summary>
            public Cell SetFlipHorizontally( bool flipHorizontally )
            {
                this._flipHorizontally = flipHorizontally;

                return this;
            }

            /// <summary>
            /// @return Whether the tile should be flipped vertically.
            /// </summary>
            public bool GetFlipVertically()
            {
                return _flipVertically;
            }

            /// <summary>
            /// Sets whether to flip the tile vertically.
            /// @param flipVertically whether or not this tile should be flipped vertically. 
            /// @return this, for method chaining
            /// </summary>
            public Cell SetFlipVertically( bool flipVertically )
            {
                this._flipVertically = flipVertically;

                return this;
            }

            /// <summary>
            /// @return The rotation of this cell, in 90 degree increments.
            /// </summary>
            public int GetRotation()
            {
                return _rotation;
            }

            /// <summary>
            /// Sets the rotation of this cell, in 90 degree increments.
            /// @param rotation the rotation in 90 degree increments (see ints below). 
            /// @return this, for method chaining
            /// </summary>
            public Cell SetRotation( int rotation )
            {
                this._rotation = rotation;

                return this;
            }

            public const int Rotate0   = 0;
            public const int Rotate90  = 1;
            public const int Rotate180 = 2;
            public const int Rotate270 = 3;
        }
    }
}

