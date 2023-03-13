using LibGDXSharp.Audio;

namespace LibGDXSharp.Core
{
    public interface IAudio
    {
        /// <summary>
        /// Creates a new <see cref="IAudioDevice"/> either in mono or stereo mode.
        /// The AudioDevice has to be disposed via its Dispose() method when it is
        /// no longer used.
        /// </summary>
        /// <param name="samplingRate">The sampling rate.</param>
        /// <param name="isMono">Whether the AudioDevice should be in mono or stereo mode.</param>
        /// <returns>The AudioDevice.</returns>
        /// <exception cref="GdxRuntimeException">Thrown if the device could not be created.</exception>
        IAudioDevice NewAudioDevice( int samplingRate, bool isMono );

        ///
        /// Creates a new <see cref="IAudioRecorder"/>. The AudioRecorder has to be disposed after it is no longer used.
	    /// 
	    /// @param samplingRate the sampling rate in Hertz
	    /// @param isMono whether the recorder records in mono or stereo
	    /// @return the AudioRecorder
	    /// 
	    /// @throws GdxRuntimeException in case the recorder could not be created
        ///
        IAudioRecorder NewAudioRecorder( int samplingRate, bool isMono );

        /// <summary>
	    /// Creates a new <see cref="ISound"/> which is used to play back audio effects such as gun shots or explosions. The Sound's audio data
	    /// is retrieved from the file specified via the {@link FileHandle}. Note that the complete audio data is loaded into RAM. You
	    /// should therefore not load big audio files with this methods. The current upper limit for decoded audio is 1 MB.
        ///
        /// Currently supported formats are WAV, MP3 and OGG.
	    /// 
	    /// The Sound has to be disposed if it is no longer used via the Sound#dispose() method.
	    /// </summary>
	    /// <returns>The new sound.</returns>
	    /// <exception cref="GdxRuntimeException">in case the sound could not be loaded.</exception>
        ISound NewSound( FileHandle fileHandle );

        /// <summary>
        /// Creates a new <see cref="IMusic"/> instance which is used to play back a music stream from a file. Currently supported formats are
	    /// WAV, MP3 and OGG. The Music instance has to be disposed if it is no longer used via the {@link Music#dispose()} method.
	    /// Music instances are automatically paused when {@link ApplicationListener#pause()} is called and resumed when
	    /// {@link ApplicationListener#resume()} is called.
	    /// </summary>
	    /// <param name="file">The filehandle.</param>
	    /// <return>the new Music or null if the Music could not be loaded.</return>
        /// <exception cref="GdxRuntimeException">in case the music could not be loaded.</exception>
        IMusic NewMusic( FileHandle file );
    }
}
