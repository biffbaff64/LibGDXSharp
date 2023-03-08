namespace LibGDXSharp.Utils.Collections
{
    public interface IPredicate<in T>
    {
        /// <summary>
        /// Return true if the item matches the criteria and should be included in the iterator's items.
        /// </summary>
        bool Evaluate( T arg0 );
    }
}
