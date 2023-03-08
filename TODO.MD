- IP = In Progress
- - - - - - - - -
- DONE = Class finished but may not be fully CSharp-ified
- - - - - - - - - - - - - - - - - - - - - - - - -
CORE
----
    - DONE - AbstractGraphics
    - DONE - AbstractInput
    - DONE - Application
    - DONE - ApplicationAdapter
    - DONE - Game
    - DONE - Gdx
    - DONE - IApplicationListener
    - DONE - IApplicationLogger
    - DONE - IAudio
    - DONE - IFile
    - DONE - IGraphics
    - DONE - IInput
    - DONE - IInputProcessor
    - DONE - ILifecycleListener
    - DONE - INet
    - DONE - InputAdapter
    - DONE - InputEventQueue
    - DONE - IPreferences
    - DONE - IScreen
    - DONE - ScreenAdapter
    - DONE - Version

ASSETS
------
    - DONE - AssetDescriptor
    - DONE - AssetLoaderParameters
    - IP   - AssetLoadingTask
    - IP   - AssetManager
    - DONE - IAssetErrorListener
    -      - RefCountedContainer

ASSETS/LOADERS
--------------
    - IP   - AssetLoader
    - DONE - AsynchronousAssetLoader
    - IP   - BitmapFontLoader
    -      - CubemapLoader
    -      - I18NBundleLoader
    - DONE - IFileHandleResolver
    -      - ModelLoader
    -      - MusicLoader
    -      - ParticleEffectLoader
    -      - PixmapLoader
    -      - ShaderProgramLoader
    -      - SkinLoader
    -      - SoundLoader
    - DONE - SynchronousAssetLoader
    -      - TextureAtlasLoader
    - IP   - TextureLoader

ASSETS/LOADERS/RESOLVERS
------------------------
    - DONE - AbsoluteFileHandleResolver
    -      - ClasspathFileHandleResolver
    -      - ExternalFileHandleResolver
    -      - InternalFileHandleResolver
    -      - LocalFileHandleResolver
    -      - PrefixFileHandleResolver
    -      - ResolutionFileResolver

AUDIO
-----
    - DONE - IAudioDevice
    - DONE - IAudioRecorder
    - DONE - IMusic
    - DONE - ISound

FILES
-----
    - DONE - FileHandle
    - DONE - FileHandleStream

GRAPHICS
--------
    - DONE - BufferFormat
    -      - Camera
    - DONE - Color
    -      - Colors
    -      - Cubemap
    -      - CubemapData
    - DONE - DisplayMode
    -      - FPSLogger
    -      - GLTexture
    - DONE - GraphicsType
    - DONE - ICursor
    - IP   - IGL20
    -      - IGL30
    - IP   - ITextureData
    -      - Mesh
    - DONE - Monitor
    -      - OrthographicCamera
    -      - PerspectiveCamera
    - IP   - Pixmap
    -      - PixmapIO
    - IP   - Texture
    -      - TextureArray
    -      - TextureArrayData
    - DONE - TextureFilter
    -      - VertexAttribute
    -      - VertexAttributes

GRAPHICS/G2D
------------
    -      - Animation
    -      - Batch
    - IP   - BitmapFont
    -      - BitmapFontCache
    -      - CpuSpriteBatch
    -      - DistanceFieldFont
    -      - Gdx2DPixmap
    -      - GlyphLayout
    -      - NinePatch
    -      - ParticleEffect
    -      - ParticleEffectPool
    -      - ParticleEmitter
    -      - PixmapPacker
    -      - PixmapPacker10
    -      - PolygonBatch
    -      - PolygonRegion
    -      - PolygonRegionLoader
    -      - PolygonSprite
    -      - PolygonSpriteBatch
    -      - RepeatablePolygonSprite
    -      - Sprite
    -      - SpriteBatch
    -      - SpriteCache
    -      - TextureAtlas
    - IP   - TextureRegion

GRAPHICS/G3D
------------
    -      - 

GRAPHICS/GLUTILS
----------------
    -      - ETC1
    -      - ETC1TextureData
    -      - FacedCubemapData
    -      - FileTextureArrayData
    -      - FileTextureData
    -      - FloatFrameBuffer
    -      - FloatTextureData
    -      - FrameBuffer
    -      - FrameBufferCubemap
    -      - GLFrameBuffer
    -      - GLOnlyTextureData
    -      - GLVersion
    - IP   - HdpiMode
    -      - HdpiUtils
    -      - ImmediateModeRenderer
    -      - ImmediateModeRenderer20
    -      - IndexArray
    -      - IndexBufferObject
    -      - IndexBufferObjectSubData
    -      - IndexData
    -      - InstanceBufferObject
    -      - InstanceBufferObjectSubData
    -      - InstanceData
    -      - KTXTTextureData
    -      - MipMapGenerator
    -      - MipMapTextureData
    -      - PixmapTextureData
    -      - ShaderProgram
    -      - ShapeRenderer
    -      - VertexArray
    -      - VertexBufferObject
    -      - VertexBufferObjectSubData
    -      - VertexBufferObjectWithVAO
    -      - VertexData

