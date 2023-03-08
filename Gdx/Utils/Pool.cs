namespace LibGDXSharp.Utils
{
    public abstract class Pool<T>
    {
        public int Max  { get; set; } // The maximum number of objects that will be pooled.
        public int Peak { get; set; } // The highest number of free objects. Can be reset any time.

        private readonly Array< T > _freeObjects;

        /// <summary>
        /// Creates a pool with an initial capacity of 16 and no maximum.
        /// </summary>
        protected Pool() : this( 16, int.MaxValue )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial size of the array supporting the pool. No objects are created/pre-allocated.
        /// </param>
        /// <param name="max">The maximum number of free objects to store in this pool.</param>
        protected Pool( int initialCapacity, int max = int.MaxValue )
        {
            _freeObjects = new Array< T >( initialCapacity );
            this.Max     = max;
        }

        protected abstract T NewObject();

        /// <summary>
        /// Returns an object from this pool.
        /// The object may be new (from <see cref="NewObject"/>) or reused (previously <see cref="Free(T)"/> freed).
        /// </summary>
        public T Obtain()
        {
            if ( _freeObjects.Count == 0 )
            {
                return NewObject();
            }
            else
            {
                var item = _freeObjects[ ^1 ];
                _freeObjects.RemoveAt( _freeObjects.Count - 1 );

                return item;
            }
        }

        /// <summary>
        /// Puts the specified object in the pool, making it eligible to be returned by
        /// <see cref="Obtain"/>. If the pool already contains <see cref="Max"/> free objects,
        /// the specified object is discarded using <see cref="Discard"/> and not added to the pool.
        /// The pool does not check if an object is already freed, so the same object must not be
        /// freed multiple times.
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Free( T obj )
        {
            if ( obj == null )
            {
                throw new ArgumentException( "object cannot be null." );
            }

            if ( _freeObjects.Count < Max )
            {
                _freeObjects.Add( obj );
                Peak = System.Math.Max( Peak, _freeObjects.Count );
                
                Reset( obj );
            }
            else
            {
                Discard( obj );
            }
        }

        /// <summary>
        /// Adds the specified number of new free objects to the pool.
        /// Usually called early on as a pre-allocation mechanism but
        /// can be used at any time.
        /// </summary>
        /// <param name="size">The number of objects to be added.</param>
        public void Fill( int size )
        {
            for ( var i = 0; i < size; i++ )
            {
                if ( _freeObjects.Count < Max )
                {
                    _freeObjects.Add( NewObject() );
                }
            }

            Peak = System.Math.Max( Peak, _freeObjects.Count );
        }

        /// <summary>
        /// Called when an object is freed to clear the state of the object for possible
        /// later reuse. The default implementation calls IPoolable.Reset if the object is Poolable.
        /// </summary>
        protected void Reset( T obj )
        {
            if ( obj is IPoolable poolable )
            {
                poolable.Reset();
            }
        }

        /// <summary>
        /// Called when an object is discarded. This is the case when an object is
        /// freed, but the maximum capacity of the pool is reached, and when the
        /// pool is cleared.
        /// </summary>
        protected void Discard( T obj )
        {
        }

        /// <summary>
        /// Puts the specified objects in the pool. Null objects within the array
        /// are silently ignored. The pool does not check if an object is already
        /// freed, so the same object must not be freed multiple times.
        /// </summary>
        public void FreeAll( Array< T > objects )
        {
            if ( objects == null ) throw new ArgumentException( "objects cannot be null." );

            var freeObjects = this._freeObjects;
            var max         = this.Max;

            for ( int i = 0, n = objects.Count; i < n; i++ )
            {
                var obj = objects[ i ];

                if ( obj == null ) continue;

                if ( freeObjects.Count < max )
                {
                    freeObjects.Add( obj );
                    Reset( obj );
                }
                else
                {
                    Discard( obj );
                }
            }

            Peak = System.Math.Max( Peak, freeObjects.Count );
        }

        /// <summary>
        /// Removes and discards all free objects from this pool.
        /// </summary>
        public void Clear()
        {
            for ( var i = 0; i < _freeObjects.Count; i++ )
            {
                var obj = _freeObjects.Pop();
                Discard( obj );
            }
        }

        /// <summary>
        /// The number of objects available to be obtained.
        /// </summary>
        public int Free()
        {
            return _freeObjects.Count;
        }
    }
}
