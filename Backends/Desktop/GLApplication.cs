using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp
{
    public class GLApplication : GLApplicationBase
    {
        public GLApplicationConfiguration?        Config             { get; set; }
        public List< GLWindow >                   Windows            { get; set; } = new List< GLWindow >();
        public GLClipboard?                       Clipboard          { get; set; }
        public ObjectMap< string, IPreferences >? Preferences        { get; set; }
        public List< IRunnable >                  Runnables          { get; set; } = new List< IRunnable >();
        public List< IRunnable >                  ExecutedRunnables  { get; set; } = new List< IRunnable >();
        public List< ILifecycleListener >         LifecycleListeners { get; set; } = new List< ILifecycleListener >();

        public static GLVersion? GLVersion { get; set; }

        private static   GlfwCallbacks.ErrorCallback? _errorCallback   = null;
        private static   Callback?                    _glDebugCallback = null;
        private volatile GLWindow?                    _currentWindow   = null;
        private readonly Sync?                        _sync;

        public static void InitialiseGL()
        {
            if ( _errorCallback == null )
            {
                GLNativesLoader.Load();

                // TODO: Create error callback

                Glfw.GetApi().InitHint( InitHint.JoystickHatButtons, false );
                
                if ( Glfw.GetApi().Init() )
                {
                    throw new GdxRuntimeException( "Unable to initialise Glfw!" );
                }
            }
        }

        public GLApplication( IGLWindowListener listener, GLApplicationConfiguration config )
        {
            InitialiseGL();

            ApplicationLogger = new GLApplicationLogger();
        }

        protected void Loop()
        {
        }
        
        public override void Log( string tag, string message )
        {
            if ( LogLevel >= LogInfo )
            {
                ApplicationLogger.Log( tag, message );
            }
        }

        public override void Log( string tag, string message, Exception exception )
        {
            if ( LogLevel >= LogInfo )
            {
                ApplicationLogger.Log( tag, message, exception );
            }
        }

        public override void Error( string tag, string message )
        {
            if ( LogLevel >= LogError )
            {
                ApplicationLogger.Error( tag, message );
            }
        }

        public override void Error( string tag, string message, Exception exception )
        {
            if ( LogLevel >= LogError )
            {
                ApplicationLogger.Error( tag, message, exception );
            }
        }

        public override void Debug( string tag, string message )
        {
            if ( LogLevel >= LogDebug )
            {
                ApplicationLogger.Debug( tag, message );
            }
        }

        public override void Debug( string tag, string message, Exception exception )
        {
            if ( LogLevel >= LogDebug )
            {
                ApplicationLogger.Debug( tag, message, exception );
            }
        }

        public override int GetVersion()
        {
            return 0;
        }

        public override ApplicationType Type { get; set; }

        public override IPreferences GetPreferences( string name )
        {
            return null;
        }

        public override void Exit()
        {
        }

        public override void AddLifecycleListener( ILifecycleListener listener )
        {
        }

        public override void RemoveLifecycleListener( ILifecycleListener listener )
        {
        }
    }
}