GRAPHICS/PROFILING
------------------
    -      - GL20Interceptor
    -      - GL30Interceptor
    -      - GLErrorListener
    -      - GLInterceptor
    -      - GLProfiler

INPUT
-----
    -      - GestureDetector
    -      - RemoteInput
    -      - RemoteSender

MAPS
----
    - IP   - IImageResolver
    - IP   - IMapRenderer
    - IP   - Map
    - DONE - MapGroupLayer
    - IP   - MapLayer
    - DONE - MapLayers
    - DONE - MapObject
    - DONE - MapObjects
    - DONE - MapProperties

MAPS/OBJECTS
------------
    -      - CircleMapObject
    -      - EllipseMapObject
    -      - PolygonMapObject
    -      - PolylineMapObject
    -      - RectangleMapObject
    -      - TextureMapObject

MAPS/TILED
----------
    - IP   - ITiledMapTile
    - DONE - TiledMap
    - DONE - TiledMapImageLayer
    - DONE - TiledMapTileLayer
    - DONE - TiledMapTileSet
    - IP   - TiledMapTileSets

MAPS/TILED/LOADERS
------------------
    -      - AtlasTmxMapLoader
    - IP   - BaseTmxMapLoader
    -      - TideMapLoader
    - IP   - TmxMapLoader

MAPS/TILED/OBJECTS
------------------
    -      - TiledMapTileMapObject

MAPS/TILED/RENDERERS
--------------------
    - IP   - BatchTiledMapRenderer
    -      - HexagonalTiledMapRenderer
    -      - IsometricStaggeredTiledMapRenderer
    -      - IsometricTiledMapRenderer
    -      - ITiledMapRenderer
    -      - OrthoCachedTiledMapRenderer
    - IP   - OrthogonalTiledMapRenderer

MAPS/TILED/TILES
----------------
    -      - AnimatedTileMapTile
    -      - StaticTiledMapTile

MATH
----
    -      - Affine2
    -      - Bezier
    -      - Bresenham
    -      - BSpline
    -      - CatmullRomSpline
    -      - Circle
    -      - ConvexHull
    -      - CumulativeDistribution
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    -      - Ellipse
    -      - FloatCounter
    -      - Frustrum
    -      - GeometryUtils
    -      - GridPoint2
    -      - GridPoint3
    -      - Interpolation
    -      - Intersector
    -      - IPath
    - IP   - MathUtils
    -      - Matrix3
    -      - Matrix4
    -      - Plane
    -      - Polygon
    -      - Polyline
    -      - Quaternion
    -      - RandomXS128
    - IP   - Rectangle
    -      - Shape2D
    -      - WindowedMean

MATH/COLLISION
--------------
    -      - BoundingBox
    -      - Ray
    -      - Segment
    -      - Sphere

NETWORK
-------
    -      - HttpParametersUtils
    -      - HttpRequestBuilder
    -      - HttpStatus
    -      - IHttpRequestHeader
    -      - IHttpResponseHeader
    -      - IServerSocket
    -      - ISocket
    -      - NetJavaImpl
    -      - NetJavaServerSocketImpl
    -      - NetJavaSocketImpl
    -      - ServerSocketHints
    -      - SocketHints

SCENES.SCENE2D
--------------
    - DONE - Action
    - IP   - Actor
    - DONE - Event
    -      - Group
    - DONE - IEventListener
    -      - InputEvent
    -      - InputListener
    - IP   - Stage
    - DONE - Touchable

SCENES.SCENE2D/ACTIONS
----------------------
    -      - Actions
    -      - AddAction
    -      - AddListenerAction
    -      - AfterAction
    -      - AlphaAction
    -      - ColorAction
    -      - CountdownEventAction
    -      - DelayAction
    -      - DelegateAction
    -      - EventAction
    -      - FloatAction
    -      - IntAction
    -      - LayoutAction
    -      - MoveByAction
    -      - MoveToAction
    -      - ParallelAction
    -      - RelativeTemporalAction
    -      - RemoveAction
    -      - RemoveActorAction
    -      - RemoveListenerAction
    -      - RepeatAction
    -      - RotateByAction
    -      - RotateToAction
    -      - RunnableAction
    -      - ScaleByAction
    -      - ScaleToAction
    -      - SequenceAction
    -      - SizeByAction
    -      - SizeToAction
    -      - TemporalAction
    -      - TimeScaleAction
    -      - TouchableAction
    -      - VisibleAction

