using System;
using System.IO;

namespace WkHtmlToPdf.Wrapper
{
    internal class FileConversionResult : ConversionResult
    {
        private Lazy<byte[]> _lazyFileBytes;
        private byte[] _fileBytes;
        private FileInfo _fileInfo;

        public override long TotalBytes => GetFileSize();
        public override bool Success => _fileInfo != default && _fileInfo.Exists;

        public FileConversionResult()
        {
            _lazyFileBytes = new Lazy<byte[]>(ReadBytes);
        }

        public override byte[] GetBytes()
        {
            if(_lazyFileBytes.IsValueCreated)
            {
                return _lazyFileBytes.Value;
            }

            if (!Success)
            {
                throw new FileNotFoundException("Could not find converted file", _fileInfo.FullName);
            }

            if (_fileBytes == default)
            {
                _fileBytes = File.ReadAllBytes(_fileInfo.FullName);
            }
            return _fileBytes;
        }

        public override Stream GetStream()
        {
            if (!Success)
            {
                throw new FileNotFoundException("Could not find converted file", _fileInfo.FullName);
            }

            return File.OpenRead(_fileInfo.FullName);
        }

        internal override void SetResult(object result)
        {
            base.SetResult(result);
            if (result is string filePath)
            {
                _fileInfo = new FileInfo(filePath);
            }
        }

        private byte[] ReadBytes()
        {
            if(_fileInfo == null)
            {
                return Array.Empty<byte>();
            }

            return File.ReadAllBytes(_fileInfo.FullName);
        }

        private long GetFileSize()
        {
            if (_fileBytes != default)
            {
                return _fileBytes.Length;
            }

            return Success ? _fileInfo.Length : -1;
        }
    }
}
