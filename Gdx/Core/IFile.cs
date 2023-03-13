namespace LibGDXSharp.Core
{
    public interface IFile
    {
        public enum FileType
        {
            /// <summary>
            /// Path relative to the root of the classpath. Classpath files are always readonly.
            /// Note that classpath files are not compatible with some functionality on Android,
            /// such as Audio#newSound(FileHandle) and Audio#newMusic(FileHandle).
            /// </summary>
            Classpath,

            /// <summary>
            /// Path relative to the asset directory on Android and to the application's root
            /// directory on the desktop. On the desktop, if the file is not found, then the
            /// classpath is checked.
            /// Internal files are always readonly.
            /// </summary>
            Internal,

            /// <summary>
            /// Path relative to the root of the app external storage on Android and
            /// to the home directory of the current user on the desktop.
            /// </summary>
            External,

            /// <summary>
            /// Path that is a fully qualified, absolute filesystem path. To ensure
            /// portability across platforms use absolute files only when absolutely
            /// necessary.
            /// </summary>
            Absolute,

            /// <summary>
            /// Path relative to the private files directory on Android and to the
            /// application's root directory on the desktop.
            /// </summary>
            Local
        }

        /// <summary>
        /// Returns a handle representing a file or directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type">Determines how the path is resolved.</param>
        /// <exception cref="GdxRuntimeException">
        /// if the type is classpath or internal and the file does not exist.
        /// </exception>
        /// <seealso cref="FileType"/>
        /// 
        FileHandle GetFileHandle( string path, FileType type );

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Classpath"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileHandle Classpath( string path );

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Internal"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileHandle Internal( string path );

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.External"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileHandle External( string path );

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Absolute"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileHandle? Absolute( string path );

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Local"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileHandle Local( string path );

        /// <summary>
        /// Returns the external storage path directory. This is the app
        /// external storage on Android and the home directory of the
        /// current user on the desktop.
        /// </summary>
        string GetExternalStoragePath();

        /// <summary>
        /// Returns true if the external storage is ready for file IO.
        /// </summary>
        bool IsExternalStorageAvailable();

        /// <summary>
        /// Returns the local storage path directory. This is the private
        /// files directory on Android and the directory of the jar on
        /// the desktop.
        /// </summary>
        string GetLocalStoragePath();

        /// <summary>
        /// Returns true if the local storage is ready for file IO.
        /// </summary>
        bool IsLocalStorageAvailable();
    }
}
