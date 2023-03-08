namespace LibGDXSharp.Utils
{
    public interface IPoolable
    {
        /// <summary>
        /// Resets the object for reuse. Object references should
        /// be nulled and fields may be set to default values.
        /// </summary>
        void Reset();
    }
}
