namespace LibGDXSharp.Backends.Desktop
{
    public interface IGLWindowListener
    {
        /// <summary>
		/// Called after the GLFW window is created. Before this callback is received, it's
		/// unsafe to use any<see cref="GLWindow"/> member functions which, for their part,
		/// involve calling GLFW functions.
        /// </summary>
		///
		/// For the main window, this is an immediate callback from inside
		/// <see cref="DesktopApplication(IApplicationListener, GLApplicationConfiguration)"/>
		/// <param name="window">the window instance.</param>
		/// <seealso cref="GLApplication"/>
        void Created( GLWindow window );

        /// <summary>
		/// Called when the window is iconified (i.e. its minimize button
		/// was clicked), or when restored from the iconified state. When a window becomes
		/// iconified, its {@link ApplicationListener} will be paused, and when restored
		/// it will be resumed.
        /// </summary>
		/// 
		/// @param isIconified True if window is iconified, false if it leaves the iconified state
        void Iconified( bool isIconified );

        /// <summary>
		/// Called when the window is maximized, or restored from the maximized state.
        /// </summary>
		///
		/// @param isMaximized true if window is maximized, false if it leaves the maximized state
        void Maximized( bool isMaximized );

        /// <summary>
		/// Called when the window lost focus to another window. The
		/// window's {@link ApplicationListener} will continue to be
		/// called.
        /// </summary>
        void FocusLost();

        /// <summary>
		/// Called when the window gained focus.
        /// </summary>
        void FocusGained();

        /// <summary>
        /// Called when the user requested to close the window, e.g. clicking
		/// the close button or pressing the window closing keyboard shortcut.
        /// </summary>
		/// @return whether the window should actually close
        bool CloseRequested();

        /// <summary>
		/// Called when external files are dropped into the window,
		/// e.g from the Desktop.
        /// </summary>
		/// @param files array with absolute paths to the files
        void FilesDropped( string[] files );

        /// <summary>
		/// Called when the window content is damaged and needs to be refreshed.
		/// When this occurs, {@link Lwjgl3Graphics#requestRendering()} is automatically called.
        /// </summary>
        void RefreshRequested();
    }
}
