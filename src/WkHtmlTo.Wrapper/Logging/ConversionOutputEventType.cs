namespace WkHtmlTo.Wrapper.Logging
{
    /// <summary>
    /// The categorized type of event that can be outputed from stdout console
    /// </summary>
    public enum ConversionOutputEventType
    {
        Information = 0,
        Error = 1,
        OverallProgress = 2,
        OverallStep = 3,
        InnerStep = 4,
        InnerStepProgress = 5
    }
}
