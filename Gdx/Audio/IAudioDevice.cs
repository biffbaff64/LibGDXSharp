namespace LibGDXSharp.Audio
{
    /// <summary>
    /// Encapsulates an audio device in mono or stereo mode. Use the
    /// WriteSamples(float[], int, int) and WriteSamples(short[], int, int)
    /// methods to write float or 16-bit signed short PCM data directly to
    /// the audio device. Stereo samples are interleaved in the order left
    /// channel sample, right channel sample. The dispose() method must be
    /// called when this AudioDevice is no longer needed.
    /// </summary>
    public interface IAudioDevice
    {
        /// <summary>
        /// Returns whether this AudioDevice is in mono or stereo mode.
        /// </summary>
        /// <returns>TRUE for mono, FALSE for stereo.</returns>
        bool IsMono();

        /// <summary>
        /// Writes the array of 16-bit signed PCM samples to the audio
        /// device and blocks until they have been processed.
        /// </summary>
        /// <param name="samples">The samples.</param>
        /// <param name="offset">The offset into the samples array.</param>
        /// <param name="numSamples">The number of samples to write to the device.</param>
        void WriteSamples(int[] samples, int offset, int numSamples);

        /// <summary>
        /// Writes the array of float PCM samples to the audio device and
        /// blocks until they have been processed.
        /// </summary>
        /// <param name="samples">The samples.</param>
        /// <param name="offset">The offset into the samples array.</param>
        /// <param name="numSamples">The number of samples to write to the device.</param>
        void WriteSample(float[] samples, int offset, int numSamples);

        /// <summary>
        /// </summary>
        /// <returns>The latency in samples.</returns>
        int GetLatency();

        /// <summary>
        /// Sets the volume in the range [0,1].
        /// </summary>
        void SetVolume(float volume);

        /// <summary>
        /// Frees all resources associated with this AudioDevice.
        /// Needs to be called when the device is no longer needed.
        /// </summary>
        void Dispose();
    }
}

