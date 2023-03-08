namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// Base class for asynchronous AssetLoader instances. Such loaders try to
    /// load parts of an OpenGL resource, like the Pixmap, on a separate thread
    /// to then load the actual resource on the thread the OpenGL context is
    /// active on.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TP"></typeparam>
    public abstract class AsynchronousAssetLoader<T, TP> : AssetLoader< T, TP > where TP : AssetLoaderParameters< T >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        protected AsynchronousAssetLoader( IFileHandleResolver resolver ) : base( resolver )
        {
        }

        /// <summary>
        /// Loads the non-OpenGL part of the asset and injects any dependencies of
        /// the asset into the AssetManager.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        public abstract void LoadAsync( AssetManager manager, string fileName, FileHandle file, TP parameter );

        /// <summary>
        /// Called if this task is unloaded before loadSync is called. This method may
        /// be invoked on any thread, but will not be invoked during or after loadSync.
        /// This method is not invoked when a task is cancelled because it threw an
        /// exception, only when the asset is unloaded before loading is complete.
        /// The default implementation does nothing. Subclasses should release any
        /// resources acquired in loadAsync, which may or may not have been called before
        /// this method, but never during or after this method. Note that loadAsync may
        /// still be executing when this method is called and must release any resources
        /// it allocated.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        public void UnloadAsync( AssetManager manager, string fileName, FileHandle file, TP parameter )
        {
        }

        /// <summary>
        /// Loads the OpenGL part of the asset.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract T LoadSync( AssetManager manager, string fileName, FileHandle file, TP parameter );
    }
}
