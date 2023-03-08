namespace LibGDXSharp.Utils
{
    public class GdxRuntimeException : Exception
    {
        public GdxRuntimeException( string invalidVersion )
            : base( invalidVersion )
        {
        }

        public GdxRuntimeException( string invalidVersion, Exception exception )
            : base( invalidVersion, exception )
        {
        }
    }
}
