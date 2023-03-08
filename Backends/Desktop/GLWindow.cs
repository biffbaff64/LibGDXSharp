namespace LibGDXSharp.Backends.Desktop
{
    public class GLWindow : IDisposable
    {
        private          GLApplicationBase          _application;
        private          IGLWindowListener          _windowListener;
        private          IGLInput                    _input;
        private          GLApplicationConfiguration _config;
        private          GLGraphics                 _graphics;
        private          IApplicationListener       _listener;
        private readonly List< IRunnable >          _runnables         = new List< IRunnable >();
        private readonly List< IRunnable >          _executedRunnables = new List< IRunnable >();
        private          IntBuffer                  _tmpBuffer;
        private          IntBuffer                  _tmpBuffer2;
        private          long                       _windowHandle;
        private          bool                       _listenerInitialised = false;
        private          bool                       _iconified           = false;
        private          bool                       _requestRendering    = false;

        /// <summary>
        /// </summary>
        /// <param name="windowHandle"></param>
        public void Create( long windowHandle )
        {
            this._input    = _application.CreateInput();
            this._graphics = new GLGraphics( this );
        }

        public bool Update()
        {
            if ( !_listenerInitialised )
            {
                InitialiseListener();
            }

            lock ( _runnables )
            {
                _executedRunnables.AddAll( _runnables );
                _runnables.Clear();
            }

            foreach ( var runnable in _executedRunnables )
            {
                runnable.Run();
            }

            var shouldRender = ( ( _executedRunnables.Count > 0 ) || ( _graphics.IsContinuousRendering() ) );

            _executedRunnables.Clear();

            if ( !_iconified )
            {
                _input.Update();
            }

            lock ( this )
            {
                shouldRender      |= ( _requestRendering && !_iconified );
                _requestRendering =  false;
            }

            if ( shouldRender )
            {
                _graphics.Update();
                _listener.Render();

                OpenGdx.GLSwapBuffers( _windowHandle );
            }

            if ( !_iconified )
            {
                _input.PrepareNext();
            }

            return shouldRender;
        }

        private void InitialiseListener()
        {
        }

        public void Dispose()
        {
        }
    }
}
