using System.Collections;

namespace LibGDXSharp.Utils.Collections
{
    public class PredicateIterable<T> : IEnumerable< T >
    {
        public IEnumerable< T >       Enumerable { get; set; }
        public IPredicate< T >        Predicate  { get; set; }
        public PredicateIterator< T > Enumerator { get; set; } = null;

        public PredicateIterable( IEnumerable< T > enumerable, IPredicate< T > predicate )
        {
            this.Enumerable = enumerable;
            this.Predicate  = predicate;
        }

        public void Set( IEnumerable< T > enumerable, IPredicate< T > predicate )
        {
            this.Enumerable = enumerable;
            this.Predicate  = predicate;
        }

        public IEnumerator< T > GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
