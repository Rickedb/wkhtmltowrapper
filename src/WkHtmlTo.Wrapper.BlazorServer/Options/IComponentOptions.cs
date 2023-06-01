using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper.BlazorServer.Options
{
    public interface IComponentOptions : IHtmlOptions
    {
        string Filename { get; set; }
    }
}
