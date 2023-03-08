namespace LibGDXSharp
{
    public class Version
    {
        // the current version of LibGDXSharp as a string in the major.minor.revision format
        public const string VersionString = "0.0.1";

        // the current major version of LibGDXSharp
        public static int Major { get; private set; }

        // the current minor version of LibGDXSharp
        public static int Minor { get; private set; }

        // the current revision version of LibGDXSharp
        public static int Revision { get; private set; }

        static Version()
        {
            try
            {
                var v = VersionString.Split( "\\." );
                
                Major    = v.Length < 1 ? 0 : int.Parse( v[ 0 ] );
                Minor    = v.Length < 2 ? 0 : int.Parse( v[ 1 ] );
                Revision = v.Length < 3 ? 0 : int.Parse( v[ 2 ] );
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Invalid version " + VersionString, e );
            }
        }

        /// <summary>
        /// Checks the provided version components against the current and reports
        /// TRUE if the CURRENT version is GREATER than the provided version.
        /// </summary>
        /// <param name="major">The Major version component.</param>
        /// <param name="minor">The Minor version component.</param>
        /// <param name="revision">The Revision version component.</param>
        public static bool IsHigher( int major, int minor, int revision )
        {
            return IsHigherEqual( major, minor, revision + 1 );
        }

        /// <summary>
        /// Checks the provided version components against the current
        /// and reports TRUE if the CURRENT version is GREATER than or
        /// EQUAL to the provided version.
        /// </summary>
        /// <param name="major">The Major version component.</param>
        /// <param name="minor">The Minor version component.</param>
        /// <param name="revision">The Revision version component.</param>
        public static bool IsHigherEqual( int major, int minor, int revision )
        {
            if ( Major != major )
            {
                return Major > major;
            }

            if ( Minor != minor )
            {
                return Minor > minor;
            }

            return Revision >= revision;
        }

        /// <summary>
        /// Checks the provided version components against the current and
        /// reports TRUE if the CURRENT version is LESS than the provided version.
        /// </summary>
        /// <param name="major">The Major version component.</param>
        /// <param name="minor">The Minor version component.</param>
        /// <param name="revision">The Revision version component.</param>
        public static bool IsLower( int major, int minor, int revision )
        {
            return IsLowerEqual( major, minor, revision - 1 );
        }

        /// <summary>
        /// Checks the provided version components against the current
        /// and reports TRUE if the CURRENT version is LESS than or
        /// EQUAL to the provided version.
        /// </summary>
        /// <param name="major">The Major version component.</param>
        /// <param name="minor">The Minor version component.</param>
        /// <param name="revision">The Revision version component.</param>
        public static bool IsLowerEqual( int major, int minor, int revision )
        {
            if ( Major != major )
            {
                return Major < major;
            }

            if ( Minor != minor )
            {
                return Minor < minor;
            }

            return Revision <= revision;
        }
    }
}