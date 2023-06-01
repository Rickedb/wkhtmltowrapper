using System.Threading;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    internal static class ProcessExtensions
    {
        internal static async Task CompatibleWaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
#if !NET5_0_OR_GREATER
            var waitForExit = Task.Factory.StartNew(() => process.WaitForExit(), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            if (cancellationToken != default)
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
