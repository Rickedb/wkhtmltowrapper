using System.Reflection;

namespace System.IO
{
    internal class PathInfo
    {
        private static readonly Lazy<string> _lazyAssemblyPath = new Lazy<string>(GetCurrentAssemblyPath);

        internal static bool IsPathFullyQualified(string path)
        {
#if NET5_0_OR_GREATER
            return Path.IsPathFullyQualified(path);
#else
            var root = Path.GetPathRoot(path);
            return root.StartsWith(@"\\") || root.EndsWith(@"\") && root != @"\";
#endif
        }
        internal static string GetAbsolutePath(string absoluteOrRelativePath)
        {
            if (IsPathFullyQualified(absoluteOrRelativePath))
            {
                return absoluteOrRelativePath;
            }

            return Path.Combine(_lazyAssemblyPath.Value, absoluteOrRelativePath);
        }

        private static string GetCurrentAssemblyPath()
            => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
