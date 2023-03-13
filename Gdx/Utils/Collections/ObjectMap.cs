using System.Collections;
using System.Diagnostics;

namespace LibGDXSharp.Utils.Collections
{
    public class ObjectMap<TK, TV> : IEnumerable< ObjectMap< TK, TV >.Entry< TK, TV > >
    {
        public int Size { get; set; }

        internal TK[]? _keyTable;
        internal TV[]? _valueTable;

        private float _loadFactor;
        private int   _threshold;

        private readonly object? _dummy = new object();

        /// <summary>
        /// Used by <see cref="Place"/> to bit shift the upper bits of a <code>long</code>
        /// into a usable range (greater than or equal to 0 and less than or equal to
        /// <see cref="_mask"/>). The shift can be negative, which is convenient to match the
        /// number of bits in mask: if mask is a 7-bit number, a shift of -7 shifts the upper
        /// 7 bits into the lowest 7 positions. This class sets the shift &gt; 32 and &lt; 64,
        /// which if used with an int will still move the upper bits of an int to the lower bits.
        /// <see cref="_mask"/>) can also be used to mask the low bits of a number, which may
        /// be faster for some hashcodes, if <see cref="Place"/> is overridden.
        /// </summary>
        private int _shift;

        /// <summary>
        /// A bitmask used to confine hashcodes to the size of the table. Must be all
        /// 1 bits in its low positions, ie a power of two minus 1.
        /// If <see cref="Place"/> is overriden, this can be used instead of <see cref="Shift"/>
        /// to isolate usable bits of a hash.
        /// </summary>
        internal int _mask;

        [NonSerialized] private Entries? _entries1;
        [NonSerialized] private Entries? _entries2;
        [NonSerialized] private Values?  _values1;
        [NonSerialized] private Values?  _values2;
        [NonSerialized] private Keys?    _keys1;
        [NonSerialized] private Keys?    _keys2;

