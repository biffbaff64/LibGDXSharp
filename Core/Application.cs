namespace LibGDXSharp
{
    // TODO: #1 Update this as LibGDXSharp is desktop only.
    // TODO: #2 The Java LibGDX implements this as an interface. LibGDXSharp converts this to an abstract class. 
    /// <summary>
    /// An Application is the main entry point of your project. It sets up a window and
    /// rendering surface and manages the different aspects of your application, namely
    /// Graphics, Audio, Input and Files.
    /// This interface should be implemented in an application class for each supported
    /// backend.
    /// Each application class has it's own startup and initialisation methods. Please
    /// refer to their documentation for more information.
    /// While game programmers are used to having a main loop, libgdx employs a different
    /// concept to accommodate the event based nature of Android applications a little more.
    /// Your application logic must be implemented in a ApplicationListener which has
    /// methods that get called by the Application when the application is created, resumed,
    /// paused, disposed or rendered. As a developer you will simply implement the
    /// ApplicationListener interface and fill in the functionality accordingly. The
    /// ApplicationListener is provided to a concrete Application instance as a parameter to
    /// the constructor or another initialization method. Please refer to the documentation of
    /// the Application implementations for more information. Note that the ApplicationListener
    /// can be provided to any Application implementation. This means that you only need to write
    /// your program logic once and have it run on different platforms by passing it to a concrete
    /// Application implementation.
    /// The Application interface provides you with a set of modules for graphics, audio, input
    /// and file i/o.
    /// Graphics offers you various methods to output visuals to the screen. This is achieved via
    /// OpenGL ES 2.0 or 3.0 depending on what's available an the platform. On the desktop the features
    /// of OpenGL ES 2.0 and 3.0 are emulated via desktop OpenGL.
    /// Audio offers you various methods to output and record sound and music.
    /// Input offers you various methods to poll user input from the keyboard, touch screen, mouse
    /// and accelerometer. Additionally you can implement an InputProcessor and use it with
    /// Input.setInputProcessor(InputProcessor) to receive input events.
    /// Files offers you various methods to access internal and external files. An internal file is
    /// a file that is stored near your application. On the desktop the classpath is first scanned for
    /// the specified file. If that fails then the root directory of your application is used for a
    /// look up. External files are resources you create in your application and write to an external
    /// storage. On the desktop external files are written to a users home directory. If you know what
    /// you are doing you can also specify absolute file names. Absolute filenames are not portable,
    /// so take great care when using this feature.
    /// Net offers you various methods to perform network operations, such as performing HTTP requests,
    /// or creating server and client sockets for more elaborate network programming.
    /// The Application also has a set of methods that you can use to query specific information such
    /// as the operating system the application is currently running on and so forth. This allows you
    /// to have operating system dependent code paths. It is however not recommended to use this
    /// facilities.
    /// The Application also has a simple logging method which will print to standard out on the desktop.
    /// </summary>
    public abstract class Application
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ApplicationType
        {
            Android,
            Desktop,
            HeadlessDesktop,
            WebGL,
        }

        public const int LogNone  = 0;
        public const int LogError = 1;
        public const int LogInfo  = 2;
        public const int LogDebug = 3;

        public int  LogLevel  { get; set; }
        public bool IsRunning { get; set; } = true;

        public IApplicationListener ApplicationListener { get; set; }
        public IApplicationLogger   ApplicationLogger   { get; set; }
        public IGraphics            Graphics            { get; set; }
        public IAudio               Audio               { get; set; }
        public IInput               Input               { get; set; }
        public IFile                Files               { get; set; }
        public INet                 Net                 { get; set; }

        public abstract void Log( string tag, string message );
        public abstract void Log( string tag, string message, Exception exception );
        public abstract void Error( string tag, string message );
        public abstract void Error( string tag, string message, Exception exception );
        public abstract void Debug( string tag, string message );
        public abstract void Debug( string tag, string message, Exception exception );

        public abstract int GetVersion();

        public abstract ApplicationType Type { get; set; }
        public abstract IPreferences    GetPreferences( string name );

        public abstract void Exit();
        public abstract void AddLifecycleListener( ILifecycleListener listener );
        public abstract void RemoveLifecycleListener( ILifecycleListener listener );
    }
}
