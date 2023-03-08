using System.Collections;

namespace LibGDXSharp.Maps
{
    public class MapObjects : IEnumerable<MapObject>
    {
        private readonly List< MapObject > _objects;

        public MapObjects()
        {
            _objects = new List< MapObject >();
        }

        public MapObject Get( int index )
        {
            return _objects[ index ];
        }

        public MapObject Get( string name )
        {
            for ( int i = 0, n = _objects.Count; i < n; i++ )
            {
                var obj = _objects[ i ];

                if ( name.Equals( obj.GetName() ) )
                {
                    return obj;
                }
            }

            return null!;
        }

        public int GetIndex( string name )
        {
            return GetIndex( Get( name ) );
        }

        public int GetIndex( MapObject obj )
        {
            return _objects.IndexOf( obj );
        }

        public int GetCount()
        {
            return _objects.Count;
        }

        public void Add( MapObject obj )
        {
            _objects.Add( obj );
        }

        public void Remove( int index )
        {
            _objects.RemoveAt( index );
        }

        public void Remove( MapObject obj )
        {
            _objects.Remove( obj );
        }

        public List< T > GetByType<T>() where T : MapObject
        {
            return GetByType( new List< T >() );
        }

        public List< T > GetByType<T>( List< T > fill ) where T : MapObject
        {
            fill.Clear();
            fill.AddRange( _objects.OfType<T>() );

            return fill;
        }

        public IEnumerator< MapObject > GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

