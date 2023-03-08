
namespace LibGDXSharp.Utils.Collections
{
    /// <summary>
    /// An array that allows modification during iteration. Guarantees that
    /// array entries provided by begin() between indexes 0 and size at the
    /// time begin was called will not be modified until end() is called. If
    /// modification of the SnapshotArray occurs between begin/end, the backing
    /// array is copied prior to the modification, ensuring that the backing
    /// array that was returned by begin() is unaffected. To avoid allocation,
    /// an attempt is made to reuse any extra array created as a result of this
    /// copy on subsequent copies.
    /// Note that SnapshotArray is not for thread safety, only for modification
    /// during iteration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SnapshotArray<T> : Array< T >
    {
        private T[] _snapshot;
        private T[] _recycled;
        private int _snapshots;

        public SnapshotArray( int capacity = 0 ) : base( capacity )
        {
        }

        public SnapshotArray( Array< T > array ) : base( array )
        {
        }

        public SnapshotArray( bool ordered, int capacity = 0 ) : base( ordered, capacity )
        {
        }

        public SnapshotArray( bool ordered, int capacity, Type arrayType )
            : base( ordered, capacity, arrayType )
        {
        }

        public SnapshotArray( bool ordered, T[] array, int startIndex, int count )
            : base( ordered, array, startIndex, count )
        {
        }

        public SnapshotArray( T[] array ) : base( array )
        {
        }

        public T[] Begin()
        {
            if ( _snapshot == null )
            {
                throw new NullReferenceException( "Snapshot array should not be NULL in Begin()" );
            }

            Modified();

            CopyTo( _snapshot );

            _snapshots++;

            return ToArray();
        }

        public void End()
        {
            _snapshots = System.Math.Max( 0, _snapshots - 1 );

            if ( _snapshot == null ) return;

            if ( _snapshot != _items && _snapshots == 0 )
            {
                // The backing array was copied, keep around the old array.
                _recycled = _snapshot;

                for ( int i = 0, n = _recycled.Length; i < n; i++ )
                {
                    _recycled[ i ] = default;
                }
            }

            _snapshot = null;
        }

        private void Modified()
        {
            if ( _snapshot == null || _snapshot != _items ) return;

            // Snapshot is in use, copy backing array to recycled array or create new backing array.
            if ( _recycled != null && _recycled.Length >= Count )
            {
                Array.Copy( _items, 0, _recycled, 0, Count );
                _items    = _recycled;
                _recycled = null;
            }
            else
            {
                Resize( _items.length );
            }
        }


        public void Set( int index, T value )
        {
            Modified();
            this[ index ] = value;
        }

        public new void Insert( int index, T value )
        {
            Modified();
            base.Insert( index, value );
        }

        public new void InsertRange( int index, IEnumerable<T> collection )
        {
            Modified();
            base.InsertRange( index, collection );
        }

        public new bool Remove( T value )
        {
            Modified();

            return base.Remove( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new T RemoveAt( int index )
        {
            Modified();

            return base.RemoveAt( index );
        }

        /// <summary>
        /// Removes a range of elements from the array.
        /// </summary>
        /// <param name="start">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public new void RemoveRange( int start, int count )
        {
            Modified();
            base.RemoveRange( start, count );
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">
        /// The Predicate delegate that defines the conditions of the elements to remove.
        /// </param>
        /// <returns>The number of elements removed from the array</returns>
        public new int RemoveAll( Predicate< T > match )
        {
            Modified();

            return base.RemoveAll( match );
        }
    }
}
