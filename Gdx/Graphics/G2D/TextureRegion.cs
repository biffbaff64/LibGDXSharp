namespace LibGDXSharp.G2D
{
    /// <summary>
    /// Defines a rectangular area of a texture. The coordinate system used has
    /// its origin in the upper left corner with the x-axis pointing to the
    /// right and the y axis pointing downwards.
    /// </summary>
    public class TextureRegion
    {
        private int   _regionWidth;
        private int   _regionHeight;
        private float _u;
        private float _v;
        private float _u2;
        private float _v2;

        public TextureRegion()
        {
        }

        public TextureRegion( Texture texture )
        {
            Texture = texture;
            SetRegion( 0, 0, texture.Width, texture.Height );
        }

        public TextureRegion( Texture texture, int width, int height )
            : this( texture, 0, 0, width, height )
        {
        }

        public TextureRegion( Texture texture, int x, int y, int width, int height )
        {
            Texture = texture;
            SetRegion( x, y, width, height );
        }

        public TextureRegion( Texture texture, float u, float v, float u2, float v2 )
        {
            Texture = texture;
            SetRegion( u, v, u2, v2 );
        }

        public TextureRegion( TextureRegion region )
        {
            SetRegion( region );
        }

        public TextureRegion( TextureRegion region, int x, int y, int width, int height )
        {
            SetRegion( region, x, y, width, height );
        }

        public void SetRegion( Texture texture )
        {
            Texture = texture;
            SetRegion( 0, 0, texture.Width, texture.Height );
        }

        public void SetRegion( int x, int y, int width, int height )
        {
            var invTexWidth  = 1f / Texture.Width;
            var invTexHeight = 1f / Texture.Height;

            SetRegion( x * invTexWidth, y * invTexHeight, ( x + width ) * invTexWidth, ( y + height ) * invTexHeight );

            RegionWidth  = System.Math.Abs( width );
            RegionHeight = System.Math.Abs( height );
        }

        public void SetRegion( float u, float v, float u2, float v2 )
        {
            var texWidth  = Texture.Width;
            var texHeight = Texture.Height;

            RegionWidth  = ( int )System.Math.Round( System.Math.Abs( u2 - u ) * texWidth );
            RegionHeight = ( int )System.Math.Round( System.Math.Abs( v2 - v ) * texHeight );

            // For a 1x1 region, adjust UVs toward pixel center to avoid filtering
            // artifacts on AMD GPUs when drawing very stretched.
            if ( RegionWidth == 1 && RegionHeight == 1 )
            {
                var adjustX = 0.25f / texWidth;

                u  += adjustX;
                u2 -= adjustX;

                var adjustY = 0.25f / texHeight;

                v  += adjustY;
                v2 -= adjustY;
            }

            this.U  = u;
            this.V  = v;
            this.U2 = u2;
            this.V2 = v2;
        }

        public void SetRegion( TextureRegion region )
        {
            Texture = region.Texture;
            SetRegion( region.U, region.V, region.U2, region.V2 );
        }

        public void SetRegion( TextureRegion region, int x, int y, int width, int height )
        {
            Texture = region.Texture;
            SetRegion( region.RegionX + x, region.RegionY + y, width, height );
        }

        public Texture Texture { get; set; }

        public float U
        {
            get => _u;
            set
            {
                _u          = value;
                RegionWidth = ( int )System.Math.Round( System.Math.Abs( _u2 - _u ) * Texture.Width );
            }
        }

        public float U2
        {
            get => _u2;
            set
            {
                _u2         = value;
                RegionWidth = ( int )System.Math.Round( System.Math.Abs( _u2 - _u ) * Texture.Width );
            }
        }

        public float V
        {
            get => _v;
            set
            {
                _v           = value;
                RegionHeight = ( int )System.Math.Round( System.Math.Abs( _v2 - _v ) * Texture.Height );
            }
        }

        public float V2
        {
            get => _v2;
            set
            {
                _v2          = value;
                RegionHeight = ( int )System.Math.Round( System.Math.Abs( _v2 - _v ) * Texture.Height );
            }
        }

        public int RegionX
        {
            get => ( int )System.Math.Round( U * Texture.Width );
            set => U = ( value / ( float )Texture.Width );
        }

        public int RegionY
        {
            get => ( int )System.Math.Round( V * Texture.Height );
            set => V = ( value / ( float )Texture.Height );
        }

        public int RegionWidth
        {
            get => _regionWidth;
            set
            {
                if ( IsFlipX() )
                {
                    U = ( U2 + value / ( float )Texture.Width );
                }
                else
                {
                    U2 = ( U + value / ( float )Texture.Width );
                }
            }
        }

        public int RegionHeight
        {
            get => _regionHeight;
            set
            {
                if ( IsFlipY() )
                {
                    V = ( V2 + value / ( float )Texture.Height );
                }
                else
                {
                    V2 = ( V + value / ( float )Texture.Height );
                }
            }
        }

        public void Flip( bool x, bool y )
        {
            if ( x )
            {
                ( U, U2 ) = ( U2, U );
            }

            if ( y )
            {
                ( V, V2 ) = ( V2, V );
            }
        }

        public bool IsFlipX() => ( U > U2 );
        public bool IsFlipY() => ( V > V2 );

        /// <summary>
        /// Offsets the region relative to the current region. Generally the region's
        /// size should be the entire size of the texture in the direction(s) it is
        /// scrolled.
        /// </summary>
        /// <param name="xAmount">The percentage to offset horizontally.</param>
        /// <param name="yAmount">
        /// The percentage to offset vertically.
        /// This is done in texture space, so up is negative.
        /// </param>
        public void Scroll( float xAmount, float yAmount )
        {
            if ( xAmount != 0 )
            {
                var width = ( U2 - U ) * Texture.Width;

                U  = ( U + xAmount ) % 1;
                U2 = U + width / Texture.Width;
            }

            if ( yAmount != 0 )
            {
                var height = ( V2 - V ) * Texture.Height;

                V  = ( V + yAmount ) % 1;
                V2 = V + height / Texture.Height;
            }
        }

        /// <summary>
        /// Helper function to create tiles out of this TextureRegion starting from the
        /// top left corner going to the right and ending at the bottom right corner.
        /// Only complete tiles will be returned so if the region's width or height are
        /// not a multiple of the tile width and height not all of the region will be
        /// used. This will not work on texture regions returned form a TextureAtlas that
        /// either have whitespace removed or where flipped before the region is split.
        /// </summary>
        /// <param name="tileWidth">Required tile's width in pixels.</param>
        /// <param name="tileHeight">Required tile's height in pixels.</param>
        /// <returns>A 2D array of TextureRegions index by [row, column].</returns>
        public TextureRegion[ , ] Split( int tileWidth, int tileHeight )
        {
            var x      = RegionX;
            var y      = RegionY;
            var width  = RegionWidth;
            var height = RegionHeight;

            var rows = height / tileHeight;
            var cols = width / tileWidth;

            var startX = x;
            var tiles  = new TextureRegion[ rows, cols ];

            for ( var row = 0; row < rows; row++, y += tileHeight )
            {
                x = startX;

                for ( var col = 0; col < cols; col++, x += tileWidth )
                {
                    tiles[ row, col ] = new TextureRegion( Texture, x, y, tileWidth, tileHeight );
                }
            }

            return tiles;
        }

        /// <summary>
        /// Helper function to create tiles out of the given Texture starting from the
        /// top left corner going to the right and ending at the bottom right corner.
        /// Only complete tiles will be returned so if the texture's width or height are
        /// not a multiple of the tile width and height not all of the texture will be used.
        /// </summary>
        /// <param name="texture">The texture to split.</param>
        /// <param name="tileWidth">Required tile's width in pixels.</param>
        /// <param name="tileHeight">Required tile's height in pixels.</param>
        /// <returns>A 2D array of TextureRegions index by [row, column].</returns>
        public static TextureRegion[ , ] Split( Texture texture, int tileWidth, int tileHeight )
        {
            var region = new TextureRegion( texture );

            return region.Split( tileWidth, tileHeight );
        }
    }
}
