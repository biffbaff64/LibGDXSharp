namespace LibGDXSharp.Core
{
    /// <summary>
    /// The ApplicationLogger provides an interface for a LibGDX Application to
    /// log messages and exceptions. A default implementations is provided for
    /// each backend, custom implementations can be provided and set using
    /// IApplication.ApplicationLogger = ApplicationLogger
    /// </summary>
    public interface IApplicationLogger
    {
        /// <summary>
        /// Logs a message with a tag.
        /// </summary>
        void Log (string tag, string message);

        /// <summary>
        /// Logs a message and exception with a tag.
        /// </summary>
        void Log (string tag, string message, Exception exception);

        /// <summary>
        /// Logs an error message with a tag.
        /// </summary>
        void Error (string tag, string message);

        /// <summary>
        /// Logs an error message and exception with a tag.
        /// </summary>
        void Error (string tag, string message, Exception exception);

        /// <summary>
        /// Logs a debug message with a tag.
        /// </summary>
        void Debug (string tag, string message);

        /// <summary>
        /// Logs a debug message and exception with a tag.
        /// </summary>
        void Debug (string tag, string message, Exception exception);
    }
}
