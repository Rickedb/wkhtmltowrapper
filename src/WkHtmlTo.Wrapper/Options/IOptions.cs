namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// Represents a wkhtmlto command options
    /// </summary>
    public interface IOptions
    {
        string ToSwitchCommand();
    }

    /// <summary>
    /// Represents a wkhtmltopdf command options
    /// </summary>
    public interface IPdfOptions : IOptions
    {
        string OutputPath { get; set; }
    }

    /// <summary>
    /// Represents a wkhtmltopdf command options with an input of a file or a url
    /// </summary>
    public interface IFileOrUrlOptions : IPdfOptions
    {
        string HtmlFilePathOrUrl { get; set; }
    }

    /// <summary>
    /// Represents a wkhtmltopdf command options with an input of a html plain text
    /// </summary>
    public interface IHtmlOptions : IPdfOptions
    {
        string Html { get; set; }
    }
}
