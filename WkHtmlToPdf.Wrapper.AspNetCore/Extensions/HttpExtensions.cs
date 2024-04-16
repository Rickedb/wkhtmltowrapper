using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Extensions
{
    internal static class HttpExtensions
    {
        public static HttpResponse Prepare(this HttpResponse response, ContentDisposition contentDisposition, string filename)
        {
            response.ContentType = "application/pdf";
            if (!string.IsNullOrEmpty(filename))
            {
                filename = SanitizeFileName(filename);
                var value = string.Format("{0}; filename=\"{1}\"", contentDisposition.ToString().ToLower(), filename);
                response.Headers.Append("Content-Disposition", value);
            }

            return response;
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
