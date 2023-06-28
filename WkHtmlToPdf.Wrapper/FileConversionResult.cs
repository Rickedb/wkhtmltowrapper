using System.IO;

namespace WkHtmlToPdf.Wrapper
{
    internal class FileConversionResult : ConversionResult
    {
        private byte[] _fileBytes;
        private FileInfo _fileInfo;

        public override long TotalBytes => GetFileSize();
        public override bool Success => _fileInfo != default && _fileInfo.Exists;

        public override byte[] GetBytes()
        {
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

        internal void SetResult(string filePath)
        {
            _fileInfo = new FileInfo(filePath);
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
