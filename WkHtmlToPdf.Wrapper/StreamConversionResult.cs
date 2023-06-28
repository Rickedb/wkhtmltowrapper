﻿using System;
using System.IO;

namespace WkHtmlToPdf.Wrapper
{
    internal class StreamConversionResult : ConversionResult
    {
        private MemoryStream Output;

        public override long TotalBytes => Success ? Output.Length : -1;
        public override bool Success => Output != null && Output.Length > 0;

        public override byte[] GetBytes()
        {
            if(TotalBytes > -1)
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

        internal void SetResult(MemoryStream output)
        {
            //_stopwatch.Stop();
            Output = output;
        }
    }
}
