namespace LibGDXSharp.Utils
{
    public class IllegalStateException : Exception
    {
        public IllegalStateException( string invalidVersion )
            : base( invalidVersion )
        {
        }

        public IllegalStateException( string invalidVersion, Exception exception )
            : base( invalidVersion, exception )
        {
        }
    }
}
