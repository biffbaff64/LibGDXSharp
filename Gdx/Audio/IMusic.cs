namespace LibGDXSharp.Audio
{
    public interface IMusic
    {
        /// <summary>
        /// Starts the play back of the music stream. In case the stream was paused
        /// this will resume the play back. In case the music stream is finished playing
        /// this will restart the play back. 
        /// </summary>
        public void Play();

        /// <summary>
        /// Pauses the play back. If the music stream has not been started yet or has
        /// finished playing a call to this method will be ignored. 
        /// </summary>
        public void Pause();

        /// <summary>
        /// Stops a playing or paused Music instance. Next time play() is invoked the
        /// Music will start from the beginning.
        /// </summary>
        public void Stop();

        /// <returns> whether this music stream is playing </returns>
        public bool IsPlaying();

        /// <summary>
        /// Sets whether the music stream is looping. This can be called at
        /// any time, whether the stream is playing.
        /// </summary>
        /// <param name="isLooping"> whether to loop the stream</param>
        public void SetLooping( bool isLooping );

        /// <returns> whether the music stream is playing. </returns>
        public bool IsLooping();

        /// <summary>
        /// Sets the volume of this music stream. The volume must be given in the
        /// range [0,1] with 0 being silent and 1 being the maximum volume.
        /// </summary>
        public void SetVolume( float volume );

        /// <returns> the volume of this music stream.</returns>
        public float GetVolume();

        /// <summary>
        /// Sets the panning and volume of this music stream.
        /// </summary>
        /// <param name="pan"> panning in the range -1 (full left) to 1 (full right). 0 is center position.</param>
        /// <param name="volume"> the volume in the range [0,1]. </param>
        public void SetPan( float pan, float volume );

        /// <summary>
        /// Set the playback position in seconds.
        /// </summary>
        public void SetPosition( float position );

        /// <summary>
        /// Returns the playback position in seconds.
        /// </summary>
        public float GetPosition();

        /// <summary>
        /// Needs to be called when the Music is no longer needed.
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Register a callback to be invoked when the end of a music stream has been reached during playback.
        /// </summary>
        /// <param name="listener"> the callback that will be run</param>
        public void setOnCompletionListener( IOnCompletionListener listener );

        /// <summary>
        /// Interface definition for a callback to be invoked when playback of a music stream has completed.
        /// </summary>
        public interface IOnCompletionListener
        {
            /// <summary>
            /// Called when the end of a media source is reached during playback.
            /// </summary>
            /// <param name="music"> the Music that reached the end of the file</param>
            public abstract void OnCompletion( IMusic music );
        }
    }
}
