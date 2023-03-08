namespace LibGDXSharp.Backends.Desktop
{
    public class GLApplicationConfiguration : GLWindowConfiguration
    {
        public bool DisableAudio           { get; set; } = false;
        public bool UseGL30                { get; set; } = false;
        public bool Debug                  { get; set; } = false;
        public bool TransparentFramebuffer { get; set; }

        public int MaxNetThreads                  { get; set; } = int.MaxValue;
        public int AudioDeviceSimultaneousSources { get; set; } = 16;
        public int AudioDeviceBufferSize          { get; set; } = 512;
        public int AudioDeviceBufferCount         { get; set; } = 9;
        public int Gles30ContextMajorVersion      { get; set; } = 3;
        public int Gles30ContextMinorVersion      { get; set; } = 2;
        public int R                              { get; set; } = 8;
        public int G                              { get; set; } = 8;
        public int B                              { get; set; } = 8;
        public int A                              { get; set; } = 8;
        public int Depth                          { get; set; } = 16;
        public int Stencil                        { get; set; } = 0;
        public int Samples                        { get; set; } = 0;
        public int IdleFPS                        { get; set; } = 60;
        public int ForegroundFPS                  { get; set; } = 0;

        public string         PreferencesDirectory { get; set; } = ".prefs/";
        public IFile.FileType PreferencesFileType  { get; set; } = IFile.FileType.External;
        public HdpiMode       HdpiMode             { get; set; } = HdpiMode.Logical;
        public PrintStream    DebugStream          { get; set; } = System.Err;

        static GLApplicationConfiguration Copy( GLApplicationConfiguration config )
        {
            var copy = new GLApplicationConfiguration();

            copy.Set( config );

            return copy;
        }

        private void Set( GLApplicationConfiguration config )
        {
            base.SetWindowConfiguration( config );

            DisableAudio                   = config.DisableAudio;
            AudioDeviceSimultaneousSources = config.AudioDeviceSimultaneousSources;
            AudioDeviceBufferSize          = config.AudioDeviceBufferSize;
            AudioDeviceBufferCount         = config.AudioDeviceBufferCount;
            UseGL30                        = config.UseGL30;
            Gles30ContextMajorVersion      = config.Gles30ContextMajorVersion;
            Gles30ContextMinorVersion      = config.Gles30ContextMinorVersion;
            R                              = config.R;
            G                              = config.G;
            B                              = config.B;
            A                              = config.A;
            Depth                          = config.Depth;
            Stencil                        = config.Stencil;
            Samples                        = config.Samples;
            TransparentFramebuffer         = config.TransparentFramebuffer;
            IdleFPS                        = config.IdleFPS;
            ForegroundFPS                  = config.ForegroundFPS;
            PreferencesDirectory           = config.PreferencesDirectory;
            PreferencesFileType            = config.PreferencesFileType;
            HdpiMode                       = config.HdpiMode;
            Debug                          = config.Debug;
            DebugStream                    = config.DebugStream;
        }

        /// <summary>
        /// Sets the audio device configuration.
        /// </summary>
        /// <param name="simultaniousSources">
        /// the maximum number of sources that can be played simultaniously (default 16)
        /// </param>
        /// <param name="bufferSize">the audio device buffer size in samples (default 512)</param>
        /// <param name="bufferCount">the audio device buffer count (default 9)</param>
        public void SetAudioConfig( int simultaniousSources, int bufferSize, int bufferCount )
        {
            this.AudioDeviceSimultaneousSources = simultaniousSources;
            this.AudioDeviceBufferSize          = bufferSize;
            this.AudioDeviceBufferCount         = bufferCount;
        }

        /// <summary>
        /// Sets whether to use OpenGL ES 3.0. If the given major/minor version is not
        /// supported, the backend falls back to OpenGL ES 2.0.
        /// </summary>
        /// <param name="useGL30">whether to use OpenGL ES 3.0</param>
        /// <param name="gles3MajorVersion">OpenGL ES major version, use 3 as default</param>
        /// <param name="gles3MinorVersion">OpenGL ES minor version, use 2 as default</param>
        public void UseOpenGL3( bool useGL30, int gles3MajorVersion = 3, int gles3MinorVersion = 2 )
        {
            this.UseGL30                   = useGL30;
            this.Gles30ContextMajorVersion = gles3MajorVersion;
            this.Gles30ContextMinorVersion = gles3MinorVersion;
        }

        /// <summary>
        /// Sets the bit depth of the color, depth and stencil buffer as well as
        /// multi-sampling.
        /// </summary>
        /// <param name="r">red bits (default 8)</param>
        /// <param name="g">green bits (default 8)</param>
        /// <param name="b">blue bits (default 8)</param>
        /// <param name="a">alpha bits (default 8)</param>
        /// <param name="depth">depth bits (default 16)</param>
        /// <param name="stencil">stencil bits (default 0)</param>
        /// <param name="samples">MSAA samples (default 0)</param>
        public void SetBackBufferConfig( int r, int g, int b, int a, int depth, int stencil, int samples )
        {
            this.R       = r;
            this.G       = g;
            this.B       = b;
            this.A       = a;
            this.Depth   = depth;
            this.Stencil = stencil;
            this.Samples = samples;
        }

        /// <summary>
        /// Sets the directory where <see cref="IPreferences"/> will be stored, as well as
        /// the file type to be used to store them. Defaults to "$USER_HOME/.prefs/"
        /// and <see cref="IFile.FileType"/>.
        ///</summary>
        public void SetPreferencesConfig( string preferencesDirectory, IFile.FileType preferencesFileType )
        {
            this.PreferencesDirectory = preferencesDirectory;
            this.PreferencesFileType  = preferencesFileType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="debugOutputStream"></param>
        public void EnableGLDebugOutput( bool enable, PrintStream debugOutputStream )
        {
            Debug       = enable;
            DebugStream = debugOutputStream;
        }

        /// <summary>
        /// Gets the currently active display mode for the primary monitor.
        /// </summary>
        public static DisplayMode GetDisplayMode()
        {
            GLApplication.InitialiseGL();

//            Glfw.GetApi().SetVideoMode();

            var videoMode = Glfw.GetApi().GetVideoMode( Glfw.GetApi().GetPrimaryMonitor() );
            
            return new DisplayMode
                (
                 Glfw.GetApi().GetPrimaryMonitor(),
                 videoMode.width(),
                 videoMode.height(),
                 videoMode.refreshRate(),
                 videoMode.redBits() + videoMode.greenBits() + videoMode.blueBits()
                );
        }


    }
}
