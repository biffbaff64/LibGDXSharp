using System.Collections;
using System.Diagnostics;

namespace LibGDXSharp.Utils.Collections
{
    public class ObjectMap<TK, TV> : IEnumerable< ObjectMap< TK, TV >.Entry< TK, TV > >
    {
        public int Size { get; set; }

        private TK[]? _keyTable;
        private TV[]? _valueTable;

        private readonly float _loadFactor;
        private readonly int   _threshold;

        /// <summary>
        /// Used by <see cref="Place"/> to bit shift the upper bits of a <code>long</code>
        /// into a usable range (greater than or equal to 0 and less than or equal to
        /// <see cref="mask"/>). The shift can be negative, which is convenient to match the
        /// number of bits in mask: if mask is a 7-bit number, a shift of -7 shifts the upper
        /// 7 bits into the lowest 7 positions. This class sets the shift &gt; 32 and &lt; 64,
        /// which if used with an int will still move the upper bits of an int to the lower bits.
        /// <see cref="mask"/>) can also be used to mask the low bits of a number, which may
        /// be faster for some hashcodes, if <see cref="Place"/> is overridden.
        /// </summary>
        protected readonly int shift;

        /// <summary>
        /// A bitmask used to confine hashcodes to the size of the table. Must be all
        /// 1 bits in its low positions, ie a power of two minus 1.
        /// If <see cref="Place"/> is overriden, this can be used instead of <see cref="Shift"/>
        /// to isolate usable bits of a hash.
        /// </summary>
        protected readonly int mask;

        [NonSerialized] private Entries? _entries1, _entries2;
        [NonSerialized] private Values?  _values1,  _values2;
        [NonSerialized] private Keys?    _keys1,    _keys2;

        /// <summary>
        /// </summary>
        /// <typeparam name="TKe"></typeparam>
        /// <typeparam name="TVe"></typeparam>
        public class Entry<TKe, TVe>
        {
            public TKe? key;
            public TVe? value;

            public override string Tostring()
            {
                return key + " = " + value;
            }
        }

        /// <summary>
        /// 
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
            mask       = tableSize - 1;
            shift      = int.LeadingZeroCount( mask );

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

            var tableSize = ObjectSet< TK >.TableSize( ( int )( map._keyTable.Length * map._loadFactor ), _loadFactor );

            _threshold = ( int )( tableSize * _loadFactor );
            mask       = tableSize - 1;
            shift      = int.LeadingZeroCount( mask );

            _keyTable   = new TK[ tableSize ];
            _valueTable = new TV[ tableSize ];

            Array.Copy( map._keyTable, 0, _keyTable, 0, map._keyTable.Length );
            Array.Copy( map._valueTable, 0, _valueTable, 0, map._valueTable.Length );

            this.Size = map.Size;
        }

