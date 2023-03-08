namespace LibGDXSharp
{
    /// <summary>
    /// Convenience implementation of <see cref="IScreen"/>.
    /// Derive from this and only override what you need.
    /// </summary>
    public class ScreenAdapter : IScreen
    {
        public virtual void Show()
        {
        }

        public virtual void Render( float delta )
        {
        }

        public virtual void Resize( int width, int height )
        {
        }

        public virtual void Pause()
        {
        }

        public virtual void Resume()
        {
        }

        public virtual void Hide()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}