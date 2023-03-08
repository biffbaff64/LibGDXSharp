namespace LibGDXSharp
{
    public interface IApplicationListener
    {
        /// <summary>
        /// Called when the <see cref="Application"/> is first created.
        /// </summary>
        void Create ();

        /// <summary>
        /// Called when the <see cref="Application"/> is resized. This can
        /// happen at any point during a non-paused state but will never happen
        /// before a call to <see cref="Create"/>
        /// </summary>
        /// <param name="width">The new width in pixels.</param>
        /// <param name="height">The new height in pixels.</param>
        void Resize (int width, int height);

        /// <summary>
        /// Called when the <see cref="Application"/> should draw itself.
        /// </summary>
        void Render ();

        /// <summary>
        /// Called when the <see cref="Application"/> is paused, usually when
        /// it's not active or visible on-screen. An Application is also
        /// paused before it is destroyed.
        /// </summary>
        void Pause ();

        /// <summary>
        /// Called when the <see cref="Application"/> is resumed from a paused state,
        /// usually when it regains focus.
        /// </summary>
        void Resume ();

        /// <summary>
        /// Called when the <see cref="Application"/> is destroyed.
        /// Preceded by a call to <see cref="Pause"/>.
        /// </summary>
        void Dispose ();
    }
}