        /// <summary>
        /// Returns an index greater than or equal to 0 and less than or equal
        /// to <see cref="mask"/> for the specified <code>item</code>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual int Place( TK item )
        {
            if ( item == null )
            {
                throw new ArgumentException( "item cannot be null!" );
            }

            return ( int )( ( ( ulong )item.GetHashCode() * 0x9E3779B97F4A7C15L ) >>> shift );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private int LocateKey( TK key )
        {
            if ( key == null ) throw new ArgumentException( "key cannot be null." );

            for ( var i = Place( key );; i = ( i + 1 ) & mask )
            {
                var other = _keyTable[ i ];

                if ( other == null ) return -( i + 1 ); // Empty space is available.

                if ( other.Equals( key ) ) return i; // Same key was found.
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TV? Put( TK key, TV? value )
        {
            var i = LocateKey( key );

            if ( i >= 0 )
            {
                // Existing key was found.
                var oldValue = _valueTable[ i ];
                _valueTable[ i ] = value;

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
        /// <remarks>Skips checks for existing keys.</remarks>
        /// <remarks>Doesn't increment Size.</remarks>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void PutResize( TK key, TV? value )
        {
            for ( var i = Place( key );; i = ( i + 1 ) & mask )
            {
                if ( _keyTable[ i ] == null )
                {
                    _keyTable[ i ]   = key;
                    _valueTable[ i ] = value!;

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

            return ( i < 0 ) ? default : _valueTable[ i ];
        }

        public TV? Get( TK key, TV? defaultValue )
        {
            var i = LocateKey( key );

            return i < 0 ? defaultValue : _valueTable[ i ];
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TV? Remove( TK key )
        {
            var i = LocateKey( key );

            if ( i < 0 ) return default;

            var oldValue = _valueTable[ i ];

            var next = ( i + 1 ) & mask;

            while ( ( key = _keyTable[ next ] ) != null )
            {
                var placement = Place( key );

                if ( ( ( next - placement ) & mask ) > ( ( i - placement ) & mask ) )
                {
                    _keyTable[ i ]   = key;
                    _valueTable[ i ] = _valueTable[ next ];

                    i = next;
                }

                next = ( next + 1 ) & mask;
            }

            _keyTable[ i ]   = default( TK )!;
            _valueTable[ i ] = default( TV )!;

            Size--;

            return oldValue;
        }

        public void Shrink( int maximumCapacity )
        {
            if ( maximumCapacity < 0 )
            {
                throw new ArgumentException( "maximumCapacity must be >= 0: " + maximumCapacity );
            }

            int tableSize = TableSize( maximumCapacity, _loadFactor );

            if ( _keyTable.Length > tableSize )
            {
                Resize( tableSize );
            }
        }

        public void clear( int maximumCapacity )
        {
            int tableSize = TableSize( maximumCapacity, _loadFactor );

            if ( _keyTable.Length <= tableSize )
            {
                clear();

                return;
            }

            Size = 0;

            resize( tableSize );
        }

        public void clear()
        {
            if ( Size == 0 ) return;
            
            Size = 0;
            
            Array.Fill( _keyTable, null );
            Array.Fill( _valueTable, null );
        }

        public bool containsValue( object value, bool identity )
        {
            V[] valueTable = this.valueTable;

            if ( value == null )
            {
                K[] keyTable = this.keyTable;

                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( keyTable[ i ] != null && valueTable[ i ] == null )
                        return true;
            }
            else if ( identity )
            {
                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( valueTable[ i ] == value )
                        return true;
            }
            else
            {
                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( value.equals( valueTable[ i ] ) )
                        return true;
            }

            return false;
        }

        public bool containsKey( K key )
        {
            return locateKey( key ) >= 0;
        }

        /** Returns the key for the specified value, or null if it is not in the map. Note this traverses the entire map and compares
	 * every value, which may be an expensive operation.
	 * @param identity If true, uses == to compare the specified value with values in the map. If false, uses
	 *           {@link #equals(Object)}. */
        public K findKey( object value, bool identity )
        {
            V[] valueTable = this.valueTable;

            if ( value == null )
            {
                K[] keyTable = this.keyTable;

                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( keyTable[ i ] != null && valueTable[ i ] == null )
                        return keyTable[ i ];
            }
            else if ( identity )
            {
                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( valueTable[ i ] == value )
                        return keyTable[ i ];
            }
            else
            {
                for ( int i = valueTable.length - 1; i >= 0; i-- )
                    if ( value.equals( valueTable[ i ] ) )
                        return keyTable[ i ];
            }

            return null;
        }

        /** Increases the size of the backing array to accommodate the specified number of additional items / loadFactor. Useful before
	 * adding many items to avoid multiple backing array resizes. */
        public void ensureCapacity( int additionalCapacity )
        {
            int tableSize = tableSize( size + additionalCapacity, loadFactor );
            if ( keyTable.length < tableSize ) resize( tableSize );
        }

        void resize( int newSize )
        {
            int oldCapacity = keyTable.length;
            threshold = ( int )( newSize * loadFactor );
            mask      = newSize - 1;
            shift     = Long.numberOfLeadingZeros( mask );

            K[] oldKeyTable   = keyTable;
            V[] oldValueTable = valueTable;

            keyTable   = ( K[] )new Object[ newSize ];
            valueTable = ( V[] )new Object[ newSize ];

            if ( size > 0 )
            {
                for ( int i = 0; i < oldCapacity; i++ )
                {
                    K key = oldKeyTable[ i ];
                    if ( key != null ) putResize( key, oldValueTable[ i ] );
                }
            }
        }

        public int hashCode()
        {
            int h          = size;
            K[] keyTable   = this.keyTable;
            V[] valueTable = this.valueTable;

            for ( int i = 0, n = keyTable.length; i < n; i++ )
            {
                K key = keyTable[ i ];

                if ( key != null )
                {
                    h += key.hashCode();
                    V value                = valueTable[ i ];
                    if ( value != null ) h += value.hashCode();
                }
            }

            return h;
        }

        public bool equals( object obj )
        {
            if ( obj == this ) return true;
            if ( !( obj instanceof ObjectMap)) return false;
            ObjectMap              other = ( ObjectMap )obj;

            if ( other.size != size ) return false;
            K[] keyTable   = this.keyTable;
            V[] valueTable = this.valueTable;

            for ( int i = 0, n = keyTable.length; i < n; i++ )
            {
                K key = keyTable[ i ];

                if ( key != null )
                {
                    V value = valueTable[ i ];

                    if ( value == null )
                    {
                        if ( other.get( key, dummy ) != null ) return false;
                    }
                    else
                    {
                        if ( !value.equals( other.get( key ) ) ) return false;
                    }
                }
            }

            return true;
        }

        /** Uses == for comparison of each value. */
        public bool equalsIdentity( object obj )
        {
            if ( obj == this ) return true;
            if ( !( obj instanceof ObjectMap)) return false;
            ObjectMap              other = ( ObjectMap )obj;

            if ( other.size != size ) return false;
            K[] keyTable   = this.keyTable;
            V[] valueTable = this.valueTable;

            for ( int i = 0, n = keyTable.length; i < n; i++ )
            {
                K key = keyTable[ i ];

                if ( key != null && valueTable[ i ] != other.get( key, dummy ) ) return false;
            }

            return true;
        }

        public string tostring( string separator )
        {
            return tostring( separator, false );
        }

        public new string Tostring()
        {
            return Tostring( ", ", true );
        }

        protected string Tostring( string separator, bool braces )
        {
            if ( Size == 0 ) return braces ? "{}" : "";

            stringBuilder buffer = new stringBuilder( 32 );

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

        public Entries< K, V > iterator()
        {
            return entries();
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

        public IEnumerator< Entry< TK, TV > > GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Entries
    {
    }

    internal class Values
    {
    }

    internal class Keys
    {
    }
}
