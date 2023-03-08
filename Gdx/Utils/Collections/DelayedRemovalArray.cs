using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils
{
    public class DelayedRemovalArray<T> : Array< T >
    {
        private          int      _iterating = 0;
        private          int      _clear     = 0;
        private readonly IntArray _remove    = new IntArray();

        public DelayedRemovalArray( Array< T > array ) : base( array )
        {
            Reset();
        }

        public DelayedRemovalArray( T[] array )
        {
            Reset();
        }

        public DelayedRemovalArray( T[] array, int startIndex, int count ) : base( array, startIndex, count )
        {
            Reset();
        }

        public DelayedRemovalArray( int initialCapacity = 16 ) : base( initialCapacity )
        {
            Reset();
        }

        public void Begin()
        {
            _iterating++;
        }

        public void End()
        {
            if ( _iterating == 0 ) throw new GdxRuntimeException( "Begin() must be called before End()!" );

            _iterating--;

            if ( _iterating == 0 )
            {
                if ( _clear > 0 && _clear == Count )
                {
                    _remove.Clear();
                    Clear();
                }
                else
                {
                    for ( int i = 0, n = _remove.Count; i < n; i++ )
                    {
                        int index = _remove.Pop();

                        if ( index >= _clear )
                        {
                            RemoveAt( index );
                        }
                    }

                    for ( int i = _clear - 1; i >= 0; i-- )
                    {
                        RemoveAt( i );
                    }
                }

                _clear = 0;
            }
        }
        
        private void Remove( int index )
        {
            if ( index < _clear ) return;

            for ( int i = 0, n = _remove.Count; i < n; i++ )
            {
                var removeIndex = _remove[ i ];

                if ( index == removeIndex ) return;

                if ( index < removeIndex )
                {
                    _remove.Insert( i, index );

                    return;
                }
            }

            _remove.Add( index );
        }

        public bool RemoveValue( T value, bool identity )
        {
            if ( _iterating > 0 )
            {
                int index = IndexOf( value );

                if ( index == -1 ) return false;
                
                Remove( index );

                return true;
            }

            return base.Remove( value );
        }

        public T RemoveIndex( int index )
        {
            if ( _iterating > 0 )
            {
                Remove( index );

                return this[ index ];
            }

            return base.RemoveAt( index );
        }

        public new void RemoveRange( int start, int end )
        {
            if ( _iterating > 0 )
            {
                for ( int i = end; i >= start; i-- )
                {
                    Remove( i );
                }
            }
            else
            {
                base.RemoveRange( start, end );
            }
        }

        public new void Clear()
        {
            if ( _iterating > 0 )
            {
                _clear = Count;

                return;
            }

            base.Clear();
        }

        public void Set( int index, T value )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );

            this[ index ] = value;
        }

        public new void Insert( int index, T value )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Insert( index, value );
        }

        public void InsertRange( int index, int count )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.InsertRange( index, count );
        }

        public void Swap( int first, int second )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Swap( first, second );
        }

        public new T Pop()
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );

            return base.Pop();
        }

        public new void Sort()
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Sort();
        }

        public void Sort( Comparator<? super T> comparator)
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Sort( comparator );
        }

        public new void Reverse()
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Reverse();
        }

        public void Shuffle()
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Shuffle();
        }

        public new void Truncate( int newSize )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );
            
            base.Truncate( newSize );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSize"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public T[] SetSize( int newSize )
        {
            if ( _iterating > 0 ) throw new GdxRuntimeException( "Invalid between begin/end." );

            return base.SetSize( newSize );
        }

        private void Reset()
        {
            _iterating = 0;
            _clear     = 0;
        }
    }
}
