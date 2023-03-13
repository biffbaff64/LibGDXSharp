namespace LibGDXSharp.Core
{
    /// <summary>
    /// An ILifecycleListener can be added to an Application via
    /// Application.AddLifecycleListener(ILifecycleListener). It will receive
    /// notification of Pause, Resume and Dispose events. This is mainly meant
    /// to be used by extensions that need to manage resources based on the life-cycle.
    /// Normal, application level development should rely on the ApplicationListener
    /// interface. The methods will be invoked on the rendering thread. The methods
    /// will be executed before the ApplicationListener methods are executed.
    /// </summary>
    public interface ILifecycleListener
    {
        /// <summary>
        /// Called when the <see cref="Application"/> is about to be paused.
        /// </summary>
        public void Pause();

        /// <summary>
        /// Called when the <see cref="Application"/> is about to be resumed.
        /// </summary>
        public void Resume();

        /// <summary>
        /// Called when the <see cref="Application"/> is about to be disposed.
        /// </summary>
        public void Dispose();
    }
}