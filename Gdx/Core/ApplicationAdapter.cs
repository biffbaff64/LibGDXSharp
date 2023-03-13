namespace LibGDXSharp.Core
{
    /// <summary>
    /// Convenience implementation of <see cref="IApplicationListener"/>.
    /// Derive from this and only override what you need.
    /// </summary>
    public class ApplicationAdapter : IApplicationListener
    {
        public virtual void Create()
        {
        }

        public virtual void Render()
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

        public virtual void Dispose()
        {
        }
    }
}

