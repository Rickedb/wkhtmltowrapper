using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.Options;
using WkHtmlToPdf.Wrapper.Tests.Fixtures;

namespace WkHtmlToPdf.Wrapper.Tests
{
    public class PdfOptionsTests
    {
        private readonly WkHtmlToPdfTestFixture _fixture;

        public PdfOptionsTests()
        {
            _fixture = new WkHtmlToPdfTestFixture();
        }

        [Fact(DisplayName = "Header and footer options")]
        public void TestHeaderAndFooterOptions()
        {
            var filename = $"{nameof(TestHeaderAndFooterOptions)}.pdf";
            var opt = _fixture.UseHtmlOptions();
            opt.Title = "My custom title";
            opt.Header = new HeaderOptions()
            {
                Html = "./Html/CustomHeader.html",
                CenterText = "My document header",
                LeftText = "Left text",
                RightText = "Right text",
                DisplaySeparatorLine = true,
                FontSize = 14,
                Spacing = 2
            };
            opt.Footer = new FooterOptions()
            {
                Html = "./Html/CustomFooter.html",
                CenterText = "My document footer",
                LeftText = "Left text",
                RightText = "Right text",
                DisplaySeparatorLine = true,
                FontSize = 14,
                Spacing = 2
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "Proxy options failure")]
        public void TestProxyUnaccessibleOptions()
        {
            Assert.ThrowsAsync<WkHtmlToException>(async () =>
            {
                var opt = _fixture.UseFileOrUrlOptions();
                opt.Proxy = new ProxyOptions()
                {
                    Url = "127.0.0.1:8080"
                };
                var task = await _fixture.Wrapper.ConvertAsync(opt);
            }).GetAwaiter().GetResult();
        }

        [Fact(DisplayName = "Page options")]
        public void TestPageOptions()
        {
            var cacheDir = $"{_fixture.CurrentAssemblyLocation}/{Guid.NewGuid()}";
            Directory.CreateDirectory(cacheDir);

            var filename = $"{nameof(TestPageOptions)}.pdf";
            var opt = _fixture.UseFileOrUrlOptions();
            opt.Page = new PageOptions()
            {
                CacheDirectory = cacheDir,
                PrintMediaType = true,
                LocalFileAccess = true
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);

            Assert.True(Directory.EnumerateFileSystemEntries(cacheDir).Count() > 0, "No cache files were created!");
            Directory.Delete(cacheDir, true);
        }

        [Fact(DisplayName = "Cookie options")]
        public void TestCookieOptions()
        {
            var opt = _fixture.UseFileOrUrlOptions();
            opt.Cookies = new CookiesOptions()
            {
                Cookies = new Dictionary<string, string>()
                    {
                        {  "cookie 1", "value 1" },
                        {  "cookie 2", "value 2" },
                    }
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "Styling options")]
        public void TestStylingOptions()
        {
            var opt = _fixture.UseHtmlOptions();
            opt.Styling = new StylingOptions()
            {
                Background = false,
                PageHeight = 500,
                PageWidth = 300,
                ZoomFactor = 2
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "Outline options")]
        public void TestOutlineOptions()
        {
            var opt = _fixture.UseHtmlOptions();
            opt.Outline = new OutlineOptions()
            {
                DumpOutlinePath = $"./Dump.xml"
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "Http options")]
        public void TestHttpOptions()
        {
            var opt = _fixture.UseFileOrUrlOptions();
            opt.Http = new HttpOptions()
            {
                CustomHeaderPropagation = true,
                CustomHeaders = new Dictionary<string, string>()
                    {
                        { "x-api-key", "api key" },
                        { "Accept", "application / json" }
                    },
                HttpAuthenticationUsername = "user 1",
                HttpAuthenticationPassword = "password 1"
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "Javascript options")]
        public void TestJavascriptOptions()
        {
            var opt = _fixture.UseHtmlOptions();
            opt.LogLevel = PromptLogLevel.Info;
            opt.Javascript = new JavascriptOptions()
            {
                Debug = true,
                Scripts = "console.log('this is a message');"
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
            //TODO: assert incoming message
        }

        [Fact(DisplayName = "Table of contents options")]
        public void TestTableOfContentsOptions()
        {
            var opt = _fixture.UseFileOrUrlOptions();
            opt.TableOfContents = new TableOfContentsOptions()
            {
                HeaderText = "Hello world"
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
        }

        [Fact(DisplayName = "All options")]
        public void TestAllTogetherOptions()
        {
            var html = File.ReadAllText("./Html/SimpleHtml.html");
            var cacheDir = $"{_fixture.CurrentAssemblyLocation}/{Guid.NewGuid()}";
            Directory.CreateDirectory(cacheDir);

            var opt = new HtmlOptions(html)
            {
                Title = "My custom title",
                Header = new HeaderOptions()
                {
                    Html = $"./Html/CustomHeader.html",
                    CenterText = "My document header",
                    LeftText = "Left text",
                    RightText = "Right text",
                    DisplaySeparatorLine = true,
                    FontSize = 14,
                    Spacing = 2
                },
                Footer = new FooterOptions()
                {
                    Html = $"./Html/CustomFooter.html",
                    CenterText = "My document footer",
                    LeftText = "Left text",
                    RightText = "Right text",
                    DisplaySeparatorLine = true,
                    FontSize = 14,
                    Spacing = 2
                },
                Page = new PageOptions()
                {
                    CacheDirectory = cacheDir,
                    PrintMediaType = true,
                    LocalFileAccess = true
                },
                Styling = new StylingOptions()
                {
                    Background = false,
                    PageHeight = 500,
                    PageWidth = 300,
                    ZoomFactor = 2
                }
            };

            var task = _fixture.Wrapper.ConvertAsync(opt);

            var result = task.GetAwaiter().GetResult();
            _fixture.AssertResult(result);
            Assert.True(Directory.EnumerateFileSystemEntries(cacheDir).Count() > 0, "No cache files were created!");
            Directory.Delete(cacheDir, true);
        }
    }
}
