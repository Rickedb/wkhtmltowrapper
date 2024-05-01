using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.Options;
using WkHtmlToPdf.Wrapper.Tests.Fixtures;

namespace WkHtmlToPdf.Wrapper.Tests
{
    public class ConversionTests 
    {
        private readonly WkHtmlToPdfTestFixture _fixture;

        public ConversionTests()
        {
            _fixture = new WkHtmlToPdfTestFixture();
        }

        [Fact(DisplayName = "Bytes from html")]
        public void GenerateBytesFromHtml()
        {
            var opt = _fixture.UseHtmlOptions();
            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "File from html")]
        public void GenerateFileFromHtml()
        {
            var filename = $"{nameof(GenerateFileFromHtml)}.pdf";
            var opt = _fixture.UseHtmlOptions();
            opt.OutputPath = filename;
            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
            Assert.True(File.Exists(filename));
            File.Delete(filename);
        }

        [Fact(DisplayName = "Bytes from url")]
        public void GenerateBytesFromUrl()
        {
            var opt = _fixture.UseFileOrUrlOptions(); 
            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "File from url")]
        public void GenerateFileFromUrl()
        {
            var filename = $"{nameof(GenerateFileFromUrl)}.pdf";
            var opt = _fixture.UseFileOrUrlOptions();
            opt.OutputPath = filename;
            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
            Assert.True(File.Exists(filename));
            File.Delete(filename);
        }

        [Fact(DisplayName = "Throwing exception on failures")]
        public void ThrowProcessExceptionFailure()
        {
            Assert.ThrowsAsync<WkHtmlToException>(async () =>
            {
                var opt = new FileOrUrlOptions("https://en.notexists.orgfail/");
                var task = await _fixture.Wrapper.ConvertAsync(opt);
            }).GetAwaiter().GetResult();
        }
    }
}