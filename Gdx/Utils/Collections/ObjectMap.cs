using System.Collections;
using System.Diagnostics;

namespace LibGDXSharp.Utils.Collections
{
    public class ObjectMap<TK, TV> : IEnumerable< ObjectMap< TK, TV >.Entry< TK, TV > >
    {
        private static object _dummy = new object();

        public int Size;

        private TK[]? _keyTable;
        private TV[]? _valueTable;

        private float _loadFactor;
        private int   _threshold;

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
        protected int shift;

        /// <summary>
        /// A bitmask used to confine hashcodes to the size of the table. Must be all
        /// 1 bits in its low positions, ie a power of two minus 1.
        /// If <see cref="Place"/> is overriden, this can be used instead of <see cref="Shift"/>
        /// to isolate usable bits of a hash.
        /// </summary>
        protected int mask;

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

            public override string ToString()
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

            int tableSize = TableSize( initialCapacity, loadFactor );

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
            if ( map == null )
            {
                throw new ArgumentException( "supplied map is null!" );
            }

            this._loadFactor = map._loadFactor;

            Debug.Assert( map._keyTable != null, "map._keyTable != null" );
            Debug.Assert( map._valueTable != null, "map._valueTable != null" );

            int tableSize = TableSize( ( int )( map._keyTable.Length * map._loadFactor ), _loadFactor );

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
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected int Place( TK? item )
        {
            Debug.Assert( item != null, nameof( item ) + " != null" );

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

            Debug.Assert( _keyTable != null, nameof( _keyTable ) + " != null" );

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
                var oldValue = _valueTable![ i ];
                _valueTable[ i ] = value!;

                return oldValue;
            }

            i = -( i + 1 ); // Empty space was found.

            _keyTable![ i ]   = key;
            _valueTable![ i ] = value!;

            if ( ++Size >= _threshold ) Resize( _keyTable.Length << 1 );

            return default;
        }

        public void PutAll( ObjectMap<TK, TV> map)
        {
            EnsureCapacity( map.Size );
        
            var keyTable   = map._keyTable;
            var valueTable = map._valueTable;

            for ( int i = 0, n = keyTable.Length; i < n; i++ )
            {
                var key = keyTable[ i ];
                
                if ( key != null ) Put( key, valueTable[ i ] );
            }
        }

        public bool NotEmpty()
        {
            return Size > 0;
        }

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
