namespace LibGDXSharp
{
    /// <summary>
    /// An ApplicationListener that delegates to a Screen. This allows an application
    /// to easily have multiple screens. Screens are not disposed automatically. You
    /// must handle whether you want to keep screens around or dispose of them when
    /// another screen is set.
    /// </summary>
    public abstract class Game : IApplicationListener
    {
        private IScreen? _screen;
        
        /// <summary>
        /// Sets the current screen. Screen.hide() is called on any old screen,
        /// and Screen.show() is called on the new screen, if any.
        /// </summary>
        public IScreen? Screen
        {
            get => _screen;
            set
            {
                _screen?.Hide();

                _screen = value;
                
                if (_screen != null)
                {
                    _screen.Show();

                    if ( Gdx.Graphics != null )
                    {
                        _screen.Resize( Gdx.Graphics.GetWidth(), Gdx.Graphics.GetHeight() );
                    }
                }

            }
        }

        public virtual void Create()
        {
        }

        /// <summary>
        /// Render the currently active screen.
        /// </summary>
        public virtual void Render()
        {
            if ( Gdx.Graphics != null )
            {
                Screen?.Render( Gdx.Graphics.GetDeltaTime() );
            }
        }

        public virtual void Resize( int width, int height )
        {
            Screen?.Resize( width, height );
        }

        public virtual void Pause()
        {
            Screen?.Pause();
        }

        public virtual void Resume()
        {
            Screen?.Resume();
        }

        public virtual void Dispose()
        {
            Screen?.Hide();
        }
    }
}