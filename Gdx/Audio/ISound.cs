namespace LibGDXSharp.Audio
{
    public interface ISound : IDisposable
    {
        /// <summary>
        /// Plays the sound. If the sound is already playing, it will be played again, concurrently.
        /// </summary>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Play();

        /// <summary>
        /// Plays the sound. If the sound is already playing, it will be played again, concurrently.
        /// </summary>
        /// <param name="volume"> the volume in the range [0,1] </param>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Play( float volume );

        /// <summary>
        /// Plays the sound. If the sound is already playing, it will be played again, concurrently.
        /// </summary>
        /// <param name="volume"> the volume in the range [0,1] </param>
        /// <param name="pitch">
        /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
        /// the value has to be between '0.5' and '2.0'
        /// </param>
        /// <param name="pan">
        /// panning in the range -1 (full left) to 1 (full right). 0 is center position.
        /// </param>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Play( float volume, float pitch, float pan );

        /// <summary>
        /// Plays the sound, looping. If the sound is already playing,
        /// it will be played again, concurrently.
        /// </summary>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Loop();

        /// <summary>
        /// Plays the sound, looping. If the sound is already playing, it will be played
        /// again, concurrently. You need to stop the sound via a call to <seealso cref="Stop(long)"/>
        /// using the returned id.
        /// </summary>
        /// <param name="volume"> the volume in the range [0, 1] </param>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Loop( float volume );

        /// <summary>
        /// Plays the sound, looping. If the sound is already playing, it will be played again,
        /// concurrently. You need to stop the sound via a call to <seealso cref="Stop(long)"/>
        /// using the returned id.
        /// </summary>
        /// <param name="volume"> the volume in the range [0,1] </param>
        /// <param name="pitch">
        /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
        /// the value has to be between 0.5 and 2.0
        /// </param>
        /// <param name="pan"> panning in the range -1 (full left) to 1 (full right). 0 is center position. </param>
        /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
        long Loop( float volume, float pitch, float pan );

        /// <summary>
        /// Stops playing all instances of this sound.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses all instances of this sound.</summary>
        void Pause();

        /// <summary>
        /// Resumes all paused instances of this sound.
        /// </summary>
        void Resume();

        /// <summary>
        /// Stops the sound instance with the given id as returned by <seealso cref="Play()"/> or
        /// <seealso cref="Play(float)"/>. If the sound is no longer playing, this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id  </param>
        void Stop( long soundId );

        /// <summary>
        /// Pauses the sound instance with the given id as returned by <seealso cref="play()"/>
        /// or <seealso cref="Play(float)"/>. If the sound is no longer playing, this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id  </param>
        void Pause( long soundId );

        /// <summary>
        /// Resumes the sound instance with the given id as returned by <seealso cref="Play()"/>
        /// or <seealso cref="Play(float)"/>. If the sound is not paused, this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id  </param>
        void Resume( long soundId );

        /// <summary>
        /// Sets the sound instance with the given id to be looping. If the sound is no longer
        /// playing this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id </param>
        /// <param name="looping"> whether to loop or not.  </param>
        void SetLooping( long soundId, bool looping );

        /// <summary>
        /// Changes the pitch multiplier of the sound instance with the given id as returned
        /// by <seealso cref="play()"/> or <seealso cref="play(float)"/>.
        /// If the sound is no longer playing, this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id </param>
        /// <param name="pitch">
        /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
        /// the value has to be between 0.5 and 2.0
        /// </param>
        void SetPitch( long soundId, float pitch );

        /// <summary>
        /// Changes the volume of the sound instance with the given id as returned by
        /// <seealso cref="Play()"/> or <seealso cref="Play(float)"/>. If the
        /// sound is no longer playing, this has no effect.
        /// </summary>
        /// <param name="soundId"> the sound id </param>
        /// <param name="volume"> the volume in the range 0 (silent) to 1 (max volume).  </param>
        void SetVolume( long soundId, float volume );

        /// <summary>
        /// Sets the panning and volume of the sound instance with the given id as returned
        /// by <seealso cref="Play()"/> or <seealso cref="Play(float)"/>.
        /// If the sound is no longer playing, this has no effect. Note that panning only works
        /// for mono sounds, not for stereo sounds!
        /// </summary>
        /// <param name="soundId"> the sound id </param>
        /// <param name="pan"> panning in the range -1 (full left) to 1 (full right). 0 is center position. </param>
        /// <param name="volume"> the volume in the range [0,1].  </param>
        void SetPan( long soundId, float pan, float volume );
    }
}