        /// <summary>
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="loadFactor"></param>
        public ObjectMap( int initialCapacity = 51, float loadFactor = 0.8f )
        {
            if ( loadFactor is <= 0f or >= 1f )
            {
                throw new ArgumentException( "loadFactor must be > 0 and < 1: " + loadFactor );
            }

            this._loadFactor = loadFactor;

            var tableSize = ObjectSet< TK >.TableSize( initialCapacity, loadFactor );

            _threshold = ( int )( tableSize * loadFactor );
            _mask      = tableSize - 1;
            _shift     = int.LeadingZeroCount( _mask );

            _keyTable   = new TK[ tableSize ];
            _valueTable = new TV[ tableSize ];
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="map"></param>
        /// <exception cref="ArgumentException"></exception>
        public ObjectMap( ObjectMap< TK, TV > map )
        {
            if ( map == null ) throw new ArgumentException( "supplied map is null!" );

            if ( map._valueTable == null )
            {
                throw new ArgumentException( "supplied map._valuetable is null!" );
            }

            this._loadFactor = map._loadFactor;

            var tableSize = ObjectSet< TK >.TableSize
                (
                 ( int )( map._keyTable!.Length * map._loadFactor ),
                 _loadFactor
                );

            _threshold = ( int )( tableSize * _loadFactor );
            _mask      = tableSize - 1;
            _shift     = int.LeadingZeroCount( _mask );

            _keyTable   = new TK[ tableSize ];
            _valueTable = new TV[ tableSize ];

            Array.Copy( map._keyTable, 0, _keyTable, 0, map._keyTable.Length );
            Array.Copy( map._valueTable, 0, _valueTable, 0, map._valueTable.Length );

            this.Size = map.Size;
        }

        /// <summary>
        /// Returns an index greater than or equal to 0 and less than or equal
        /// to <see cref="_mask"/> for the specified <code>item</code>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int Place( TK item )
        {
            if ( item == null )
            {
                throw new ArgumentException( "item cannot be null!" );
            }

            return ( int )( ( ( ulong )item.GetHashCode() * 0x9E3779B97F4A7C15L ) >>> _shift );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private int LocateKey( TK key )
        {
            if ( key == null ) throw new ArgumentException( "key cannot be null." );

            if ( _keyTable == null ) throw new NullReferenceException( "_keyTable is null" );

            for ( var i = Place( key );; i = ( i + 1 ) & _mask )
            {
                var other = _keyTable[ i ];

                if ( other == null ) return -( i + 1 ); // Empty space is available.

                if ( other.Equals( key ) ) return i; // Same key was found.
            }
        }

        /// <summary>
        /// Replaces the value associated with the specified key,
        /// and returns the old value.
        /// If the key is not found, the value is added at the end
        /// of the map and null is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TV? Put( TK key, TV? value )
        {
            if ( _keyTable == null ) throw new NullReferenceException( "Put(): _keyTable is null" );
            if ( _valueTable == null ) throw new NullReferenceException( "Put(): _valueTable is null" );

            var i = LocateKey( key );

            if ( i >= 0 )
            {
                // Existing key was found.
                var oldValue = _valueTable[ i ];
                _valueTable[ i ] = value!;

                return oldValue;
            }

            i = -( i + 1 ); // Empty space was found.

            _keyTable[ i ]   = key;
            _valueTable[ i ] = value!;

            if ( ++Size >= _threshold ) Resize( _keyTable.Length << 1 );

            return default;
        }

        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        public void PutAll( ObjectMap< TK, TV > map )
        {
            EnsureCapacity( map.Size );

            for ( int i = 0, n = _keyTable!.Length; i < n; i++ )
            {
                var key = _keyTable[ i ];

                if ( key != null ) Put( key, _valueTable![ i ] );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="newSize"></param>
        void Resize( int newSize )
        {
            var oldCapacity = _keyTable!.Length;
            _threshold = ( int )( newSize * _loadFactor );

            _mask  = newSize - 1;
            _shift = int.LeadingZeroCount( _mask );

            var oldKeyTable   = _keyTable;
            var oldValueTable = _valueTable;

            _keyTable   = new TK[ newSize ];
            _valueTable = new TV[ newSize ];

            if ( Size > 0 )
            {
                for ( var i = 0; i < oldCapacity; i++ )
                {
                    var key = oldKeyTable[ i ];

                    if ( key != null )
                    {
                        PutResize( key, oldValueTable![ i ] );
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>Skips checks for existing keys.</remarks>
        /// <remarks>
        /// Doesn't increment Size. This method is actually a utility
        /// method for <see cref="Resize"/>
        /// </remarks>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void PutResize( TK key, TV? value )
        {
            for ( var i = Place( key );; i = ( i + 1 ) & _mask )
            {
                if ( _keyTable![ i ] == null )
                {
                    _keyTable[ i ]    = key;
                    _valueTable![ i ] = value!;

                    return;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TT"></typeparam>
        /// <returns></returns>
        public TV? Get<TT>( TT key ) where TT : TK
        {
            var i = LocateKey( key );

            return ( i < 0 ) ? default : _valueTable![ i ];
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TV? Get( TK key, TV? defaultValue )
        {
            var i = LocateKey( key );

            return i < 0 ? defaultValue : _valueTable![ i ];
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TV? Remove( TK key )
        {
            if ( _keyTable == null ) throw new NullReferenceException( "Remove(): _keyTable is null" );
            if ( _valueTable == null ) throw new NullReferenceException( "Remove(): _valueTable is null" );

            var i = LocateKey( key );

            if ( i < 0 ) return default;

            var oldValue = _valueTable[ i ];

            var next = ( i + 1 ) & _mask;

            while ( ( key = _keyTable[ next ] ) != null )
            {
                var placement = Place( key );

                if ( ( ( next - placement ) & _mask ) > ( ( i - placement ) & _mask ) )
                {
                    _keyTable[ i ]   = key;
                    _valueTable[ i ] = _valueTable[ next ];

                    i = next;
                }

                next = ( next + 1 ) & _mask;
            }

            _keyTable[ i ]   = default( TK )!;
            _valueTable[ i ] = default( TV )!;

            Size--;

            return oldValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="maximumCapacity"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Shrink( int maximumCapacity )
        {
            if ( maximumCapacity < 0 )
            {
                throw new ArgumentException( "maximumCapacity must be >= 0: " + maximumCapacity );
            }

            int tableSize = TableSize( maximumCapacity, _loadFactor );

            Debug.Assert( _keyTable != null, nameof( _keyTable ) + " != null" );

            if ( _keyTable.Length > tableSize )
            {
                Resize( tableSize );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="maximumCapacity"></param>
        public void Clear( int maximumCapacity )
        {
            int tableSize = TableSize( maximumCapacity, _loadFactor );

            Debug.Assert( _keyTable != null, nameof( _keyTable ) + " != null" );

            if ( _keyTable.Length <= tableSize )
            {
                Clear();

                return;
            }

            Size = 0;

            Resize( tableSize );
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            if ( Size == 0 ) return;

            Size = 0;

            Debug.Assert( _keyTable != null, nameof( _keyTable ) + " != null" );
            Debug.Assert( _valueTable != null, nameof( _valueTable ) + " != null" );

            Array.Clear( _keyTable );
            Array.Clear( _valueTable );
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool ContainsValue( object? value, bool identity )
        {
            Debug.Assert( _keyTable != null, nameof( _keyTable ) + " != null" );
            Debug.Assert( _valueTable != null, nameof( _valueTable ) + " != null" );

            if ( value == null )
            {
                for ( var i = _valueTable.Length - 1; i >= 0; i-- )
                {
                    if ( _keyTable[ i ] != null && _valueTable[ i ] == null )
                    {
                        return true;
                    }
                }
            }
            else
            {
                for ( var i = _valueTable.Length - 1; i >= 0; i-- )
                {
                    if ( value.Equals( _valueTable[ i ] ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey( TK key )
        {
            return LocateKey( key ) >= 0;
        }

        /// <summary>
        /// Returns the key for the specified value, or null if it is not in the map.
        /// Note this traverses the entire map and compares every value, which may be
        /// an expensive operation.
        /// </summary>
        public TK? FindKey( object? value )
        {
            if ( value == null )
            {
                for ( var i = _valueTable!.Length - 1; i >= 0; i-- )
                {
                    if ( _keyTable![ i ] != null && _valueTable[ i ] == null )
                    {
                        return _keyTable[ i ];
                    }
                }
            }
            else
            {
                for ( var i = _valueTable!.Length - 1; i >= 0; i-- )
                {
                    if ( value.Equals( _valueTable[ i ] ) )
                    {
                        return _keyTable![ i ];
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// Increases the size of the backing array to accommodate the specified number
        /// of additional items / loadFactor. Useful before adding many items to avoid
        /// multiple backing array resizes.
        /// </summary>
        public void EnsureCapacity( int additionalCapacity )
        {
            int tableSize = TableSize( Size + additionalCapacity, _loadFactor );

            if ( _keyTable!.Length < tableSize )
            {
                Resize( tableSize );
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public int HashCode()
        {
            var h = Size;

            for ( int i = 0, n = _keyTable!.Length; i < n; i++ )
            {
                var key = _keyTable[ i ];

                if ( key != null )
                {
                    h += key.HashCode();

                    var value = _valueTable![ i ];

                    if ( value != null )
                    {
                        h += value.HashCode();
                    }
                }
            }

            return h;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public new bool Equals( object obj )
        {
            if ( obj == this ) return true;

            if ( obj is not ObjectMap< TK, TV > )
            {
                return false;
            }

            var other = ( ObjectMap< TK, TV > )obj;

            if ( other.Size != Size ) return false;

            for ( int i = 0, n = _keyTable!.Length; i < n; i++ )
            {
                var key = _keyTable[ i ];

                if ( key != null )
                {
                    var value = _valueTable![ i ];

                    if ( value == null )
                    {
                        if ( other.Get( key, ( TV? )_dummy ) != null ) return false;
                    }
                    else
                    {
                        if ( !value.Equals( other.Get( key ) ) ) return false;
                    }
                }
            }

            return true;
        }

        public string ToString( string separator )
        {
            return ToString( separator, false );
        }

        public new string ToString()
        {
            return ToString( ", ", true );
        }

        protected string ToString( string separator, bool braces )
        {
            if ( Size == 0 ) return braces ? "{}" : "";

            var buffer = new StringBuilder( 32 );

            if ( braces ) buffer.Append( '{' );

            var keyTable   = this._keyTable;
            var valueTable = this._valueTable;

            int i = _keyTable.Length;

            while ( i-- > 0 )
            {
                K key = keyTable[ i ];

                if ( key == null ) continue;
                buffer.append( key == this ? "(this)" : key );
                buffer.append( '=' );
                V value = valueTable[ i ];
                buffer.append( value == this ? "(this)" : value );

                break;
            }

            while ( i-- > 0 )
            {
                K key = keyTable[ i ];

                if ( key == null ) continue;
                buffer.append( separator );
                buffer.append( key == this ? "(this)" : key );
                buffer.append( '=' );
                V value = valueTable[ i ];
                buffer.append( value == this ? "(this)" : value );
            }

            if ( braces ) buffer.append( '}' );

            return buffer.tostring();
        }

        /// <summary>
        /// Helper method.
        /// </summary>
        /// <returns>TRUE if Size is greater than zero.</returns>
        public bool NotEmpty()
        {
            return Size > 0;
        }

        /// <summary>
        /// Helper method.
        /// </summary>
        /// <returns>TRUE if Size is zero.</returns>
        public bool IsEmpty()
        {
            return Size == 0;
        }

        public Keys< TK > Keys()
        {
            if ( Collections.AllocateIterators ) return new Keys( this );

            if ( keys1 == null )
            {
                keys1 = new Keys( this );
                keys2 = new Keys( this );
            }

            if ( !keys1.valid )
            {
                keys1.reset();
                keys1.valid = true;
                keys2.valid = false;

                return keys1;
            }

            keys2.reset();
            keys2.valid = true;
            keys1.valid = false;

            return keys2;
        }

        public IEnumerator< Entry< TK, TV > > GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKe"></typeparam>
    /// <typeparam name="TVe"></typeparam>
    public class Entry<TKe, TVe>
    {
        public TKe? key;
        public TVe? value;

        public override string ToString()
        {
            return key + " = " + value;
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKm"></typeparam>
    /// <typeparam name="TVm"></typeparam>
    /// <typeparam name="TI"></typeparam>
    public abstract class MapIterator<TKm, TVm, TI> //: Iterable<TI>, Iterator<TI>
    {
        protected bool                  hasNext;
        protected ObjectMap< TKm, TVm > map;
        protected bool                  valid        = true;
        protected int                   nextIndex    = 0;
        protected int                   currentIndex = 0;

        protected MapIterator( ObjectMap< TKm, TVm > map )
        {
            this.map = map;
            Reset();
        }

        public void Reset()
        {
            currentIndex = -1;
            nextIndex    = -1;

            FindNextIndex();
        }

        protected void FindNextIndex()
        {
            for ( var n = map._keyTable!.Length; ++nextIndex < n; )
            {
                if ( map._keyTable[ nextIndex ] != null )
                {
                    hasNext = true;

                    return;
                }
            }

            hasNext = false;
        }

        public void Remove()
        {
            var i = currentIndex;

            if ( i < 0 ) throw new IllegalStateException( "next must be called before remove." );

            var mask = map._mask;
            var next = ( i + 1 ) & mask;
            TKm key;

            while ( ( key = map._keyTable![ next ] ) != null )
            {
                var placement = map.Place( key );

                if ( ( ( next - placement ) & mask ) > ( ( i - placement ) & mask ) )
                {
                    map._keyTable[ i ]    = key;
                    map._valueTable![ i ] = map._valueTable[ next ];

                    i = next;
                }

                next = ( next + 1 ) & mask;
            }

            map._keyTable[ i ]    = default!;
            map._valueTable![ i ] = default!;

            map.Size--;

            if ( i != currentIndex )
            {
                --nextIndex;
            }

            currentIndex = -1;
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKe"></typeparam>
    /// <typeparam name="TVe"></typeparam>
    public class Entries<TKe, TVe> : MapIterator< TKe, TVe, Entry< TKe, TVe > >
    {
        private readonly Entry< TKe, TVe > _entry = new Entry< TKe, TVe >();

        public Entries( ObjectMap< TKe, TVe > map ) : base( map )
        {
        }

        /// <summary>
        /// Note the same entry instance is returned each time this method is called.
        /// </summary>
        public Entry< TKe, TVe > Next()
        {
            if ( !hasNext ) throw new NoSuchElementException();

            if ( !valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

            _entry.key   = map._keyTable![ nextIndex ];
            _entry.value = map._valueTable![ nextIndex ];

            currentIndex = nextIndex;

            FindNextIndex();

            return _entry;
        }

        public bool HasNext()
        {
            if ( !valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

            return hasNext;
        }

        public Entries< TKe, TVe > Iterator()
        {
            return this;
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TVv"></typeparam>
    public class Values<TVv> : MapIterator< object, TVv, TVv >
    {
    }

    /// <summary>
    /// </summary>
    public class Keys<TKk> : MapIterator< TKk, object, TKk >
    {
        public Keys( ObjectMap< TKk, object > map ) : base( ( ObjectMap< TKk, object > )map )
        {
        }

        public bool HasNext()
        {
            if ( !valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

            return hasNext;
        }

        public TKk Next()
        {
            if ( !hasNext ) throw new NoSuchElementException();
            if ( !valid ) throw new GdxRuntimeException( "#iterator() cannot be used nested." );

            var key = map._keyTable![ nextIndex ];

            currentIndex = nextIndex;

            FindNextIndex();

            return key;
        }

        public Keys< TKk > Iterator()
        {
            return this;
        }

        /// <summary>
        /// Returns a new array containing the remaining keys.
        /// </summary>
        public Array< TKk > ToArray()
        {
            return ToArray( new Array< TKk >( true, map.Size ) );
        }

        /// <summary>
        /// Adds the remaining keys to the array.
        /// </summary>
        public Array< TKk > ToArray( Array< TKk > array )
        {
            while ( base.hasNext )
            {
                array.Add( Next() );
            }

            return array;
        }
    }
}
