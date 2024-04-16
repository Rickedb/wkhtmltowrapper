using System;
using System.IO;

namespace WkHtmlToPdf.Wrapper.AspNetCore
{
    internal class StreamConversionResult : ConversionResult
    {
        private MemoryStream Output;

        public override long TotalBytes => Success ? Output.Length : -1;
        public override bool Success => Output != null && Output.Length > 0;

        public override byte[] GetBytes()
        {
            if (TotalBytes > -1)
            {
                return Output.ToArray();
            }

            throw new AccessViolationException();
        }

        public override Stream GetStream()
        {
            if (TotalBytes > -1)
            {
                return Output;
            }

            throw new AccessViolationException();
        }

        internal override void SetResult(object result)
        {
            base.SetResult(result);
            if (result is MemoryStream ms)
            {
                ms.Position = 0;
                Output = ms;
            }
        }
    }
}
