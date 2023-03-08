namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// Abstract base class for asset loaders.
    /// </summary>
    /// <typeparam name="T">The class of the asset the loader supports.</typeparam>
    /// <typeparam name="TP">The class of the loading parameters the loader supports.</typeparam>
    public abstract class AssetLoader<T, TP> where TP : AssetLoaderParameters< T >
    {
        public IFileHandleResolver Resolver { get; set; }

        /// <summary>
        /// Constructor, sets the FileHandleResolver to use to resolve
        /// the file associated with the asset name.
        /// </summary>
        protected AssetLoader( IFileHandleResolver resolver )
        {
            this.Resolver = resolver;
        }

        public FileHandle? Resolve( string fileName )
        {
            return Resolver.Resolve( fileName );
        }

        /// <summary>
        /// Returns the assets this asset requires to be loaded first.
        /// This method may be called on a thread other than the GL thread.
        /// </summary>
        /// <param name="fileName">name of the asset to load</param>
        /// <param name="file">the resolved file to load</param>
        /// <param name="parameter">parameters for loading the asset</param>
        public abstract Array< AssetDescriptor< T > > GetDependencies( string fileName, FileHandle file, TP parameter );
    }
}
