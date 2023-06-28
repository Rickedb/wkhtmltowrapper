using System.Diagnostics;
using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var options = new PdfOptions();
            var html = File.ReadAllText("./Html/SimpleHtml.html");
            var driver = new WkHtmlToPdfDriver("./Executables");
            driver.OutputEvent += (obj, str) => Debug.WriteLine($"[{str.EventType}]: {str.Message}");

            var t1= driver.ConvertHtmlAsync(options, html);
            //var t2 = driver.ConvertUriAsync(options, "https://onepieceex.net/episodio-1065/");

            Task.WaitAll(t1);
            File.WriteAllBytes("C:\\Users\\Henrique\\Documents\\Temp\\url.pdf", t1.Result.GetBytes());
            //File.WriteAllBytes("C:\\Users\\Henrique\\Documents\\Temp\\html.pdf", t2.Result.GetBytes());
        }
    }
}