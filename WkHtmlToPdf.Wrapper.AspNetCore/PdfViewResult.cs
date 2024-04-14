using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WkHtmlToPdf.Wrapper.AspNetCore
{
    public class PdfViewResult : ViewResult
    {
        public new object Model { get; set; }
        public RazorPdfOptions Options { get; set; } = new RazorPdfOptions();

        public PdfViewResult(ViewDataDictionary viewData = null)
        {
            ViewName = string.Empty;
            Model = null;
            ViewData = viewData;
        }

        public PdfViewResult(string viewName, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
        }

        public PdfViewResult(object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            Model = model;
        }

        public PdfViewResult(string viewName, object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
            Model = model;
        }

        public PdfViewResult(string viewName, object model)
            : this(model)
        {
            
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var fileContent = await BuildFileBytesAsync(context);
            var response = PrepareResponse(context.HttpContext.Response);
            await response.Body.WriteAsync(fileContent);
        }

        public async Task<byte[]> BuildFileBytesAsync(ActionContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var result = await GeneratePdfAsync(context);
            if(!result.Success)
            {
                throw new InvalidOperationException();
            }
            return result.GetBytes();
        }

        public async Task<ConversionResult> SaveAsync(ActionContext context, string folderPath)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            //ArgumentException.ThrowIfNullOrWhiteSpace(folderPath, nameof(folderPath));

            var options = await Options.ToHtmlOptionsAsync(this, context);
            return await new WkHtmlToPdfWrapper().GenerateAsync(options);
        }

        public async Task<Stream> BuildFileStreamAsync(ActionContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var fileContent = await GeneratePdfAsync(context);
            return fileContent.GetStream();
        }

        private HttpResponse PrepareResponse(HttpResponse response)
        {
            response.ContentType = "application/pdf";
            var filename = Options.Filename;
            if (!string.IsNullOrEmpty(filename))
            {
                var contentDisposition = Options.ContentDisposition == ContentDisposition.Attachment ? "attachment" : "inline";
                filename = SanitizeFileName(filename);
                response.Headers.Append("Content-Disposition", string.Format("{0}; filename=\"{1}\"", contentDisposition, filename));
            }

            return response;
        }

        protected async Task<ConversionResult> GeneratePdfAsync(ActionContext context)
        {
            var options = await Options.ToHtmlOptionsAsync(this, context);
            return await new WkHtmlToPdfWrapper().GenerateAsync(options);
        }

        private static string SanitizeFileName(string name)
        {
            var invalidChars = string.Concat(new string(Path.GetInvalidPathChars()), new string(Path.GetInvalidFileNameChars()));
            var invalidCharsPattern = string.Format(@"[{0}]+", Regex.Escape(invalidChars));
            var result = Regex.Replace(name, invalidCharsPattern, "_");
            return result;
        }
    }
}
