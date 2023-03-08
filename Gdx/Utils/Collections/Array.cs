using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.Math;

namespace LibGDXSharp.Utils.Collections
{
    [SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
    public class Array<T> //: IEnumerator< T >, IEnumerable
    {
        public T[]? Items   { get; private set; }
        public int  Size    { get; private set; }
        public bool Ordered { get; private set; }

        private ArrayIterable< T >?        _iterable;
        private PredicateIEnumerable< T >? _predicateIEnumerable;

        /// <summary>
        /// Creates a new Array with the specified initial capacity.
        /// </summary>
        /// <param name="ordered">
        /// If false, methods that remove elements may change the order of other
        /// elements in the array, which avoids a memory copy.
        /// </param>
        /// <param name="capacity">
        /// The initial capacity.
        /// Any elements added beyond this will cause the backing array to be grown.
        /// </param>
        public Array( bool ordered = true, int capacity = 16 )
        {
            Ordered = ordered;
            Items   = new T[ capacity ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public Array( Array< T > array ) : this( array.Ordered, array.Size )
        {
            Size = array.Size;

            Debug.Assert( Items != null, nameof( Items ) + " != null" );
            Debug.Assert( array.Items != null, nameof( array.Items ) + " != null" );

            Array.Copy( array.Items, 0, Items, 0, Size );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public Array( T[] array ) : this( true, array, 0, array.Length )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordered"></param>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        public Array( bool ordered, T[] array, int start, int count ) : this( ordered, count )
        {
            Size = count;

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            Array.Copy( array, start, Items, 0, Size );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Add( T value )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( Size == Items.Length )
            {
                Items = Resize( System.Math.Max( 8, ( int )( Size * 1.75f ) ) );
            }

            Items[ Size++ ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public void AddAll( Array< T > array )
        {
            AddAll( array, 0, array.Size );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddAll( Array< T > array, int start, int count )
        {
            if ( start + count > array.Size )
            {
                throw new ArgumentOutOfRangeException
                    ( "start + count must be <= size - " + start + " + " + count + " <= " + array.Size );
            }

            Debug.Assert( array.Items != null, "array.Items != null" );

            AddAll( array.Items, start, count );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public void AddAll( params T[] array )
        {
            AddAll( array, 0, array.Length );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        public void AddAll( T[] array, int start, int count )
        {
            var sizeNeeded = Size + count;

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( sizeNeeded > Items.Length )
            {
                Items = Resize( System.Math.Max( 8, ( int )( sizeNeeded * 1.75f ) ) );
            }

            Array.Copy( array, start, Items, Size, count );

            Size += count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T Get( int index )
        {
            if ( index >= Size )
            {
                throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            return Items[ index ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Set( int index, T value )
        {
            if ( index >= Size )
            {
                throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            Items[ index ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Insert( int index, T value )
        {
            if ( index > Size )
            {
                throw new ArgumentOutOfRangeException( "index can't be > size - " + index + " > " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( Size == Items.Length )
            {
                Items = Resize( System.Math.Max( 8, ( int )( Size * 1.75f ) ) );
            }

            if ( Ordered )
            {
                Array.Copy( Items, index, Items, index + 1, Size - index );
            }
            else
            {
                Items[ Size ] = Items[ index ];
            }

            Size++;
            Items[ index ] = value;
        }

        public void InsertRange( int index, int count )
        {
            if ( index > Size )
            {
                throw new IndexOutOfRangeException( "index can't be > size - " + index + " > " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            var sizeNeeded = Size + count;

            if ( sizeNeeded > Items.Length )
            {
                Items = Resize( System.Math.Max( System.Math.Max( 8, sizeNeeded ), ( int )( Size * 1.75f ) ) );
            }

            Array.Copy( Items, index, Items, index + count, Size - index );

            Size = sizeNeeded;
        }

        public void Swap( int first, int second )
        {
            if ( first >= Size )
            {
                throw new ArgumentOutOfRangeException( "first can't be >= size - " + first + " >= " + Size );
            }

            if ( second >= Size )
            {
                throw new ArgumentOutOfRangeException( "second can't be >= size - " + second + " >= " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            ( Items[ first ], Items[ second ] ) = ( Items[ second ], Items[ first ] );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains( T value )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            var i = Size - 1;

            while ( i >= 0 )
            {
                if ( Items[ i-- ]!.Equals( value ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf( T value )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( value == null )
            {
                for ( int i = 0, n = Size; i < n; i++ )
                {
                    if ( Items[ i ]!.Equals( value ) )
                    {
                        return i;
                    }
                }
            }
            else
            {
                for ( int i = 0, n = Size; i < n; i++ )
                {
                    if ( value.Equals( Items[ i ] ) )
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int LastIndexOf( T value )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            for ( var i = Size - 1; i >= 0; i-- )
            {
                if ( value!.Equals( Items[ i ] ) )
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
        /// <returns></returns>
        public bool RemoveValue( T value )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( value!.Equals( Items[ i ] ) )
                {
                    RemoveIndex( i );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T RemoveIndex( int index )
        {
            if ( index >= Size )
            {
                throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            var value = Items[ index ];

            Size--;

            if ( Ordered )
            {
                Array.Copy( Items, index + 1, Items, index, Size - index );
            }
            else
            {
                Items[ index ] = Items[ Size ];
            }

            Items[ Size ] = default( T )!;

            return value;
        }

        public void RemoveRange( int start, int end )
        {
            if ( end >= Size )
            {
                throw new ArgumentOutOfRangeException( "end can't be >= size - " + end + " >= " + Size );
            }

            if ( start > end )
            {
                throw new ArgumentOutOfRangeException( "start can't be > end - " + start + " > " + end );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            var count = end - start + 1;

            if ( Ordered )
            {
                Array.Copy( Items, start + count, Items, start, Size - ( start + count ) );
            }
            else
            {
                var lastIndex = this.Size - 1;

                for ( var i = 0; i < count; i++ )
                {
                    Items[ start + i ] = Items[ lastIndex - i ];
                }
            }

            Size -= count;
        }

        public bool RemoveAll( Array< T > array, bool identity )
        {
            var size      = this.Size;
            var startSize = size;

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            for ( int i = 0, n = array.Size; i < n; i++ )
            {
                var item = array.Get( i );

                for ( var ii = 0; ii < size; ii++ )
                {
                    if ( item!.Equals( Items[ ii ] ) )
                    {
                        RemoveIndex( ii );
                        size--;

                        break;
                    }
                }
            }

            return size != startSize;
        }

        /// <summary>
        /// Removes and returns the last item.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
        public T Pop()
        {
            if ( Size == 0 )
            {
                throw new IndexOutOfRangeException( "Array is empty." );
            }

            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            --Size;

            var item = Items[ Size ];

            Items[ Size ] = default!;

            return item;
        }

        /// <summary>
        /// Returns the last item in the array.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
        public T Peek()
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( Size == 0 )
            {
                throw new NullReferenceException( "Array is empty." );
            }

            return Items[ Size - 1 ];
        }

        /// <summary>
        /// Returns the first item in the array.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
        public T First()
        {
            if ( Size == 0 )
            {
                throw new NullReferenceException( "Array is empty." );
            }

            return Items[ 0 ];
        }

        public void Clear()
        {
            var localItems = this.Items;

            for ( int i = 0, n = Size; i < n; i++ )
            {
                localItems[ i ] = default( T );
            }

            Size = 0;
        }

        public T[] Shrink()
        {
            if ( Items.Length != Size )
            {
                Resize( Size );
            }

            return Items;
        }

        public T[] SetSize( int newSize )
        {
            Truncate( newSize );

            if ( newSize > Items.Length )
            {
                Resize( System.Math.Max( 8, newSize ) );
            }

            Size = newSize;

            return Items;
        }

        public T[] EnsureCapacity( int additionalCapacity )
        {
            var sizeNeeded = Size + additionalCapacity;

            if ( sizeNeeded > Items.Length )
            {
                Resize( System.Math.Max( 8, sizeNeeded ) );
            }

            return Items;
        }

        protected T[] Resize( int newSize )
        {
            var newItems = ( T[] )Array.CreateInstance( Items.GetType().BaseType, newSize );

            Array.Copy( Items, 0, newItems, 0, System.Math.Min( Size, newItems.Length ) );

            this.Items = newItems;

            return newItems;
        }

        public void Sort()
        {
            SortUtils.Instance().Sort( Items as object[], 0, Size );
        }

        public void Sort( IComparer< T > comparator )
        {
            SortUtils.Instance().Sort( Items, comparator, 0, Size );
        }

        public T SelectRanked( IComparer< T > comparator, int kthLowest )
        {
            if ( kthLowest < 1 )
            {
                throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
            }

            return Select< T >.Instance().Selecting( Items, comparator, kthLowest, Size );
        }

        public int SelectRankedIndex( IComparer< T > comparator, int kthLowest )
        {
            if ( kthLowest < 1 )
            {
                throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
            }

            return Select< T >.instance().SelectIndex( Items, comparator, kthLowest, Size );
        }

        /// <summary>
        /// Rearrange this array in reverse order.
        /// </summary>
        public void Reverse()
        {
            if ( Items == null ) return;

            for ( int i = 0, lastIndex = Size - 1, n = Size / 2; i < n; i++ )
            {
                var ii = lastIndex - i;

                ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
            }
        }

        /// <summary>
        /// Shuffle this array.
        /// </summary>
        public void Shuffle()
        {
            if ( Items == null ) return;

            for ( var i = Size - 1; i >= 0; i-- )
            {
                var ii = MathUtils.Random( i );

                ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator< T > Enumerator()
        {
            _iterable ??= new ArrayIterable< T >( this );

            return _iterable.Iterator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable< T > Select( IPredicate< T > predicate )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( _predicateIEnumerable == null )
            {
                _predicateIEnumerable = new PredicateIEnumerable< T >( Items, predicate );
            }
            else
            {
                _predicateIEnumerable.Set( Items, predicate );
            }

            return _predicateIEnumerable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSize"></param>
        public void Truncate( int newSize )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( ( Items == null ) || ( Size <= newSize ) ) return;

            for ( var i = newSize; i < Size; i++ )
            {
                Items[ i ] = default( T )!;
            }

            Size = newSize;
        }

        public T? Random()
        {
            if ( ( Items == null ) || ( Size == 0 ) )
            {
                return default( T );
            }

            return Items[ MathUtils.Random( 0, Size - 1 ) ];
        }

        public T[] ToArray()
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            var memberInfo = Items.GetType().BaseType;

            return memberInfo != null
                ? ToArray( memberInfo )
                : ( T[] )Array.CreateInstance( Items.GetType(), Size );
        }

        public T[] ToArray( Type type )
        {
            var result = ( T[] )Array.CreateInstance( type, Size );

            if ( Items != null )
            {
                Array.Copy( Items, 0, result, 0, Size );
            }

            return result;
        }

        public int HashCode()
        {
            if ( !Ordered || ( Items == null ) ) return base.GetHashCode();

            var h = 1;

            for ( int i = 0, n = Size; i < n; i++ )
            {
                h *= 31;

                var item = Items[ i ];

                if ( item != null )
                {
                    h += item.GetHashCode();
                }
            }

            return h;
        }

        public new bool Equals( object obj )
        {
            Debug.Assert( Items != null, nameof( Items ) + " != null" );

            if ( Items == null ) return false;

            if ( obj == this ) return true;

            if ( !Ordered ) return false;

            var array = ( Array< T > )obj;

            if ( !array.Ordered ) return false;

            var n = Size;

            if ( n != array.Size ) return false;

            var items1 = this.Items;
            var items2 = array.Items;

            if ( items2 == null ) return false;

            for ( var i = 0; i < n; i++ )
            {
                var o1 = items1[ i ];
                var o2 = items2[ i ];

                if ( !( o1?.Equals( o2 ) ?? o2 == null ) ) return false;
            }

            return true;
        }

        public IEnumerator< T > GetEnumerator()
        {
            return Enumerator();
        }

        public new string ToString()
        {
            if ( Size == 0 ) return "[]";
            if ( Items == null ) return "[]";

            var buffer = new StringBuilder( 32 );

            buffer.Append( '[' );
            buffer.Append( Items[ 0 ] );

            for ( var i = 1; i < Size; i++ )
            {
                buffer.Append( ", " );
                buffer.Append( Items[ i ] );
            }

            buffer.Append( ']' );

            return buffer.ToString();
        }

        public string ToString( string separator )
        {
            if ( Size == 0 ) return "";
            if ( Items == null ) return "";

            var buffer = new StringBuilder( 32 );

            buffer.Append( Items[ 0 ] );

            for ( var i = 1; i < Size; i++ )
            {
                buffer.Append( separator );
                buffer.Append( Items[ i ] );
            }

            return buffer.ToString();
        }
    }
}
