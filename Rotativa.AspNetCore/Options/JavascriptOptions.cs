using System.Collections.Generic;

namespace Rotativa.AspNetCore.Options
{
    public class JavascriptOptions
    {
        /// <summary>
        /// Do allow web pages to run javascript
        /// <para><em>AKA: <c>--enable-javascript</c> and <c>--disable-javascript</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-javascript", "--disable-javascript")]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Show javascript debugging output
        /// <para><em>AKA: <c>--debug-javascript</c> and <c>--no-debug-javascript</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--debug-javascript", "--no-debug-javascript")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Stop slow running javascripts
        /// <para><em>AKA: <c>--stop-slow-scripts</c> and <c>--no-stop-slow-scripts</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--stop-slow-scripts", "--no-stop-slow-scripts")]
        public bool StopSlowScript { get; set; } = true;

        /// <summary>
        /// Wait some milliseconds for javascript finish
        /// <para><em>AKA: <c>--javascript-delay</c></em></para>
        /// </summary>
        [OptionFlag("--javascript-delay")]
        public int FinishDelay { get; set; } = 200;

        /// <summary>
        /// Run this additional javascript after the page is done loading
        /// <para><em>AKA: <c>--run-script</c></em></para>
        /// </summary>
        [OptionFlag("--run-script")]
        public List<string> Scripts { get; set; }
    }
}
