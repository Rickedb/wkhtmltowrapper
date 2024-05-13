namespace WkHtmlTo.Wrapper.Logging
{
    /// <summary>
    /// The categorized type of event that can be outputed from stdout console
    /// </summary>
    public enum ConversionOutputEventType
    {
        /// <summary>
        /// An information output
        /// </summary>
        Information = 0,
        /// <summary>
        /// An output of a error which can or cannot be critical
        /// </summary>
        Error = 1,
        /// <summary>
        /// An output of the current progress of the conversion
        /// </summary>
        OverallProgress = 2,
        /// <summary>
        /// An output of which step the executable is running
        /// </summary>
        OverallStep = 3,
        /// <summary>
        /// An output of the inner step of the current running step
        /// </summary>
        InnerStep = 4,
        /// <summary>
        /// An output of the progress of the current inner step
        /// </summary>
        InnerStepProgress = 5
    }
}
