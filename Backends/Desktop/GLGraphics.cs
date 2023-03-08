namespace LibGDXSharp.Backends.Desktop
{
    public class GLGraphics : AbstractGraphics, IDisposable
    {
        public GLWindow? Window { get; set; }

        private IGL20? _gl20;
        private IGL30? _gl30;
        
        public GLGraphics( GLWindow window )
        {
            this.Window = window;

            if ( window.GetConfig().useGL30 )
            {
                this._gl30 = new GL30();
                this._gl20 = this._gl30;
            }
            else
            {
                this._gl20 = new GL20();
                this._gl30 = null;
            }

            UpdateFramebufferInfo();
            InitiateGL();
            
            GLFW.glfwSetFramebufferSizeCallback( window.getWindowHandle(), resizeCallback );
        }

        public class Monitor
        {
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                this._gl20 = null;
                this._gl30 = null;
            }
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }
}
