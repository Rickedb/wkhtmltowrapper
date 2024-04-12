using System.Diagnostics;
using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void GenerateFromRawHtml()
        {
            var html = File.ReadAllText("./Html/SimpleHtml.html");
            var driver = new WkHtmlToPdfWrapper("./Executables");
            driver.OutputEvent += (obj, str) => Debug.WriteLine($"[{str.EventType}]: {str.Message}");

            var opt = new HtmlOptions(html);
            var task = driver.GenerateAsync(opt);
            
            Task.WaitAll(task);
            File.WriteAllBytes("raw-html.pdf", task.Result.GetBytes());
        }

        [Fact]
        public void GenerateFromUrl()
        {
            var html = File.ReadAllText("./Html/SimpleHtml.html");
            var driver = new WkHtmlToPdfWrapper("./Executables");
            driver.OutputEvent += (obj, str) => Debug.WriteLine($"[{str.EventType}]: {str.Message}");

            var opt = new FileOrUrlOptions("https://en.wikipedia.org/");
            var task = driver.GenerateAsync(opt);

            Task.WaitAll(task);
            File.WriteAllBytes("url.pdf", task.Result.GetBytes());
        }
    }
}