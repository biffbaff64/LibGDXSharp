using System.Collections;

namespace LibGDXSharp.Utils.Collections
{
    public class ArrayIterable<T> : IEnumerable< T >
    {
        private readonly Array< T >         _array;
        private readonly bool               _allowRemove;
        private          ArrayIterator< T > _iterator1, _iterator2;

        public ArrayIterable( Array< T > array, bool allowRemove = true )
        {
            this._array       = array;
            this._allowRemove = allowRemove;
        }

        public IEnumerator< T > Iterator()
        {
            // lastAcquire.getBuffer().setLength(0);
            // new Throwable().printStackTrace(new java.io.PrintWriter(lastAcquire));
            if ( _iterator1 == null )
            {
                _iterator1 = new ArrayIterator< T >( _array, _allowRemove );
                _iterator2 = new ArrayIterator< T >( _array, _allowRemove );
                // iterator1.iterable = this;
                // iterator2.iterable = this;
            }

            if ( !_iterator1.valid )
            {
                _iterator1.index = 0;
                _iterator1.valid = true;
                _iterator2.valid = false;

                return _iterator1;
            }

            _iterator2.index = 0;
            _iterator2.valid = true;
            _iterator1.valid = false;

            return _iterator2;
        }

        public IEnumerator< T > GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

