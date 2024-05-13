using System.Diagnostics;
using System.IO;
using System.Reflection;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper.Tests.Fixtures
{
    public class WkHtmlToPdfTestFixture
    {
        public WkHtmlToPdfWrapper Wrapper { get; }
        public string CurrentAssemblyLocation { get; }

        public WkHtmlToPdfTestFixture()
        {
            Wrapper = new WkHtmlToPdfWrapper("./Executables");
            Wrapper.OutputEvent += (obj, str) => Debug.WriteLine($"[{str.Event.EventType}]: {str.Event.Message}");
            CurrentAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public void AssertResult(ConversionResult result)
        {
            Assert.True(result.Success);
            Assert.True(result.TotalBytes > 0);
        }

        public HtmlOptions UseHtmlOptions()
        {
            var html = File.ReadAllText("./Html/SimpleHtml.html");
            return new HtmlOptions(html);
        }

        public FileOrUrlOptions UseFileOrUrlOptions()
            => new FileOrUrlOptions("https://en.wikipedia.org/");
    }
}
