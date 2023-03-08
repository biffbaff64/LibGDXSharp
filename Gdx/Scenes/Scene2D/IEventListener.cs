namespace LibGDXSharp.Scenes.Scene2D
{
    /// <summary>
    /// Low level interface for receiving events.
    /// Typically there is a listener class for each specific event class.
    /// </summary>
    /// <seealso cref="InputListener"/>
    /// <seealso cref="InputEvent"/>
    public interface IEventListener
    {
        /// <summary>
        /// Try to handle the given event, if it is applicable.
        /// </summary>
        /// <returns>
        /// True if the event should be considered as handled by scene2d.
        /// </returns>
        public bool Handle( Event e );
    }
}
