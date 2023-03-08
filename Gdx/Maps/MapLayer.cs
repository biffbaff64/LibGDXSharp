namespace LibGDXSharp.Maps
{
    public class MapLayer
    {
        private string        _name    = "";
        private float         _opacity = 1.0f;
        private bool          _visible = true;
        private float         _offsetX;
        private float         _offsetY;
        private float         _renderOffsetX;
        private float         _renderOffsetY;
        private bool          _renderOffsetDirty = true;
        private MapLayer      _parent;
        
        private readonly MapObjects    _objects    = new MapObjects();
        private readonly MapProperties _properties = new MapProperties();

        public string GetName()
        {
            return _name;
        }

        public void SetName( string name )
        {
            this._name = name;
        }

        public float GetOpacity()
        {
            return _opacity;
        }

        public void SetOpacity( float opacity )
        {
            this._opacity = opacity;
        }

        public float GetOffsetX()
        {
            return _offsetX;
        }

        public void SetOffsetX( float offsetX )
        {
            this._offsetX = offsetX;
            InvalidateRenderOffset();
        }

        public float GetOffsetY()
        {
            return _offsetY;
        }

        public void SetOffsetY( float offsetY )
        {
            this._offsetY = offsetY;
            InvalidateRenderOffset();
        }

        public float GetRenderOffsetX()
        {
            if ( _renderOffsetDirty ) CalculateRenderOffsets();

            return _renderOffsetX;
        }

        public float GetRenderOffsetY()
        {
            if ( _renderOffsetDirty ) CalculateRenderOffsets();

            return _renderOffsetY;
        }

        public void InvalidateRenderOffset()
        {
            _renderOffsetDirty = true;
        }

        public MapLayer GetParent()
        {
            return _parent;
        }

        public void SetParent( MapLayer parent )
        {
            if ( parent == this ) throw new GdxRuntimeException( "Can't set self as the parent" );
            this._parent = parent;
        }

        public MapObjects GetObjects()
        {
            return _objects;
        }

        public bool IsVisible()
        {
            return _visible;
        }

        public void SetVisible( bool visible )
        {
            this._visible = visible;
        }

        public MapProperties GetProperties()
        {
            return _properties;
        }

        protected void CalculateRenderOffsets()
        {
            if ( _parent != null )
            {
                _parent.CalculateRenderOffsets();
                _renderOffsetX = _parent.GetRenderOffsetX() + _offsetX;
                _renderOffsetY = _parent.GetRenderOffsetY() + _offsetY;
            }
            else
            {
                _renderOffsetX = _offsetX;
                _renderOffsetY = _offsetY;
            }

            _renderOffsetDirty = false;
        }
    }
}
