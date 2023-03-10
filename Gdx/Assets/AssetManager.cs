using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Utils.Async;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Json;
using LibGDXSharp.Utils.Reflect;

namespace LibGDXSharp.Assets
{
    public class AssetManager
    {
        private ObjectMap< Type, ObjectMap< string, RefCountedContainer > > _assets;
        private ObjectMap< string, Type >                                   _assetTypes;
        private ObjectMap< string, Array< string > >                        _assetDependencies;
        private ObjectSet< string >                                         _injected;

        private ObjectMap< Type, ObjectMap< string, AssetLoader > > _loaders;
        private Array< AssetDescriptor >                            _loadQueue;
        private AsyncExecutor                                       _executor;

        private Stack< AssetLoadingTask > _tasks = new Stack< AssetLoadingTask >();
        private IAssetErrorListener       _listener;
        private int                       _loaded;
        private int                       _toLoad;
        private int                       _peakTasks;

        private IFileHandleResolver _resolver;

        private Logger _log = new Logger( "AssetManager", Application.LogNone );

        /// <summary>
        /// Creates a new AssetManager with all default loaders.
        ///</summary>
        public AssetManager() : this( new InternalFileHandleResolver() )
        {
        }

        /// <summary>
        /// Creates a new AssetManager with optionally all default loaders.
        /// If you don't add the default loaders then you do have to manually add
        /// the loaders you need, including any loaders they might depend on.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="defaultLoaders">Whether to add the default loaders (default is true).</param>
        public AssetManager( IFileHandleResolver resolver, bool defaultLoaders = true )
        {
            this._resolver = resolver;

            if ( defaultLoaders )
            {
                SetLoader( BitmapFont.class, new BitmapFontLoader( resolver ));
                SetLoader( Music.class, new MusicLoader( resolver ));
                SetLoader( Pixmap.class, new PixmapLoader( resolver ));
                SetLoader( Sound.class, new SoundLoader( resolver ));
                SetLoader( TextureAtlas.class, new TextureAtlasLoader( resolver ));
                SetLoader( Texture.class, new TextureLoader( resolver ));
                SetLoader( Skin.class, new SkinLoader( resolver ));
                SetLoader( ParticleEffect.class, new ParticleEffectLoader( resolver ));
                SetLoader( PolygonRegion.class, new PolygonRegionLoader( resolver ));
                SetLoader( I18NBundle.class, new I18NBundleLoader( resolver ));
                SetLoader( Model.class, ".g3dj", new G3dModelLoader( new JsonReader(), resolver ));
                SetLoader( Model.class, ".g3db", new G3dModelLoader( new UBJsonReader(), resolver ));
                SetLoader( Model.class, ".obj", new ObjLoader( resolver ));
                SetLoader( ShaderProgram.class, new ShaderProgramLoader( resolver ));
                SetLoader( Cubemap.class, new CubemapLoader( resolver ));
            }

            _executor = new AsyncExecutor( 1, "AssetManager" );
        }

        /// <summary>
        /// Returns the <see cref="IFileHandleResolver"/> for which this AssetManager was loaded with.
        /// </summary>
        /// <returns>the file handle resolver which this AssetManager uses.</returns>
        public IFileHandleResolver GetFileHandleResolver()
        {
            return _resolver;
        }

