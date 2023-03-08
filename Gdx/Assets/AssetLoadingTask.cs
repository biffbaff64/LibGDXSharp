using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Assets
{
    public class AssetLoadingTask<T> : IAsyncTask< bool > where T : AssetLoaderParameters< T >
    {
        private AssetManager         _manager;
        private AssetDescriptor< T > _assetDesc;
        private AssetLoader< T, T >  _loader;
        private AsyncExecutor        _executor;
        private long                 _startTime;

        private volatile bool                          _asyncDone;
        private volatile bool                          _dependenciesLoaded;
        private volatile Array< AssetDescriptor< T > > _dependencies;
        private volatile AsyncResult                   _depsFuture;
        private volatile AsyncResult                   _loadFuture;
        private volatile object                        _asset;
        private volatile bool                          _cancel;

        public AssetLoadingTask( AssetManager manager, AssetDescriptor< T > assetDesc, AssetLoader< T, T > loader, AsyncExecutor threadPool )
        {
            this._manager   = manager;
            this._assetDesc = assetDesc;
            this._loader    = loader;
            this._executor  = threadPool;
            this._startTime = manager.Log.GetLevel() == Logger.DEBUG ? TimeUtils.NanoTime() : 0;
        }

        public bool Call()
        {
            return false;
        }

        public bool Update()
        {
            return false;
        }

        public void Unload()
        {
        }

        private FileHandle Resolve( AssetLoader< T, T > loader, AssetDescriptor< T > assetDesc )
        {
            return new FileHandle( string.Empty );
        }

        private void HandleSyncLoader()
        {
        }

        private void HandleAsyncLoader()
        {
        }

        private void RemoveDuplicates( Array< AssetDescriptor< T > > array )
        {
        }
    }
}
