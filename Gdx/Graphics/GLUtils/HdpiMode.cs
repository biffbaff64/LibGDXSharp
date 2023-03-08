namespace LibGDXSharp.Graphics.GLUtils
{
    public enum HdpiMode
    {
	    /// <summary>
		/// mouse coordinates, Graphics#Width and Graphics#Height will return
		/// logical coordinates according to the system defined HDPI scaling.
		/// Rendering will be performed to a backbuffer at raw resolution.
		/// Use <see cref="HdpiUtils"/> when calling GL20#glScissor or GL20#glViewport
		/// which expect raw coordinates.
	    /// </summary>
        Logical,

        /// <summary>
		/// Mouse coordinates, Graphics#Width and Graphics#Height will return
		/// raw pixel coordinates irrespective of the system defined HDPI scaling.
		/// </summary>
        Pixels
    }
}
