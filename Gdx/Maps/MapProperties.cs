namespace LibGDXSharp.Maps
{
    public class MapProperties
    {
        private Dictionary< string, object > _properties;

        public MapProperties()
        {
            _properties = new Dictionary< string, object >();
        }

        public bool ContainsKey( string key )
        {
            return _properties.ContainsKey( key );
        }

        public object Get( string key )
        {
            return _properties[ key ];
        }

        public T Get<T>( string key )
        {
            return ( T )Get( key );
        }

        public T Get<T>( string key, T defaultValue )
        {
            var obj = Get( key );

            return ( T )obj;
        }

        public void Put( string key, object value )
        {
            _properties[ key ] = value;
        }

        public void PutAll( MapProperties properties )
        {
            _properties = new Dictionary<string, object>( properties._properties );
        }

        public void Remove( string key )
        {
            _properties.Remove( key );
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public Dictionary< string, object >.KeyCollection GetKeys()
        {
            return _properties.Keys;
        }

        public Dictionary< string, object >.ValueCollection GetValues()
        {
            return _properties.Values;
        }
    }
}

