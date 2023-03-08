using System.Collections;

namespace LibGDXSharp.Utils.Collections
{
    public class PredicateIEnumerator<T> : IEnumerator< T >
    {
        public IEnumerator< T? > Enumerator { get; set; }
        public Predicate< T >    Predicate  { get; set; }
        public bool              End        { get; set; } = false;
        public bool              Peeked     { get; set; } = false;
        public T?                Next       { get; set; } = default;

        private T _current;

        public PredicateIEnumerator( IEnumerable< T? > enumerable, Predicate< T > predicate )
            : this( enumerable.GetEnumerator(), predicate )
        {
        }

        public PredicateIEnumerator( IEnumerator< T? > enumerator, Predicate< T > predicate )
        {
            this.Enumerator = enumerator;
            this.Predicate  = predicate;
            End             = false;
            Peeked          = false;
            Next            = default;
        }

        public void Set( IEnumerable< T? > enumerable, Predicate< T > predicate )
        {
            Set( enumerable.GetEnumerator(), predicate );
        }

        public void Set( IEnumerator< T? > iterator, Predicate< T > predicate )
        {
            this.Enumerator = iterator;
            this.Predicate  = predicate;
            End             = false;
            Peeked          = false;
            Next            = default;
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        public T Current => _current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
