namespace System.IO
{
    internal class PathInfo
    {
        public static bool IsPathFullyQualified(string path)
        {
#if NET5_0_OR_GREATER
            return Path.IsPathFullyQualified(path);
#else
            var root = Path.GetPathRoot(path);
            return root.StartsWith(@"\\") || root.EndsWith(@"\") && root != @"\";
#endif
        }
    }
}