        /// <summary>
        ///
        /// </summary>
        public T Get<T>( string name )
        {
            T asset;
            
            lock ( this )
            {
                var type = _assetTypes.Get( name );

                if ( type == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                var assetsByType = _assets.Get( type );

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                var assetContainer = assetsByType.Get( name );

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                asset = assetContainer.GetObject( type );

                if ( asset == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
            }

            return asset;
        }

        public T Get<T>( string name, Type type )
        {
            T asset;
            
            lock( this )
            {
                ObjectMap< string, RefCountedContainer > assetsByType = _assets.Get( type );

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
                
                RefCountedContainer assetContainer = assetsByType.Get( name );

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
                
                asset = assetContainer.GetObject( type );

                if ( asset == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
            }
            
            return asset;
        }

        public Array GetAll<T>( Type type, Array< T > outArray )
        {
            lock ( this )
            {
                ObjectMap< string, RefCountedContainer > assetsByType = _assets.Get( type );

                if ( assetsByType != null )
                {
                    foreach ( ObjectMap.Entry< string, RefCountedContainer > asset in assetsByType.Entries() )
                    {
                        outArray.Add( asset.value.getObject( type ) );
                    }
                }
            }

            return out;
        }

        public synchronized< T > T get( AssetDescriptor< T > assetDescriptor )
        {
            return Get( assetDescriptor.fileName, assetDescriptor.type );
        }

        public synchronized bool Contains( string fileName )
        {
            if ( _tasks.size() > 0 && _tasks.firstElement().assetDesc.fileName.equals( fileName ) ) return true;

            for ( int i = 0; i < _loadQueue.size; i++ )
                if ( _loadQueue.get( i ).fileName.equals( fileName ) )
                    return true;

            return IsLoaded( fileName );
        }

        public synchronized bool Contains( string fileName, Class type )
        {
            if ( _tasks.size() > 0 )
            {
                AssetDescriptor assetDesc = _tasks.firstElement().assetDesc;

                if ( assetDesc.type == type && assetDesc.fileName.equals( fileName ) ) return true;
            }

            for ( int i = 0; i < _loadQueue.size; i++ )
            {
                AssetDescriptor assetDesc = _loadQueue.get( i );

                if ( assetDesc.type == type && assetDesc.fileName.equals( fileName ) ) return true;
            }

            return IsLoaded( fileName, type );
        }

        /** Removes the asset and all its dependencies, if they are not used by other assets.
	 * @param fileName the file name */
        public synchronized void Unload( string fileName )
        {
            // convert all windows path separators to unix style
            fileName = fileName.replace( '\\', '/' );

            // check if it's currently processed (and the first element in the stack, thus not a dependency) and cancel if necessary
            if ( _tasks.size() > 0 )
            {
                AssetLoadingTask currentTask = _tasks.firstElement();

                if ( currentTask.assetDesc.fileName.equals( fileName ) )
                {
                    _log.info( "Unload (from tasks): " + fileName );
                    currentTask.cancel = true;
                    currentTask.unload();

                    return;
                }
            }

            Class type = _assetTypes.get( fileName );

            // check if it's in the queue
            int foundIndex = -1;

            for ( int i = 0; i < _loadQueue.size; i++ )
            {
                if ( _loadQueue.get( i ).fileName.equals( fileName ) )
                {
                    foundIndex = i;

                    break;
                }
            }

            if ( foundIndex != -1 )
            {
                _toLoad--;
                AssetDescriptor desc = _loadQueue.removeIndex( foundIndex );
                _log.info( "Unload (from queue): " + fileName );

                // if the queued asset was already loaded, let the callback know it is available.
                if ( type != null && desc.params != null && desc.params.loadedCallback != null)
                desc.params.loadedCallback.finishedLoading( this, desc.fileName, desc.type );

                return;
            }

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            RefCountedContainer assetRef = _assets.get( type ).get( fileName );

            // if it is reference counted, decrement ref count and check if we can really get rid of it.
            assetRef.DecRefCount();

            if ( assetRef.GetRefCount() <= 0 )
            {
                _log.info( "Unload (dispose): " + fileName );

                // if it is disposable dispose it
                if ( assetRef.GetObject
                        ( Object.class) instanceof disposable) ( ( Disposable )assetRef.GetObject( Object.class)).Dispose();

                // remove the asset from the manager.
                _assetTypes.remove( fileName );
                _assets.get( type ).remove( fileName );
            }
            else
                _log.info( "Unload (decrement): " + fileName );

            // remove any dependencies (or just decrement their ref count).
            Array< string > dependencies = _assetDependencies.get( fileName );

            if ( dependencies != null )
            {
                for ( string dependency :
                dependencies)
                if ( isLoaded( dependency ) ) Unload( dependency );
            }

            // remove dependencies if ref count < 0
            if ( assetRef.GetRefCount() <= 0 ) _assetDependencies.remove( fileName );
        }

        /** @param asset the asset
	 * @return whether the asset is contained in this manager */
        public synchronized< T > bool ContainsAsset( T asset )
        {
            ObjectMap< string, RefCountedContainer > assetsByType = _assets.get( asset.getClass() );

            if ( assetsByType == null ) return false;
            for ( string fileName :
            assetsByType.keys()) {
                T otherAsset = ( T )assetsByType.get( FileName ).getObject( Object.class);

                if ( otherAsset == asset || asset.equals( otherAsset ) ) return true;
            }

            return false;
        }

        /** @param asset the asset
	 * @return the filename of the asset or null */
        public synchronized< T > string GetAssetFileName( T asset )
        {
            for ( _class assetType :
            _assets.keys()) {
                ObjectMap< string, RefCountedContainer > assetsByType = _assets.get( assetType );
                for ( string fileName :
                assetsByType.keys()) {
                    T otherAsset = ( T )assetsByType.get( FileName ).getObject( Object.class);

                    if ( otherAsset == asset || asset.equals( otherAsset ) ) return FileName;
                }
            }

            return null;
        }

        /** @param assetDesc the AssetDescriptor of the asset
	 * @return whether the asset is loaded */
        public synchronized bool IsLoaded( AssetDescriptor assetDesc )
        {
            return isLoaded( assetDesc.fileName );
        }

        /** @param fileName the file name of the asset
	 * @return whether the asset is loaded */
        public synchronized bool IsLoaded( string fileName )
        {
            if ( fileName == null ) return false;

            return _assetTypes.containsKey( fileName );
        }

        /** @param fileName the file name of the asset
	 * @return whether the asset is loaded */
        public synchronized bool IsLoaded( string fileName, Class type )
        {
            ObjectMap< string, RefCountedContainer > assetsByType = _assets.get( type );

            if ( assetsByType == null ) return false;
            RefCountedContainer assetContainer = assetsByType.get( fileName );

            if ( assetContainer == null ) return false;

            return assetContainer.GetObject( type ) != null;
        }

        /** Returns the default loader for the given type.
	 * @param type The type of the loader to get
	 * @return The loader capable of loading the type, or null if none exists */
        public <T> AssetLoader GetLoader( final @class<T> type) {
            return getLoader( type, null );
        }

        /** Returns the loader for the given type and the specified filename. If no loader exists for the specific filename, the
	 * default loader for that type is returned.
	 * @param type The type of the loader to get
	 * @param fileName The filename of the asset to get a loader for, or null to get the default loader
	 * @return The loader capable of loading the type and filename, or null if none exists */
        public <T> AssetLoader GetLoader( final @class<T> type, final string FileName) {
            final ObjectMap< string, AssetLoader >
                loaders = this._loaders.get( type );

            if ( loaders == null || loaders.size < 1 ) return null;
            if ( fileName == null ) return loaders.get( "" );
            AssetLoader result = null;
            int         l      = -1;
            for ( ObjectMap.Entry< string, AssetLoader > entry :
            loaders.entries())

            {
                if ( entry.key.length() > l && FileName.endsWith( entry.key ) )
                {
                    result = entry.value;
                    l      = entry.key.length();
                }
            }

            return result;
        }

        /** Adds the given asset to the loading queue of the AssetManager.
	 * @param fileName the file name (interpretation depends on {@link AssetLoader})
	 * @param type the type of the asset. */
        public synchronized< T > void Load( string fileName, Class< T > type )
        {
            Load( fileName, type, null );
        }

        /** Adds the given asset to the loading queue of the AssetManager.
	 * @param fileName the file name (interpretation depends on {@link AssetLoader})
	 * @param type the type of the asset.
	 * @param parameter parameters for the AssetLoader. */
        public synchronized< T > void Load( string fileName, Class< T > type, AssetLoaderParameters< T > parameter )
        {
            AssetLoader loader = getLoader( type, fileName );

            if ( loader == null )
                throw new GdxRuntimeException( "No loader for type: " + ClassReflection.getSimpleName( type ) );

            // reset stats
            if ( _loadQueue.size == 0 )
            {
                _loaded    = 0;
                _toLoad    = 0;
                _peakTasks = 0;
            }

            // check if an asset with the same name but a different type has already been added.

            // check preload queue
            for ( int i = 0; i < _loadQueue.size; i++ )
            {
                AssetDescriptor desc = _loadQueue.get( i );

                if ( desc.fileName.equals( fileName ) && !desc.type.equals( type ) )
                    throw new GdxRuntimeException
                        (
                         "Asset with name '"
                         + fileName
                         + "' already in preload queue, but has different type (expected: "
                         + ClassReflection.getSimpleName( type )
                         + ", found: "
                         + ClassReflection.getSimpleName( desc.type )
                         + ")"
                        );
            }

            // check task list
            for ( int i = 0; i < _tasks.size(); i++ )
            {
                AssetDescriptor desc = _tasks.get( i ).assetDesc;

                if ( desc.fileName.equals( fileName ) && !desc.type.equals( type ) )
                    throw new GdxRuntimeException
                        (
                         "Asset with name '"
                         + fileName
                         + "' already in task list, but has different type (expected: "
                         + ClassReflection.getSimpleName( type )
                         + ", found: "
                         + ClassReflection.getSimpleName( desc.type )
                         + ")"
                        );
            }

            // check loaded assets
            Class otherType = _assetTypes.get( fileName );

            if ( otherType != null && !otherType.equals( type ) )
                throw new GdxRuntimeException
                    (
                     "Asset with name '"
                     + fileName
                     + "' already loaded, but has different type (expected: "
                     + ClassReflection.getSimpleName( type )
                     + ", found: "
                     + ClassReflection.getSimpleName( otherType )
                     + ")"
                    );

            _toLoad++;
            AssetDescriptor assetDesc = new AssetDescriptor( fileName, type, parameter );
            _loadQueue.add( assetDesc );
            _log.debug( "Queued: " + assetDesc );
        }

        /** Adds the given asset to the loading queue of the AssetManager.
	 * @param desc the {@link AssetDescriptor} */
        public synchronized void Load( AssetDescriptor desc )
        {
            Load( desc.fileName, desc.type, desc.params );
        }

        /** Updates the AssetManager for a single task. Returns if the current task is still being processed or there are no tasks,
	 * otherwise it finishes the current task and starts the next task.
	 * @return true if all loading is finished. */
        public synchronized bool Update()
        {
            try
            {
                if ( _tasks.size() == 0 )
                {
                    // loop until we have a new task ready to be processed
                    while ( _loadQueue.size != 0 && _tasks.size() == 0 )
                        NextTask();

                    // have we not found a task? We are done!
                    if ( _tasks.size() == 0 ) return true;
                }

                return UpdateTask() && _loadQueue.size == 0 && _tasks.size() == 0;
            }
            catch ( Throwable t )
            {
                HandleTaskError( t );

                return _loadQueue.size == 0;
            }
        }

        /** Updates the AssetManager continuously for the specified number of milliseconds, yielding the CPU to the loading thread
	 * between updates. This may block for less time if all loading tasks are complete. This may block for more time if the portion
	 * of a single task that happens in the GL thread takes a long time.
	 * @return true if all loading is finished. */
        public bool Update( int millis )
        {
            long endTime = TimeUtils.millis() + millis;

            while ( true )
            {
                bool done = Update();

                if ( done || TimeUtils.millis() > endTime ) return done;
                ThreadUtils.yield();
            }
        }

        /** Returns true when all assets are loaded. Can be called from any thread but note {@link #update()} or related methods must
	 * be called to process tasks. */
        public synchronized bool IsFinished()
        {
            return _loadQueue.size == 0 && _tasks.size() == 0;
        }

        /** Blocks until all assets are loaded. */
        public void FinishLoading()
        {
            _log.debug( "Waiting for loading to complete..." );

            while ( !Update() )
                ThreadUtils.yield();

            _log.debug( "Loading complete." );
        }

        /** Blocks until the specified asset is loaded.
	 * @param assetDesc the AssetDescriptor of the asset */
        public <T> T FinishLoadingAsset( AssetDescriptor assetDesc )
        {
            return finishLoadingAsset( assetDesc.fileName );
        }

        /** Blocks until the specified asset is loaded.
	 * @param fileName the file name (interpretation depends on {@link AssetLoader}) */
        public <T> T FinishLoadingAsset( string fileName )
        {
            _log.debug( "Waiting for asset to be loaded: " + fileName );

            while ( true )
            {
                synchronized( this ) {
                    Class< T > type = _assetTypes.get( fileName );

                    if ( type != null )
                    {
                        ObjectMap< string, RefCountedContainer > assetsByType = _assets.get( type );

                        if ( assetsByType != null )
                        {
                            RefCountedContainer assetContainer = assetsByType.get( fileName );

                            if ( assetContainer != null )
                            {
                                T asset = assetContainer.GetObject( type );

                                if ( asset != null )
                                {
                                    _log.debug( "Asset loaded: " + fileName );

                                    return asset;
                                }
                            }
                        }
                    }

                    Update();
                }

                ThreadUtils.yield();
            }
        }

        synchronized void InjectDependencies( string parentAssetFilename, Array< AssetDescriptor > dependendAssetDescs )
        {
            ObjectSet< string > injected = this._injected;
            for ( AssetDescriptor desc :
            dependendAssetDescs) {
                if ( injected.contains( desc.fileName ) ) continue; // Ignore subsequent dependencies if there are duplicates.
                injected.add( desc.fileName );
                InjectDependency( parentAssetFilename, desc );
            }

            injected.clear( 32 );
        }

        private synchronized void InjectDependency( string parentAssetFilename, AssetDescriptor dependendAssetDesc )
        {
            // add the asset as a dependency of the parent asset
            Array< string > dependencies = _assetDependencies.get( parentAssetFilename );

            if ( dependencies == null )
            {
                dependencies = new Array();
                _assetDependencies.put( parentAssetFilename, dependencies );
            }

            dependencies.add( dependendAssetDesc.fileName );

            // if the asset is already loaded, increase its reference count.
            if ( isLoaded( dependendAssetDesc.fileName ) )
            {
                _log.debug( "Dependency already loaded: " + dependendAssetDesc );
                Class               type     = _assetTypes.get( dependendAssetDesc.fileName );
                RefCountedContainer assetRef = _assets.get( type ).get( dependendAssetDesc.fileName );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( dependendAssetDesc.fileName );
            }
            else
            {
                // else add a new task for the asset.
                _log.info( "Loading dependency: " + dependendAssetDesc );
                AddTask( dependendAssetDesc );
            }
        }

        /** Removes a task from the loadQueue and adds it to the task stack. If the asset is already loaded (which can happen if it was
	 * a dependency of a previously loaded asset) its reference count will be increased. */
        private void NextTask()
        {
            AssetDescriptor assetDesc = _loadQueue.removeIndex( 0 );

            // if the asset not meant to be reloaded and is already loaded, increase its reference count
            if ( isLoaded( assetDesc.fileName ) )
            {
                _log.debug( "Already loaded: " + assetDesc );
                Class               type     = _assetTypes.get( assetDesc.fileName );
                RefCountedContainer assetRef = _assets.get( type ).get( assetDesc.fileName );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( assetDesc.fileName );
                if ( assetDesc.params != null && assetDesc.params.loadedCallback != null)
                assetDesc.params.loadedCallback.finishedLoading( this, assetDesc.fileName, assetDesc.type );
                _loaded++;
            }
            else
            {
                // else add a new task for the asset.
                _log.info( "Loading: " + assetDesc );
                AddTask( assetDesc );
            }
        }

        /** Adds a {@link AssetLoadingTask} to the task stack for the given asset. */
        private void AddTask( AssetDescriptor assetDesc )
        {
            AssetLoader loader = getLoader( assetDesc.type, assetDesc.fileName );

            if ( loader == null )
                throw new GdxRuntimeException( "No loader for type: " + ClassReflection.getSimpleName( assetDesc.type ) );

            _tasks.push( new AssetLoadingTask( this, assetDesc, loader, _executor ) );
            _peakTasks++;
        }

        /** Adds an asset to this AssetManager */
        protected <T> void AddAsset( final string _fileName, _class< T > type, T Asset) {
            // add the asset to the filename lookup
            _assetTypes.put( fileName, type );

            // add the asset to the type lookup
            ObjectMap< string, RefCountedContainer > typeToAssets = _assets.get( type );

            if ( typeToAssets == null )
            {
                typeToAssets = new ObjectMap< string, RefCountedContainer >();
                _assets.put( type, typeToAssets );
            }

            typeToAssets.put( fileName, new RefCountedContainer( asset ) );
        }

        /** Updates the current task on the top of the task stack.
	 * @return true if the asset is loaded or the task was cancelled. */
        private bool UpdateTask()
        {
            AssetLoadingTask task = _tasks.peek();

            bool complete = true;

            try
            {
                complete = task.cancel || task.update();
            }
            catch ( RuntimeException ex )
            {
                task.cancel = true;
                TaskFailed( task.assetDesc, ex );
            }

            // if the task has been cancelled or has finished loading
            if ( complete )
            {
                // increase the number of loaded assets and pop the task from the stack
                if ( _tasks.size() == 1 )
                {
                    _loaded++;
                    _peakTasks = 0;
                }

                _tasks.pop();

                if ( task.cancel ) return true;

                AddAsset( task.assetDesc.fileName, task.assetDesc.type, task.asset );

                // otherwise, if a listener was found in the parameter invoke it
                if ( task.assetDesc.params != null && task.assetDesc.params.loadedCallback != null)
                task.assetDesc.params.loadedCallback.finishedLoading( this, task.assetDesc.fileName, task.assetDesc.type );

                long endTime = TimeUtils.nanoTime();
                _log.debug( "Loaded: " + ( endTime - task.startTime ) / 1000000f + "ms " + task.assetDesc );

                return true;
            }

            return false;
        }

        /** Called when a task throws an exception during loading. The default implementation rethrows the exception. A subclass may
	 * supress the default implementation when loading assets where loading failure is recoverable. */
        protected void TaskFailed( AssetDescriptor assetDesc, RuntimeException ex )
        {
            throw ex;
        }

        private void IncrementRefCountedDependencies( string parent )
        {
            Array< string > dependencies = _assetDependencies.get( parent );

            if ( dependencies == null ) return;

            for ( string dependency :
            dependencies) {
                Class               type     = _assetTypes.get( dependency );
                RefCountedContainer assetRef = _assets.get( type ).get( dependency );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( dependency );
            }
        }

        /** Handles a runtime/loading error in {@link #update()} by optionally invoking the {@link AssetErrorListener}.
	 * @param t */
        private void HandleTaskError( Throwable t )
        {
            _log.error( "Error loading asset.", t );

            if ( _tasks.isEmpty() ) throw new GdxRuntimeException( t );

            // pop the faulty task from the stack
            AssetLoadingTask task      = _tasks.pop();
            AssetDescriptor  assetDesc = task.assetDesc;

            // remove all dependencies
            if ( task.dependenciesLoaded && task.dependencies != null )
            {
                for ( AssetDescriptor desc :
                task.dependencies)
                Unload( desc.fileName );
            }

            // clear the rest of the stack
            _tasks.clear();

            // inform the listener that something bad happened
            if ( _listener != null )
                _listener.error( assetDesc, t );
            else
                throw new GdxRuntimeException( t );
        }

        /** Sets a new {@link AssetLoader} for the given type.
	 * @param type the type of the asset
	 * @param loader the loader */
        public synchronized< T, P Extends AssetLoaderParameters< T >> void SetLoader(
            Class< T > type,
            AssetLoader< T, P > loader )
        {
            SetLoader( type, null, loader );
        }

        /** Sets a new {@link AssetLoader} for the given type.
	 * @param type the type of the asset
	 * @param suffix the suffix the filename must have for this loader to be used or null to specify the default loader.
	 * @param loader the loader */
        public synchronized< T, P Extends AssetLoaderParameters< T >> void SetLoader( Class< T > type,
            string suffix,
            AssetLoader< T, P > loader )
        {
            if ( type == null ) throw new IllegalArgumentException( "type cannot be null." );
            if ( loader == null ) throw new IllegalArgumentException( "loader cannot be null." );

            _log.debug
                (
                 "Loader set: "
                 + ClassReflection.getSimpleName( type )
                 + " -> "
                 + ClassReflection.getSimpleName( loader.getClass() )
                );

            ObjectMap< string, AssetLoader > loaders = this._loaders.get( type );
            if ( loaders == null ) this._loaders.put( type, loaders = new ObjectMap< string, AssetLoader >() );
            loaders.put( suffix == null ? "" : suffix, loader );
        }

        /** @return the number of loaded assets */
        public synchronized int GetLoadedAssets()
        {
            return _assetTypes.size;
        }

        /** @return the number of currently queued assets */
        public synchronized int GetQueuedAssets()
        {
            return _loadQueue.size + _tasks.size();
        }

        /** @return the progress in percent of completion. */
        public synchronized float GetProgress()
        {
            if ( _toLoad == 0 ) return 1;
            float fractionalLoaded = _loaded;

            if ( _peakTasks > 0 )
            {
                fractionalLoaded += ( ( _peakTasks - _tasks.size() ) / ( float )_peakTasks );
            }

            return Math.min( 1, fractionalLoaded / _toLoad );
        }

        /** Sets an {@link AssetErrorListener} to be invoked in case loading an asset failed.
	 * @param listener the listener or null */
        public synchronized void SetErrorListener( AssetErrorListener listener )
        {
            this._listener = listener;
        }

        /** Disposes all assets in the manager and stops all asynchronous loading. */
        @Override

        public synchronized void Dispose()
        {
            _log.debug( "Disposing." );
            Clear();
            _executor.dispose();
        }

        /** Clears and disposes all assets and the preloading queue. */
        public synchronized void Clear()
        {
            _loadQueue.clear();

            while ( !Update() )
                ;

            ObjectIntMap< string > dependencyCount = new ObjectIntMap< string >();

            while ( _assetTypes.size > 0 )
            {
                // for each asset, figure out how often it was referenced
                dependencyCount.clear();
                Array< string > assets = _assetTypes.keys().toArray();
                for ( string asset :
                assets)
                dependencyCount.put( Asset, 0 );

                for ( string asset :
                assets) {
                    Array< string > dependencies = _assetDependencies.get( Asset );

                    if ( dependencies == null ) continue;
                    for ( string dependency :
                    dependencies) {
                        int count = dependencyCount.get( dependency, 0 );
                        count++;
                        dependencyCount.put( dependency, count );
                    }
                }

                // only dispose of assets that are root assets (not referenced)
                for ( string asset :
                assets)
                if ( dependencyCount.get( Asset, 0 ) == 0 ) Unload( Asset );
            }

            this._assets.clear();
            this._assetTypes.clear();
            this._assetDependencies.clear();
            this._loaded    = 0;
            this._toLoad    = 0;
            this._peakTasks = 0;
            this._loadQueue.clear();
            this._tasks.clear();
        }

        /** @return the {@link Logger} used by the {@link AssetManager} */
        public Logger GetLogger()
        {
            return _log;
        }

        public void SetLogger( Logger logger )
        {
            _log = logger;
        }

        /** Returns the reference count of an asset.
	 * @param fileName */
        public synchronized int GetReferenceCount( string fileName )
        {
            Class type = _assetTypes.get( fileName );

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            return _assets.get( type ).get( fileName ).getRefCount();
        }

        /** Sets the reference count of an asset.
	 * @param fileName */
        public synchronized void SetReferenceCount( string fileName, int refCount )
        {
            Class type = _assetTypes.get( fileName );

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );
            _assets.get( type ).get( fileName ).setRefCount( refCount );
        }

        /** @return a string containing ref count and dependency information for all assets. */
        public synchronized string GetDiagnostics()
        {
            StringBuilder sb = new StringBuilder( 256 );
            for ( string fileName :
            _assetTypes.keys()) {
                if ( sb.length() > 0 ) sb.append( "\n" );
                sb.append( FileName );
                sb.append( ", " );

                Class               type         = _assetTypes.get( FileName );
                RefCountedContainer assetRef     = _assets.get( type ).get( FileName );
                Array< string >     dependencies = _assetDependencies.get( FileName );

                sb.append( ClassReflection.getSimpleName( type ) );

                sb.append( ", refs: " );
                sb.append( assetRef.GetRefCount() );

                if ( dependencies != null )
                {
                    sb.append( ", deps: [" );
                    for ( string dep :
                    dependencies) {
                        sb.append( dep );
                        sb.append( "," );
                    }

                    sb.append( "]" );
                }
            }

            return sb.toString();
        }

        public Array<string> GetAssetNames()
        {
            lock ( this )
            {
                return _assetTypes.Keys().ToArray();
            }
        }

        public Array<string> GetDependencies( string name )
        {
            lock ( this )
            {
                Array< string > array = _assetDependencies.Get( name );
                return array;
            }
        }

        public Type GetAssetType( string name )
        {
            lock ( this )
            {
                Type type = _assetTypes.Get( name );
                return type;
            }
        }
    }
}
