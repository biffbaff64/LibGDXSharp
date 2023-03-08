using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled.Tiles
{
    public class StaticTiledMapTile : ITiledMapTile
    {
        private int            _id;
        private MapProperties? _properties;
        private MapObjects?    _mapObjects;
        private float          _offsetX;
        private float          _offsetY;
        private TextureRegion  _textureRegion;

        private ITiledMapTile.BlendMode _blendMode = ITiledMapTile.BlendMode.Alpha;

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

        public int GetId()
        {
            return _id;
        }

        public void SetId( int id )
        {
            this._id = id;
        }

        public ITiledMapTile.BlendMode GetBlendMode()
        {
            return _blendMode;
        }

        public void SetBlendMode( ITiledMapTile.BlendMode blendMode )
        {
            this._blendMode = blendMode;
        }

        public TextureRegion GetTextureRegion()
        {
            return _textureRegion;
        }

        public void SetTextureRegion( TextureRegion textureRegion )
        {
            this._textureRegion = textureRegion;
        }

        public float GetOffsetX()
        {
            return _offsetX;
        }

        public void SetOffsetX( float offsetX )
        {
            this._offsetX = offsetX;
        }

        public float GetOffsetY()
        {
            return _offsetY;
        }

        public void SetOffsetY( float offsetY )
        {
            this._offsetY = offsetY;
        }

        /// <summary>
        /// Creates a static tile with the given region
        /// </summary>
        /// <param name="texture">The <see cref="Texture2D"/> to use.</param>
        /// @param textureRegion the {@link TextureRegion} to use.
        public StaticTiledMapTile( TextureRegion texture )
        {
            this._textureRegion = texture;
        }

        /**
         * Copy constructor
	     * 
	     * @param copy the StaticTiledMapTile to copy.
         */
        public StaticTiledMapTile( StaticTiledMapTile copy )
        {
            if ( copy._properties != null )
            {
                GetProperties().PutAll( copy._properties );
            }

            this._mapObjects    = copy._mapObjects;
            this._textureRegion = copy._textureRegion;
            this._id             = copy._id;
        }
    }
}

