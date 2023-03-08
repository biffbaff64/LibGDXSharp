using System.Collections;
using System.Diagnostics;
using System.Text;

using LibGDXSharp.Math;

namespace LibGDXSharp.Utils.Collections
{
    /// <summary>
    /// An ordered or unordered map of objects.
    /// This implementation uses arrays to store the keys and values, which means
    /// gets do a comparison for each key in the map. This is slower than a typical
    /// hash map implementation, but may be acceptable for small maps and has the
    /// benefits that keys and values can be accessed by index, which makes iteration
    /// fast. Like Array, if ordered is false, this class avoids a memory copy when
    /// removing elements (the last element is moved to the removed element's position).
    /// </summary>
    public class ArrayMap<TK, TV> : IEnumerable< ObjectMap.Entry< TK, TV > >
    {
        public TK?[] MapKeys   { get; set; }
        public TV?[] MapValues { get; set; }
        public int   Size      { get; set; }
        public bool  IsOrdered { get; set; }

        [NonSerialized] private Entries< TK, TV >? _entries1, _entries2;
        [NonSerialized] private Values< TV >?      _values1,  _values2;
        [NonSerialized] private Keys< TK >?        _keys1,    _keys2;

        /// <summary>
        /// Creates a new ordered, or unordered, ArrayMapWIP with the specified initial capacity.
        /// Default capacity is 16. Default ordering is 'ordered'.
        /// </summary>
        /// <param name="ordered">
        /// If false, methods that remove elements may change the order of
        /// other elements in the arrays, which avoids a memory copy.
        /// </param>
        /// <param name="capacity">
        /// Any elements added beyond this will cause the backing arrays to be grown.
        /// </param>
        public ArrayMap( bool ordered = true, int capacity = 16 )
        {
            Size = capacity;

            IsOrdered = ordered;
            MapKeys   = new TK[ capacity ];
            MapValues = new TV[ capacity ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public ArrayMap( ArrayMap< TK, TV > array ) : this( array.IsOrdered, array.Size )
        {
            Debug.Assert( MapKeys != null );
            Debug.Assert( array.MapKeys != null );
            Debug.Assert( MapValues != null );
            Debug.Assert( array.MapValues != null );

            Array.Copy( array.MapKeys, MapKeys, array.MapKeys.Length );
            Array.Copy( array.MapValues, MapValues, array.MapValues.Length );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyArray"></param>
        /// <param name="valueArray"></param>
        /// <param name="ordered"></param>
        /// <param name="capacity"></param>
        public ArrayMap( Type keyArray, Type valueArray, bool ordered = true, int capacity = 16 )
        {
            MapKeys   = ( TK[] )Array.CreateInstance( keyArray, capacity );
            MapValues = ( TV[] )Array.CreateInstance( valueArray, capacity );

            Size      = capacity;
            IsOrdered = ordered;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Put( TK key, TV value )
        {
            var index = IndexOfKey( key );

            if ( index < 0 )
            {
                if ( Size == MapKeys.Length )
                {
                    Resize( System.Math.Max( 8, ( int )( Size * 1.75f ) ) );
                    index = Size++;
                }
            }

            MapKeys[ index ]   = key;
            MapValues[ index ] = value;

            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int PutAt( TK key, TV value, int index )
        {
            var existingIndex = IndexOfKey( key );

            if ( existingIndex >= 0 )
            {
                RemoveIndex( existingIndex );
            }
            else if ( Size == MapKeys.Length )
            {
                Resize( System.Math.Max( 8, ( int )( Size * 1.75f ) ) );
            }

            Array.Copy( MapKeys, index, MapKeys, index + 1, Size - index );
            Array.Copy( MapValues, index, MapValues, index + 1, Size - index );

            MapKeys[ index ]   = key;
            MapValues[ index ] = value;

            Size++;

            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public virtual void PutAll<T1, T2>( ArrayMap< T1, T2 > map ) where T1 : TK where T2 : TV
        {
            PutAll( map, 0, map.Size );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void PutAll<T1, T2>( ArrayMap< T1, T2 > map, int offset, int length ) where T1 : TK where T2 : TV
        {
            if ( offset + length > map.Size )
            {
                throw new ArgumentException
                    (
                     "offset + length must be <= size - " + offset + " + " + length + " <= " + map.Size
                    );
            }

            var sizeNeeded = Size + length - offset;

            if ( sizeNeeded >= MapKeys.Length )
            {
                Resize( System.Math.Max( 8, ( int )( sizeNeeded * 1.75f ) ) );
            }

            Array.Copy( map.MapKeys, offset, MapKeys, Size, length );
            Array.Copy( map.MapValues, offset, MapValues, Size, length );

            Size += length;
        }

        /// <summary>
        /// Returns the value (which may be null) for the specified key, or null
        /// if the key is not in the map. Note this does an .Equals() comparison
        /// of each key in reverse order until the specified key is found. 
        /// </summary>
        public virtual TV? Get( TK key )
        {
            return Get( key, default( TV ) );
        }

        /// <summary>
        /// Returns the value (which may be null) for the specified key, or the
        /// default value if the key is not in the map. Note this does an .Equals()
        /// comparison of each key in reverse order until the specified key is found. 
        /// </summary>
        protected virtual TV? Get( TK key, TV? defaultValue )
        {
            if ( key != null )
            {
                for ( var i = Size - 1; i >= 0; i-- )
                {
                    if ( key.Equals( MapKeys[ i ] ) )
                    {
                        return MapValues[ i ];
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the key for the specified value.
        /// Note this does an .Equals() comparison of each value in reverse
        /// order until the specified value is found.
        /// </summary>
        public virtual TK? GetKey( TV value )
        {
            if ( value != null )
            {
                for ( var i = Size - 1; i >= 0; i-- )
                {
                    if ( value.Equals( MapValues[ i ] ) )
                    {
                        return MapKeys[ i ];
                    }
                }
            }

            return default( TK );
        }

        /// <summary>
        /// Returns the key at the specified position.
        /// </summary>
        /// <param name="index">The required position.</param>
        /// <returns>The key, which can be null.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual TK? GetKeyAt( int index )
        {
            if ( index >= Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            return MapKeys[ index ];
        }

        /// <summary>
        /// Returns the value at the specified position.
        /// </summary>
        /// <param name="index">The required position.</param>
        /// <returns>The key, which can be null.</returns>
        public virtual TV? GetValueAt( int index )
        {
            if ( index >= Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            return MapValues[ index ];
        }

        /// <summary>
        /// Returns the key at position 0 in the map.
        /// </summary>
        /// <returns>The key, which can be null.</returns>
        public virtual TK? FirstKey()
        {
            if ( Size == 0 )
            {
                throw new System.InvalidOperationException( "Map is empty." );
            }

            return MapKeys[ 0 ];
        }

        /// <summary>
        /// Returns the value at position 0 in the map.
        /// </summary>
        /// <returns>The value, which can be null.</returns>
        public virtual TV? FirstValue()
        {
            if ( Size == 0 )
            {
                throw new System.InvalidOperationException( "Map is empty." );
            }

            return MapValues[ 0 ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual void SetKey( int index, TK key )
        {
            if ( index >= Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            MapKeys[ index ] = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual void SetValue( int index, TV value )
        {
            if ( index >= Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            MapValues[ index ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual void Insert( int index, TK key, TV value )
        {
            if ( index > Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            if ( Size == MapKeys.Length )
            {
                Resize( System.Math.Max( 8, ( int )( Size * 1.75f ) ) );
            }

            if ( IsOrdered )
            {
                Array.Copy( MapKeys, index, MapKeys, index + 1, Size - index );
                Array.Copy( MapValues, index, MapValues, index + 1, Size - index );
            }
            else
            {
                MapKeys[ Size ]   = MapKeys[ index ];
                MapValues[ Size ] = MapValues[ index ];
            }

            Size++;

            MapKeys[ index ]   = key;
            MapValues[ index ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey( TK key )
        {
            if ( key == null ) return false;

            var i = Size - 1;

            while ( i >= 0 )
            {
                if ( key.Equals( MapKeys[ i-- ] ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int IndexOfKey( TK key )
        {
            if ( key == null ) return -1;

            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( key.Equals( MapKeys[ i ] ) )
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public virtual int IndexOfValue( TV value )
        {
            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( value != null && value.Equals( MapValues[ i ] ) )
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TV? RemoveKey( TK key )
        {
            if ( key != null )
            {
                for ( int i = 0, n = Size; i < n; i++ )
                {
                    if ( key.Equals( MapKeys[ i ] ) )
                    {
                        var value = MapValues[ i ];
                        RemoveIndex( i );

                        return value;
                    }
                }
            }

            return default( TV );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool RemoveValue( TV value )
        {
            if ( value == null ) return false;

            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( value.Equals( MapValues[ i ] ) )
                {
                    RemoveIndex( i );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes and returns the key/values pair at the specified index.
        /// </summary>
        public virtual void RemoveIndex( int index )
        {
            if ( index >= Size )
            {
                throw new System.IndexOutOfRangeException( index.ToString() );
            }

            Size--;

            if ( IsOrdered )
            {
                Array.Copy( MapKeys, index + 1, MapKeys, index, Size - index );
                Array.Copy( MapValues, index + 1, MapValues, index, Size - index );
            }
            else
            {
                MapKeys[ index ]   = MapKeys[ Size ];
                MapValues[ index ] = MapValues[ Size ];
            }

            MapKeys[ Size ]   = default( TK );
            MapValues[ Size ] = default( TV );
        }

        /// <summary>
        /// Returns true if the map has one or more items.
        /// </summary>
        public virtual bool NotEmpty() => Size > 0;

        /// <summary>
        /// Returns true if the map is empty.
        /// </summary>
        public virtual bool Empty() => ( Size == 0 );

        /// <summary>
        /// Returns the last key.
        /// </summary>
        public virtual TK? PeekKey() => MapKeys[ Size - 1 ];

        /// <summary>
        /// Returns the last value. </summary>
        public virtual TV? PeekValue() => MapValues[ Size - 1 ];

        /// <summary>
        /// Clears the map and reduces the size of the backing arrays to
        /// be the specified capacity if they are larger.
        /// </summary>
        public virtual void Clear( int maximumCapacity )
        {
            if ( MapKeys.Length <= maximumCapacity )
            {
                Clear();

                return;
            }

            Size = 0;
            Resize( maximumCapacity );
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Clear()
        {
            Array.Clear( MapKeys, 0, Size );
            Array.Clear( MapValues, 0, Size );
            Size = 0;
        }

        /// <summary>
        /// Reduces the size of the backing arrays to the size of the actual
        /// number of entries. This is useful to release memory when many
        /// items have been removed, or if it is known that more entries
        /// will not be added. 
        /// </summary>
        public virtual void Shrink()
        {
            if ( MapKeys.Length == Size )
            {
                return;
            }

            Resize( Size );
        }

        /// <summary>
        /// Increases the size of the backing arrays to accommodate the specified
        /// number of additional entries. Useful before adding many entries to
        /// avoid multiple backing array resizes. 
        /// </summary>
        public virtual void EnsureCapacity( int additionalCapacity )
        {
            if ( additionalCapacity < 0 )
            {
                throw new System.ArgumentException( "additionalCapacity must be >= 0 - " + additionalCapacity );
            }

            var sizeNeeded = Size + additionalCapacity;

            if ( sizeNeeded > MapKeys.Length )
            {
                Resize( System.Math.Max( System.Math.Max( 8, sizeNeeded ), ( int )( Size * 1.75f ) ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSize"></param>
        protected virtual void Resize( int newSize )
        {
            var newKeys = ( TK?[] )Array.CreateInstance
                (
                 MapKeys.GetType().GetElementType()
                 ?? throw new NullReferenceException(),
                 newSize
                );

            Array.Copy( MapKeys, 0, newKeys, 0, System.Math.Min( Size, newKeys.Length ) );
            this.MapKeys = newKeys;

            var newValues = ( TV?[] )Array.CreateInstance
                (
                 MapValues.GetType().GetElementType()
                 ?? throw new NullReferenceException(),
                 newSize
                );

            Array.Copy( MapValues, 0, newValues, 0, System.Math.Min( Size, newValues.Length ) );
            this.MapValues = newValues;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Reverse()
        {
            for ( int i = 0, lastIndex = Size - 1, n = Size / 2; i < n; i++ )
            {
                var ii = lastIndex - i;

                ( MapKeys[ i ], MapKeys[ ii ] )     = ( MapKeys[ ii ], MapKeys[ i ] );
                ( MapValues[ i ], MapValues[ ii ] ) = ( MapValues[ ii ], MapValues[ i ] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Shuffle()
        {
            for ( var i = Size - 1; i >= 0; i-- )
            {
                var ii = MathUtils.Random( i );

                ( MapKeys[ i ], MapKeys[ ii ] )     = ( MapKeys[ ii ], MapKeys[ i ] );
                ( MapValues[ i ], MapValues[ ii ] ) = ( MapValues[ ii ], MapValues[ i ] );
            }
        }

        /// <summary>
        /// Reduces the size of the arrays to the specified size. If the arrays
        /// are already smaller than the specified size, no action is taken. 
        /// </summary>
        public virtual void Truncate( int newSize )
        {
            if ( Size <= newSize ) return;

            for ( var i = newSize; i < Size; i++ )
            {
                MapKeys[ i ]   = default;
                MapValues[ i ] = default;
            }

            Size = newSize;
        }

        // Java LibGDX hashCode() Method not implemented. Use object.GetHashCode().
        // Java LibGDX equals() Method not implemented. Use object.Equals().

        /// <summary>
        /// Uses == for comparison of each value.
        /// </summary>
        public virtual bool EqualsIdentity( object obj )
        {
            throw new NotImplementedException();

//            if ( obj == this )
//            {
//                return true;
//            }

//            if ( !( obj is ArrayMap ) )
//            {
//                return false;
//            }

//            ArrayMap other = ( ArrayMap )obj;

//            if ( other.size != size )
//            {
//                return false;
//            }

//            K[] keys   = this.keys;
//            V[] values = this.values;

//            for ( int i = 0, n = size; i < n; i++ )
//            {
//                if ( values[ i ] != other.get( keys[ i ], ObjectMap.dummy ) )
//                {
//                    return false;
//                }
//            }

//            return true;
        }

        public IEnumerator< ObjectMap.Entry< TK, TV > > GetEnumerator()
        {
            yield break;
        }

        public override string ToString()
        {
            if ( Size == 0 ) return "{}";

            var buffer = new StringBuilder( 32 );

            buffer.Append( '{' );
            buffer.Append( MapKeys[ 0 ] );
            buffer.Append( '=' );
            buffer.Append( MapValues[ 0 ] );

            for ( var i = 1; i < Size; i++ )
            {
                buffer.Append( ", " );
                buffer.Append( MapKeys[ i ] );
                buffer.Append( '=' );
                buffer.Append( MapValues[ i ] );
            }

            buffer.Append( '}' );

            return buffer.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //        public Entries< TK, TV > Entries()
//        {
//            if ( Collections.allocateIterators ) return new Entries( this );

//            if ( entries1 == null )
//            {
//                entries1 = new Entries( this );
//                entries2 = new Entries( this );
//            }

//            if ( !entries1.valid )
//            {
//                entries1.index = 0;
//                entries1.valid = true;
//                entries2.valid = false;

//                return entries1;
//            }

//            _entries2.index = 0;
//            _entries2.valid = true;
//            _entries1.valid = false;

//            return _entries2;
//        }

        public class Entries<TKe, TVe>
            : IEnumerable< ObjectMap.Entry< TKe, TVe > >, IEnumerator< ObjectMap.Entry< TKe, TVe > >
        {
            private readonly ArrayMap< TKe, TVe >        _map;
            private readonly ObjectMap.Entry< TKe, TVe > _entry;

            private int  _index;
            private bool _valid;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="map"></param>
            public Entries( ArrayMap< TKe, TVe > map )
            {
                this._map   = map;
                this._entry = new ObjectMap.Entry< TKe, TVe >();

                _valid = true;

                Current = _entry;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="GdxRuntimeException"></exception>
            public bool HasNext()
            {
                if ( !_valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

                return _index < _map.Size;
            }

            /// <summary>
            /// Note the same entry instance is returned each time this method is called.
            /// </summary>
            public ObjectMap.Entry< TKe, TVe > Next()
            {
                if ( _index >= _map.Size )
                {
                    throw new InvalidOperationException( _index.ToString() );
                }

                if ( !_valid )
                {
                    throw new GdxRuntimeException( "#iterator() cannot be used nested." );
                }

                _entry.key   = _map.MapKeys[ _index ];
                _entry.value = _map.MapValues[ _index++ ];

                return _entry;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Remove()
            {
                _index--;
                _map.RemoveIndex( _index );
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return false;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                _index = 0;
            }

            public ObjectMap.Entry< TKe, TVe > Current { get; }

            object IEnumerator.Current => Current;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerator< ObjectMap.Entry< TKe, TVe > > GetEnumerator()
            {
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
            }
        }

        public class Values<TVv> : IEnumerable< TVv >, IEnumerator< TVv >
        {
            private readonly ArrayMap< object, TVv > _map;

            private int  _index;
            private bool _valid = true;

            public Values( ArrayMap< object, TVv > map )
            {
                this._map = map;
            }

            public bool HasNext()
            {
                if ( !_valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

                return _index < _map.Size;
            }

            public TVv? Next()
            {
                if ( _index >= _map.Size ) throw new InvalidOperationException( _index.ToString() );
                if ( !_valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

                return _map.MapValues[ _index++ ];
            }

            public void Remove()
            {
                _index--;
                _map.RemoveIndex( _index );
            }

            public void Reset()
            {
                _index = 0;
            }

            public IEnumerator< TVv > GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool MoveNext()
            {
                return false;
            }

            public TVv Current { get; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }

        public class Keys<TKk> : IEnumerable< TKk >, IEnumerator< TKk >
        {
            private readonly ArrayMap< TKk, object > _map;

            private int  _index;
            private bool _valid = true;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="map"></param>
            public Keys( ArrayMap< TKk, object > map )
            {
                this._map = map;

                _index = 0;
                _valid = true;
                
                Debug.Assert( _map is not null );
                Debug.Assert( _map.MapKeys is not null );
                Debug.Assert( _map.MapKeys[ _index] is not null );
                
                Current = _map.MapKeys[ _index ] ?? throw new NullReferenceException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="GdxRuntimeException"></exception>
            public bool HasNext()
            {
                if ( !_valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

                return _index < _map.Size;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            /// <exception cref="GdxRuntimeException"></exception>
            public TKk? Next()
            {
                if ( _index >= _map.Size ) throw new InvalidOperationException( _index.ToString() );

                if ( !_valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

                return _map.MapKeys[ _index++ ];
            }

            /// <summary>
            /// 
            /// </summary>
            public void Remove()
            {
                _index--;
                _map.RemoveIndex( _index );
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return false;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                _index = 0;
            }

            /// <summary>
            /// 
            /// </summary>
            public TKk Current { get; }

            /// <summary>
            /// 
            /// </summary>
            object? IEnumerator.Current => Current;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            IEnumerator< TKk > IEnumerable< TKk >.GetEnumerator()
            {
                yield break;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerator< ObjectMap.Entry< TKk, TV > > GetEnumerator()
            {
                yield break;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                GC.SuppressFinalize( this );
            }
        }
    }
}