SCENES.SCENE2D/UI
-----------------
    -      - Button
    -      - ButtonGroup
    -      - Cell
    -      - CheckBox
    -      - Container
    -      - Dialog
    -      - HorizontalGroup
    -      - Image
    -      - ImageButton
    -      - ImageTextButton
    -      - Label
    -      - List
    -      - ParticleEffectActor
    -      - ProgressBar
    -      - ScrollPane
    -      - SelectBox
    -      - Skin
    -      - Slider
    -      - SplitPane
    -      - Stack
    -      - Table
    -      - TextArea
    -      - TextButton
    -      - TextField
    -      - TextTooltip
    -      - TooltipManager
    -      - Touchpad
    -      - Tree
    -      - Value
    -      - VerticalGroup
    -      - Widget
    -      - WidgetGroup
    -      - Window

SCENES.SCENE2D/UTILS
--------------------
    -      - ActorGestureListener
    -      - ArraySelection
    -      - BaseDrawable
    -      - ChangeListener
    -      - ClickListener
    -      - DragAndDrop
    -      - DragListener
    -      - DragScrollListener
    -      - FocusListener
    -      - ICullable
    -      - IDisableable
    -      - IDrawable
    -      - ILayout
    -      - ITransformDrawable
    -      - NinePatchDrawable
    -      - ScissorStack
    -      - Selection
    -      - SpriteDrawable
    -      - TextureRegionDrawable
    -      - TiledDrawable
    -      - UIUtils

UTILS
-----
    - DONE - Align
    -      - AtomicQueue
    -      - Base64Coder
    -      - BinaryHeap
    -      - Bits
    -      - Buffer
    -      - BufferUtils
    -      - IClipboard
    -      - ComparableTimSort
    -      - DataBuffer
    -      - DataInput
    -      - DataOutput
    - IP   - FloatConsts
    -      - FlushablePool
    -      - GdxNativesLoader
    - DONE - GdxRuntimeException
    -      - I18NBundle
    - DONE - IllegalStateException
    - IP   - IPoolable
    - IP   - IRunnable
    -      - LittleEndianInputStream
    -      - Logger
    - IP   - NumberUtils
    -      - PauseableThread
    -      - PerformanceCounter
    -      - PerformanceCounters
    -      - Pool
    -      - PooledLinkedList
    -      - Pools
    -      - PropertiesUtils
    -      - QuadTreeFloat
    -      - Queue
    -      - QuickSelect
    -      - ReflectionPool
    -      - Scaling
    -      - ScreenUtils
    - IP   - Selector (aka Libgdx Select)
    -      - SerializationException
    -      - SharedLibraryLoader
    -      - Sort
    -      - SortedIntList
    -      - StreamUtils
    -      - StringBuilder
    -      - TextFormatter
    -      - Timer
    -      - TimeUtils
    -      - TimSort

UTILS/ASYNC
-----------
    -      - AsyncExecutor
    -      - AsyncResult
    -      - IAsyncTask
    -      - ThreadUtils

UTILS/COLLECTIONS
-----------------
    - IP   - Array
    - IP   - ArrayIterable
    - IP   - ArrayIterator
    - IP   - ArrayMap
    -      - BooleanArray
    -      - ByteArray
    -      - CharArray
    -      - Collections
    - IP   - DelayedRemovalArray
    -      - FloatArray
    -      - IdentityMap
    -      - IntArray
    -      - IntFloatMap
    -      - IntIntMap
    -      - IntMap
    -      - IntSet
    - IP   - IPredicate
    -      - LongArray
    -      - LongMap
    -      - ObjectFloatMap
    -      - ObjectIntMap
    -      - ObjectLongMap
    - IP   - ObjectMap
    -      - ObjectSet
    -      - OrderedMap
    -      - OrderedSet
    - IP   - PredicateIterable
    - IP   - PredicateIterator
    -      - ShortArray
    - IP   - SnapshotArray

UTILS/COMPRESSION
-----------------
    -      - CRC
    -      - ICodeProgress
    -      - LzmaAdapted

    UTILS/COMPRESSION/LZ
    --------------------
            -      - BinTree
            -      - InWindow
            -      - OutWindow
    
    UTILS/COMPRESSION/LZMA
    ----------------------
            -      - Base
            -      - Decoder
            -      - Encoder
    
    UTILS/COMPRESSION/RANGECODER
    ----------------------------
            -      - BitTreeDecoder
            -      - BitTreeEncoder
            -      - Decoder
            -      - Encoder

UTILS/JSON
----------
    -      - IBaseJsonReader
    -      - Json
    -      - JsonReader
    -      - JsonValue
    -      - JsonWriter
    -      - UBJsonReader
    -      - UBJsonWriter

UTILS/REFLECT
-------------
    -      - Annotation
    -      - ArrayReflection
    -      - ClassReflection
    -      - Constructor
    -      - Field
    -      - Method
    -      - ReflectionException

UTILS/VIEWPORT
--------------
    -      - ExtendedViewport
    -      - FillViewport
    -      - FitViewport
    -      - ScalingViewport
    -      - ScreenViewport
    -      - StretchViewport
    -      - Viewport

UTILS/XML
---------
    -      - XmlReader
    -      - XmlWriter

