using Monitor = LibGDXSharp.Graphics.Monitor;

namespace LibGDXSharp.Core
{
    public abstract class AbstractGraphics : IGraphics
    {
        public float GetRawDeltaTime()
        {
            return GetDeltaTime();
        }

        public float GetDensity()
        {
            return GetPpiX() / 160f;
        }

        public float GetBackBufferScale()
        {
            return GetBackBufferWidth() / (float)GetWidth();
        }

        // ========================================================================
        // Abstract methods because C# insists this is done to fulfill the contract
        // between the class and interface.
        
        public abstract bool SupportsDisplayModeChange();

        public abstract Monitor GetPrimaryMonitor();

        public abstract Monitor GetMonitor();

        public abstract Monitor[] GetMonitors();

        public abstract DisplayMode[] GetDisplayModes();

        public abstract DisplayMode[] GetDisplayModes( Monitor monitor );

        public abstract DisplayMode GetDisplayMode();

        public abstract DisplayMode GetDisplayMode( Monitor monitor );

        public abstract bool SetFullscreenMode( DisplayMode displayMode );

        public abstract bool SetWindowedMode( int width, int height );

        public abstract void SetTitle( string title );

        public abstract void SetUndecorated( bool undecorated );

        public abstract void SetResizable( bool resizable );

        public abstract void SetVSync( bool vsync );

        public abstract void SetForegroundFps( int fps );

        public abstract BufferFormat GetBufferFormat();

        public abstract bool SupportsExtension( string extension );

        public abstract void SetContinuousRendering( bool isContinuous );

        public abstract bool IsContinuousRendering();

        public abstract void RequestRendering();

        public abstract bool IsFullscreen();

        public abstract ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );

        public abstract void SetCursor( ICursor cursor );

        public abstract void SetSystemCursor( ICursor.SystemCursor systemCursor );

        public abstract bool IsGl30Available();

        public abstract IGL20 GetGl20();

        public abstract IGL30 GetGl30();

        public abstract void SetGl20( IGL20 gl20 );

        public abstract void SetGl30( IGL30 gl30 );

        public abstract int GetWidth();

        public abstract int GetHeight();

        public abstract int GetBackBufferWidth();

        public abstract int GetBackBufferHeight();

        public abstract int GetSafeInsetLeft();

        public abstract int GetSafeInsetTop();

        public abstract int GetSafeInsetBottom();

        public abstract int GetSafeInsetRight();

        public abstract long GetFrameId();

        public abstract float GetDeltaTime();

        public abstract int GetFramesPerSecond();

        public abstract GraphicsType GetGraphicsType();

        public abstract GLVersion GetGlVersion();

        public abstract float GetPpiX();

        public abstract float GetPpiY();

        public abstract float GetPpcX();
        
        public abstract float GetPpcY();
    }
}
