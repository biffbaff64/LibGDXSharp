namespace LibGDXSharp.Utils
{
    /// <summary>
    /// This class is for selecting a ranked element (kth ordered statistic) from an
    /// unordered list in faster time than sorting the whole array. Typical applications
    /// include finding the nearest enemy unit(s), and other operations which are likely
    /// to run as often as every x frames. Certain values of k will result in a partial
    /// sorting of the Array.
    /// The lowest ranking element starts at 1, not 0. 1 = first, 2 = second, 3 = third,
    /// etc. Calling with a value of zero will result in a GdxRuntimeException
    /// This class uses very minimal extra memory, as it makes no copies of the array.
    /// The underlying algorithms used are a naive single-pass for k = min and k =max, and
    /// Hoare's quickselect for values in between.
    /// </summary>
    public class Selector<T>
    {
        private readonly static Selector< T > instance = new Selector< T >();
        private                 QuickSelect   _quickSelect;

        public static Selector< T > Instance() => instance;

        public T Select( T[] items, IComparator< T > comp, int kthLowest, int size )
        {
        }
    }
}
