namespace LibGDXSharp.Core
{
    /// <summary>
    /// Represents one of many application screens, such as a main menu,
    /// a settings menu, the game screen and so on.
    /// Note that Dispose() is not called automatically.
    /// </summary>
    public interface IScreen
    {
        void Show();

        void Render( float delta );

        void Resize( int width, int height );

        void Pause();

        void Resume();

        void Hide();

        void Dispose();
    }
}