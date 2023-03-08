using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled.Tiles
{
    public class AnimatedTiledMapTile : ITiledMapTile
    {
        private static long _initialTimeOffset      = DateTime.Now.Millisecond;
        private static long _lastTiledMapRenderTime = 0;

        private int                     _id;
        private MapProperties?          _properties;
        private MapObjects?             _mapObjects;
        private int                     _loopDuration;
        private StaticTiledMapTile[]    _frameTiles;
        private ITiledMapTile.BlendMode _blendMode = ITiledMapTile.BlendMode.Alpha;
        private int[]                   _animationIntervals;

        /// <summary>
        /// Creates an animated tile with the given animation interval and frame tiles.
        /// </summary>
        /// <param name="interval">The interval between each individual frame tile.</param>
        /// <param name="frameTiles">
        /// An array of <see cref="StaticTiledMapTile"/> that make up the animation.
        /// </param>
        public AnimatedTiledMapTile( float interval, List< StaticTiledMapTile > frameTiles )
        {
            this._frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

            this._loopDuration       = frameTiles.Count * ( int )( interval * 1000f );
            this._animationIntervals = new int[ frameTiles.Count ];

            for ( var i = 0; i < frameTiles.Count; ++i )
            {
                this._frameTiles[ i ]         = frameTiles[ i ];
                this._animationIntervals[ i ] = ( int )( interval * 1000f );
            }
        }

        /// <summary>
        /// Creates an animated tile with the given animation intervals and frame tiles.
        /// </summary>
        /// <param name="intervals">
        /// The intervals between each individual frame tile in milliseconds.
        /// </param>
        /// <param name="frameTiles">
        /// An array of <see cref="StaticTiledMapTile"/> that make up the animation.
        /// </param>
        public AnimatedTiledMapTile( List< int > intervals, List< StaticTiledMapTile > frameTiles )
        {
            this._frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

            this._animationIntervals = intervals.ToArray();
            this._loopDuration       = 0;

            for ( int i = 0; i < intervals.Count; ++i )
            {
                this._frameTiles[ i ] =  frameTiles[ i ];
                this._loopDuration    += intervals[ i ];
            }
        }

        public ITiledMapTile GetCurrentFrame()
        {
            return _frameTiles[ GetCurrentFrameIndex() ];
        }

        public int GetCurrentFrameIndex()
        {
            var currentTime = ( int )( _lastTiledMapRenderTime % _loopDuration );

            for ( var i = 0; i < _animationIntervals.Length; ++i )
            {
                var animationInterval = _animationIntervals[ i ];

                if ( currentTime <= animationInterval )
                {
                    return i;
                }

                currentTime -= animationInterval;
            }

            throw new SystemException
                (
                 "Could not determine current animation frame in AnimatedTiledMapTile.  This should never happen."
                );
        }

        public static void UpdateAnimationBaseTime()
        {
            _lastTiledMapRenderTime = DateTime.Now.Millisecond - _initialTimeOffset;
        }

        public int GetId() => _id;

        public void SetId( int id ) => _id = id;

        public ITiledMapTile.BlendMode GetBlendMode() => _blendMode;

        public void SetBlendMode( ITiledMapTile.BlendMode blendMode ) => _blendMode = blendMode;

        public TextureRegion GetTextureRegion()
        {
            return GetCurrentFrame().GetTextureRegion();
        }

        public void SetTextureRegion( TextureRegion textureRegion )
        {
            // TODO: Illegal
        }

        public float GetOffsetX()
        {
            return GetCurrentFrame().GetOffsetX();
        }

        public void SetOffsetX( float offsetX )
        {
            // TODO: Illegal
        }

        public float GetOffsetY()
        {
            return GetCurrentFrame().GetOffsetY();
        }

        public void SetOffsetY( float offsetY )
        {
            // TODO: Illegal
        }

        public MapProperties GetProperties()
        {
            if ( _properties == null )
            {
                _properties = new MapProperties();
            }

            return _properties;
        }

        public MapObjects GetObjects()
        {
            if ( _mapObjects == null )
            {
                _mapObjects = new MapObjects();
            }

            return _mapObjects;
        }

        public StaticTiledMapTile[] GetFrameTiles() => _frameTiles;
    }
}

