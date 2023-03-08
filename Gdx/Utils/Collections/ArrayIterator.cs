using System.Collections;

namespace LibGDXSharp.Utils.Collections
{
    public class ArrayIterator<T> : IEnumerator< T >
    {
        private readonly Array< T > _array;
        private readonly bool       _allowRemove;
        public           int        index;
        public           bool       valid = true;

        public ArrayIterator( Array< T > array, bool allowRemove = true )
        {
            this._array       = array;
            this._allowRemove = allowRemove;
        }

        public bool HasNext()
        {
            if ( !valid )
            {
                throw new GdxRuntimeException( "#iterator() cannot be used nested." );
            }

            return index < _array.Size;
        }

        public T Next()
        {
            if ( index >= _array.Size )
            {
                throw new ArgumentOutOfRangeException( index.ToString() );
            }

            if ( !valid )
            {
                throw new GdxRuntimeException( "#iterator() cannot be used nested." );
            }

            return _array.Items[ index++ ];
        }

        public void Remove()
        {
            if ( !_allowRemove )
            {
                throw new GdxRuntimeException( "Remove not allowed." );
            }

            index--;
            _array.RemoveIndex( index );
        }

        public void Reset()
        {
            index = 0;
        }

        public IEnumerator< T > Iterator()
        {
            return this;
        }

        public void Dispose()
        {
            Remove();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public T Current { get; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}

