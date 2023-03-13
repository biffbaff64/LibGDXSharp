namespace LibGDXSharp.Utils.Collections
{
    public class Collections
    {
        /** When true, {@link Iterable#iterator()} for {@link Array}, {@link ObjectMap}, and other collections will allocate a new
	 * iterator for each invocation. When false, the iterator is reused and nested use will throw an exception. Default is
	 * false. */
        public static bool AllocateIterators { get; set; }
    }
}
