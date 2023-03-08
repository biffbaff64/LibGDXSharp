namespace LibGDXSharp.Maps
{
    public class MapObject
    {
        private string        _name       = "";
        private float         _opacity    = 1.0f;
        private bool          _visible    = true;
        private MapProperties _properties = new MapProperties();
        private Color         _color      = Color.White;

        public string GetName()
        {
            return _name;
        }

        public void SetName( string name )
        {
            this._name = name;
        }

        public Color GetColor()
        {
            return _color;
        }

        public void SetColor( Color color )
        {
            this._color = color;
        }

        public float GetOpacity()
        {
            return _opacity;
        }

        public void SetOpacity( float opacity )
        {
            this._opacity = opacity;
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
    }
}

