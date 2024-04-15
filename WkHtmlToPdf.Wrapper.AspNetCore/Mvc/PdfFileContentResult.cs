using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Mvc
{
    internal class PdfFileContentResult : FileContentResult
    {
        public RazorPdfViewOptions Options { get; set; } = new RazorPdfViewOptions();

        public PdfFileContentResult() : base(null, "application/pdf")
        {

        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            
            return base.ExecuteResultAsync(context);
        }
    }
}
