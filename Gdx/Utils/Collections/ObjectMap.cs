namespace LibGDXSharp.Utils.Collections
{
    public class ObjectMap<TK, TV>
    {
        public class Entry<TKe, TVe>
        {
            public TKe  key;
            public TVe? value;

            public override string ToString()
            {
                return key + " = " + value;
            }
        }
    }
}
