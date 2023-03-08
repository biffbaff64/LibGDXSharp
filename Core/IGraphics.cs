
using Monitor = LibGDXSharp.Graphics.Monitor;

namespace LibGDXSharp
{
    public interface IGraphics
    {
        /// <summary>
        /// Returns whether OpenGL ES 3.0 is available.
        /// If it is you can get an instance of GL30 via GetGL30() to access
        /// OpenGL ES 3.0 functionality. Note that this functionality will
        /// only be available if you instructed the Application instance
        /// to use OpenGL ES 3.0!
        /// </summary>
        /// <returns>TRUE if available.</returns>
        bool IsGl30Available();

        /// <summary>
        /// Returns the <see cref="IGL20"/> instance.
        /// </summary>
        /// <remarks>MAY be replaced by a property.</remarks>
        IGL20 GetGl20();

        /// <summary>
        /// Returns the <see cref="IGL30"/> instance or null if unsupported.
        /// </summary>
        /// <remarks>MAY be replaced by a property.</remarks>
        IGL30 GetGl30();

        /// <summary>
        /// Sets the IGL20 instance.
        /// </summary>
        /// <remarks>MAY be replaced by a property.</remarks>
        void SetGl20( IGL20 gl20 );

        /// <summary>
        /// Sets the IGL30 instance.
        /// </summary>
        /// <remarks>MAY be replaced by a property.</remarks>
        void SetGl30( IGL30 gl30 );

        int GetWidth();

        int GetHeight();

        int GetBackBufferWidth();

        int GetBackBufferHeight();

        float GetBackBufferScale();

        int GetSafeInsetLeft();

        int GetSafeInsetTop();

        int GetSafeInsetBottom();

        int GetSafeInsetRight();

        long GetFrameId();

        float GetDeltaTime();

        int GetFramesPerSecond();

        GraphicsType GetGraphicsType();

        GLVersion GetGlVersion();

        float GetPpiX();

        float GetPpiY();

        float GetPpcX();

        float GetPpcY();

        /// <summary>
        /// This is a scaling factor for the Density Independent Pixel
        /// unit, following the convention where one DIP is one pixel on
        /// an approximately 160 dpi screen. Thus on a 160dpi screen this
        /// density value will be 1; on a 120 dpi screen it would be .75; etc.
        /// </summary>
        /// <returns>the Density Independent Pixel factor of the display.</returns>
        float GetDensity();

        bool SupportsDisplayModeChange();

        Monitor GetPrimaryMonitor();

        Monitor GetMonitor();

        Monitor[] GetMonitors();

        DisplayMode[] GetDisplayModes();

        DisplayMode[] GetDisplayModes( Monitor monitor );

        DisplayMode GetDisplayMode();

        DisplayMode GetDisplayMode( Monitor monitor );

        bool SetFullscreenMode( DisplayMode displayMode );

        bool SetWindowedMode( int width, int height );

        void SetTitle( string title );

        void SetUndecorated( bool undecorated );

        void SetResizable( bool resizable );

        void SetVSync( bool vsync );

        void SetForegroundFps( int fps );

        BufferFormat GetBufferFormat();

        bool SupportsExtension( string extension );

        void SetContinuousRendering( bool isContinuous );

        bool IsContinuousRendering();

        void RequestRendering();

        /// <summary>
        /// Whether the app is full screen or not.
        /// </summary>
        bool IsFullscreen();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixmap"></param>
        /// <param name="xHotspot"></param>
        /// <param name="yHotspot"></param>
        /// <returns></returns>
        ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );

        /// <summary>
        /// Only viable on the lwjgl-backend and on the gwt-backend.
        /// Browsers that support cursor:url() and support the png format (the pixmap
        /// is converted to a data-url of type image/png) should also support custom
        /// cursors. Will set the mouse cursor image to the image represented by the
        /// Cursor. It is recommended to call this function in the main render thread,
        /// and maximum one time per frame.
        /// </summary>
        /// <param name="cursor">The mouse cursor as a <see cref="ICursor"/></param>
        void SetCursor( ICursor cursor );

        /// <summary>
        /// Sets one of the predefined <see cref="ICursor.SystemCursor"/>s.
        /// </summary>
        /// <param name="systemCursor">The system cursor to use.</param>
        void SetSystemCursor( ICursor.SystemCursor systemCursor );
    }
}
