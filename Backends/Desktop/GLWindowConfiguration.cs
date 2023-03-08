namespace LibGDXSharp.Backends.Desktop
{
    public class GLWindowConfiguration
    {
        public int                WindowX                { get; set; } = -1;
        public int                WindowY                { get; set; } = -1;
        public int                WindowWidth            { get; set; } = 640;
        public int                WindowHeight           { get; set; } = 480;
        public int                WindowMinWidth         { get; set; } = -1;
        public int                WindowMinHeight        { get; set; } = -1;
        public int                WindowMaxWidth         { get; set; } = -1;
        public int                WindowMaxHeight        { get; set; } = -1;
        public bool               WindowResizable        { get; set; } = true;
        public bool               WindowDecorated        { get; set; } = true;
        public bool               WindowMaximized        { get; set; } = false;
        public bool               AutoIconify            { get; set; } = false;
        public Color              InitialBackgroundColor { get; set; } = Color.Black;
        public bool               InitialVisible         { get; set; } = true;
        public bool               VSyncEnabled           { get; set; } = true;
        public GLMonitor?         MaximizedMonitor       { get; set; }
        public IFile.FileType     WindowIconFileType     { get; set; }
        public string[]?          WindowIconPaths        { get; set; }
        public IGLWindowListener? WindowListener         { get; set; }
        public GLDisplayMode?     FullscreenMode         { get; set; }
        public string?            Title                  { get; set; }

        internal void SetWindowConfiguration( GLWindowConfiguration config )
        {
            WindowX            = config.WindowX;
            WindowY            = config.WindowY;
            WindowWidth        = config.WindowWidth;
            WindowHeight       = config.WindowHeight;
            WindowMinWidth     = config.WindowMinWidth;
            WindowMinHeight    = config.WindowMinHeight;
            WindowMaxWidth     = config.WindowMaxWidth;
            WindowMaxHeight    = config.WindowMaxHeight;
            WindowResizable    = config.WindowResizable;
            WindowDecorated    = config.WindowDecorated;
            WindowMaximized    = config.WindowMaximized;
            MaximizedMonitor   = config.MaximizedMonitor;
            AutoIconify        = config.AutoIconify;
            WindowIconFileType = config.WindowIconFileType;

            if ( config.WindowIconPaths != null )
            {
                WindowIconPaths = new string[ config.WindowIconPaths.Length ];

                Array.Copy( config.WindowIconPaths, WindowIconPaths, config.WindowIconPaths.Length );
            }

            WindowListener         = config.WindowListener;
            FullscreenMode         = config.FullscreenMode;
            Title                  = config.Title;
            InitialBackgroundColor = config.InitialBackgroundColor;
            InitialVisible         = config.InitialVisible;
            VSyncEnabled           = config.VSyncEnabled;
        }

        ///
        /// @param visibility whether the window will be visible on creation. (default true)
        ///
        public void SetInitialVisible( bool visibility )
        {
            this.InitialVisible = visibility;
        }

        ///
        /// Sets the app to use windowed mode.
        /// 
        /// @param width
        ///            the width of the window (default 640)
        /// @param height
        ///            the height of the window (default 480)
        ///
        public void SetWindowedMode( int width, int height )
        {
            this.WindowWidth  = width;
            this.WindowHeight = height;
        }

        /// 
        /// @param resizable whether the windowed mode window is resizable (default true)
        ///
        public void SetResizable( bool resizable )
        {
            this.WindowResizable = resizable;
        }

        ///
        /// @param decorated whether the windowed mode window is decorated, i.e. displaying the title bars (default true)
        ///
        public void SetDecorated( bool decorated )
        {
            this.WindowDecorated = decorated;
        }

        ///
        /// @param maximized whether the window starts maximized. Ignored if the window is full screen. (default false)
        ///
        public void SetMaximized( bool maximized )
        {
            this.WindowMaximized = maximized;
        }

        ///
        /// @param monitor what monitor the window should maximize to
        ///
        public void SetMaximizedMonitor( GLMonitor monitor )
        {
            this.MaximizedMonitor = monitor;
        }

        /// 
        /// @param autoIconify whether the window should automatically iconify and restore previous video mode on input focus loss. (default false)
        ///                    Does nothing in windowed mode.
        /// 
        public void SetAutoIconify( bool autoIconify )
        {
            this.AutoIconify = autoIconify;
        }

        ///
        /// Sets the position of the window in windowed mode.
        /// Default -1 for both coordinates for centered on primary monitor.
        ///
        public void SetWindowPosition( int x, int y )
        {
            WindowX = x;
            WindowY = y;
        }

        ///
        /// Sets minimum and maximum size limits for the window. If the window is full screen or not resizable, these 
        /// limits are ignored. The default for all four parameters is -1, which means unrestricted.
        ///
        public void SetWindowSizeLimits( int minWidth, int minHeight, int maxWidth, int maxHeight )
        {
            WindowMinWidth  = minWidth;
            WindowMinHeight = minHeight;
            WindowMaxWidth  = maxWidth;
            WindowMaxHeight = maxHeight;
        }

        ///
        /// Sets the icon that will be used in the window's title bar. Has no effect in macOS, which doesn't use window icons.
        /// @param filePaths One or more {@linkplain FileType#Internal internal} image paths. Must be JPEG, PNG, or BMP format.
        /// The one closest to the system's desired size will be scaled. Good sizes include 16x16, 32x32 and 48x48.
        ///
        public void SetWindowIcon( params string[] filePaths )
        {
            SetWindowIcon( IFile.FileType.Internal, filePaths );
        }

        ///
        /// Sets the icon that will be used in the window's title bar. Has no effect in macOS, which doesn't use window icons.
        /// @param fileType The type of file handle the paths are relative to.
        /// @param filePaths One or more image paths, relative to the given {@linkplain FileType}. Must be JPEG, PNG, or BMP format. 
        /// The one closest to the system's desired size will be scaled. Good sizes include 16x16, 32x32 and 48x48.
        ///
        public void SetWindowIcon( IFile.FileType fileType, params string[] filePaths )
        {
            WindowIconFileType = fileType;
            WindowIconPaths    = filePaths;
        }

        ///
        /// Sets the {@link GLWindowListener} which will be informed about
        /// iconficiation, focus loss and window close events.
        ///
        public void SetWindowListener( IGLWindowListener windowListener )
        {
            this.WindowListener = windowListener;
        }

        ///
        /// Sets the app to use fullscreen mode. Use the static methods like
        /// {@link GLApplicationConfiguration#getDisplayMode()} on this class to enumerate connected monitors
        /// and their fullscreen display modes.
        ///
        public void SetFullscreenMode( GLDisplayMode mode )
        {
            this.FullscreenMode = mode;
        }

        ///
        /// Sets the window title. Defaults to empty string.
        ///
        public void SetTitle( string title )
        {
            this.Title = title;
        }

        ///
        /// Sets the initial background color. Defaults to black.
        ///
        public void SetInitialBackgroundColor( Color color )
        {
            InitialBackgroundColor = color;
        }

        ///
        /// Sets whether to use vsync. This setting can be changed anytime at runtime
        /// via {@link Graphics#setVSync(bool)}.
        ///
        /// For multi-window applications, only one (the main) window should enable vsync.
        /// Otherwise, every window will wait for the vertical blank on swap individually,
        /// effectively cutting the frame rate to (refreshRate / numberOfWindows).
        ///
        public void UseVsync( bool vsync )
        {
            this.VSyncEnabled = vsync;
        }
    }
}
