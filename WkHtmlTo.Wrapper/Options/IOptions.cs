namespace WkHtmlTo.Wrapper.Options
{
    public interface IOptions
    {
        string ToSwitchCommand();
    }

    public interface IPdfOptions : IOptions
    {
        string OutputPath { get; set; }
    }

    public interface IFileOrUrlOptions : IPdfOptions
    {
        string HtmlFilePathOrUrl { get; set; }
    }

    public interface IHtmlOptions : IPdfOptions
    {
        string Html { get; set; }
    }
}
