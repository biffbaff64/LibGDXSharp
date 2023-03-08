namespace LibGDXSharp.Audio
{
    public interface IAudioRecorder
    {
        /// <summary>
        /// Reads in numSamples samples into the array samples starting at offset.
        /// If the recorder is in stereo you have to multiply numSamples by 2.
        /// </summary>
        /// <param name="samples">the array to write the samples to</param>
        /// <param name="offset">the offset into the array</param>
        /// <param name="numSamples">The number of samples to be read.</param>
        void Read(short[] samples, int offset, int numSamples);

        void Dispose();
    }
}

