namespace LibGDXSharp.Backends.Desktop
{
    /// <summary>
    /// Convenience implementation of <see cref="IGLWindowListener"/>.
    /// Derive from this class and only overwrite the methods you are interested in.
    /// </summary>
    public class GLWindowAdapter : IGLWindowListener
    {
        public void Created( GLWindow window )
        {
        }

        public void Iconified( bool isIconified )
        {
        }

        public void Maximized( bool isMaximized )
        {
        }

        public void FocusLost()
        {
        }

        public void FocusGained()
        {
        }

        public bool CloseRequested()
        {
            return false;
        }

        public void FilesDropped( string[] files )
        {
        }

        public void RefreshRequested()
        {
        }
    }
}

