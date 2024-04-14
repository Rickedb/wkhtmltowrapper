using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WkHtmlToPdf.Wrapper.AspNetCore
{
    public class PdfFileStreamResult : FileStreamResult
    {
        public PdfFileStreamResult() : base(Stream.Null, "application/pdf")
        {
        }
    }
}
