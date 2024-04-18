using System.Threading;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    internal static class ProcessExtensions
    {
        internal static async Task CompatibleWaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD
            var waitForExit = Task.Run(() => process.WaitForExit());
            if (cancellationToken != null)
            {
                var timeout = Task.Delay(Timeout.Infinite, cancellationToken);
                await Task.WhenAny(waitForExit, timeout);
            }
            else
            {
                await waitForExit;
            }
#else
            await process.WaitForExitAsync(cancellationToken);
#endif
        }
    }
}
