using System.Collections.Generic;
using System.Text;
using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// Represents all page options that can be set to wkhtmlto
    /// </summary>
    public class PageOptions : Options, IOptions
    {
        /// <summary>
        /// Allow the file or files from the specified folder to be loaded
        /// <para><em>AKA: <c>--allow</c></em></para>
        /// </summary>
        [PathOptionFlag("--allow")]
        public List<string> Allow { get; set; }

        /// <summary>
        /// Web cache directory
        /// <para><em>AKA: <c>--cache-dir</c></em></para>
        /// </summary>
        [PathOptionFlag("--cache-dir")]
        public string CacheDirectory { get; set; }

        /// <summary>
        /// Set the default text encoding, for input
        /// <para><em>AKA: <c>--encoding</c></em></para>
        /// </summary>
        [OptionFlag("--encoding")]
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Make links to remote web pages
        /// <para><em>AKA: <c>--enable-external-links</c> and <c>--disable-external-links</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-external-links", "--disable-external-links")]
        public bool ExternalLinks { get; set; } = true;

        /// <summary>
        /// Turn HTML form fields into pdf form fields
        /// <para><em>AKA: <c>--enable-forms</c> and <c>--disable-forms</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-forms", "--disable-forms")]
        public bool EnableForms { get; set; }

        /// <summary>
        /// Do load or print images
        /// <para><em>AKA: <c>--images</c> and <c>--no-images</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--images", "--no-images")]
        public bool LoadImages { get; set; } = true;

        /// <summary>
        /// Keep relative external links as relative external links
        /// <para><em>AKA: <c>--keep-relative-links</c></em></para>
        /// </summary>
        [OptionFlag("--keep-relative-links")]
        public bool KeepRelativeLinks { get; set; }

        /// <summary>
        /// Specify how to handle pages that fail to load: abort, ignore or skip (default abort)
        /// <para><em>AKA: <c>--load-error-handling</c></em></para>
        /// </summary>
        [OptionFlag("--load-error-handling")]
        public ErrorHandling LoadErrorHandling { get; set; } = ErrorHandling.Abort;

        /// <summary>
        /// Specify how to handle media files that fail to load: abort, ignore or skip
        /// <para><em>AKA: <c>--load-media-error-handling</c></em></para>
        /// </summary>
        [OptionFlag("--load-media-error-handling")]
        public ErrorHandling LoadMediaErrorHandling { get; set; } = ErrorHandling.Ignore;

        /// <summary>
        /// Allowed conversion of a local file to read in other local files. When disabled, allows only paths explicitly allowed with <c>--allow</c>. 
        /// <para><em>AKA: <c>--enable-local-file-access</c> and <c>--disable-local-file-access</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-local-file-access", "--disable-local-file-access")]
        public bool LocalFileAccess { get; set; }

        /// <summary>
        /// Include the page in the table of contents and outlines
        /// <para><em>AKA: <c>--include-in-outline</c> and <c>--exclude-from-outline</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--include-in-outline", "--exclude-from-outline")]
        public bool IncludeInOutline { get; set; }

        /// <summary>
        /// Enable installed plugins <em>(plugins will likely not work)</em>
        /// <para><em>AKA: <c>--enable-plugins</c> and <c>--disable-plugins</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-plugins", "--disable-plugins")]
        public bool EnablePlugins { get; set; }

        /// <summary>
        /// Add an additional post field
        /// <para><em>AKA: <c>--post</c></em></para>
        /// </summary>
        [OptionFlag("--post")]
        public Dictionary<string, string> PostFields { get; set; }

        /// <summary>
        /// Post an additional file
        /// <para><em>AKA: <c>--post-file</c></em></para>
        /// </summary>
        [OptionFlag("--post-file")]
        public Dictionary<string, string> PostFiles { get; set; }

        /// <summary>
        /// Use print media-type (<c>@media print</c>) instead of screen
        /// <para><em>AKA: <c>--print-media-type</c> and <c>--no-print-media-type</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--print-media-type", "--no-print-media-type")]
        public bool PrintMediaType { get; set; }

        /// <summary>
        /// Resolve relative external links into absolute links
        /// <para><em>AKA: <c>--resolve-relative-links</c></em></para>
        /// </summary>
        [OptionFlag("--resolve-relative-links")]
        public bool ResolveRelativeLinks { get; set; } = true;

        /// <summary>
        /// Enable the intelligent shrinking strategy used by WebKit that makes the pixel/dpi ratio non-constant
        /// <para><em>AKA: <c>--enable-smart-shrinking</c> and <c>--disable-smart-shrinking</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-smart-shrinking", "--disable-smart-shrinking")]
        public bool EnableSmartShrinking { get; set; } = true;

        /// <summary>
        /// Path to the ssl client cert public key in OpenSSL PEM format, optionally followed by intermediate ca and trusted certs
        /// <para><em>AKA: <c>--ssl-crt-path</c></em></para>
        /// </summary>
        [PathOptionFlag("--ssl-crt-path")]
        public string CertificatePath { get; set; }

        /// <summary>
        /// Password to ssl client cert private key
        /// <para><em>AKA: <c>--ssl-key-password</c></em></para>
        /// </summary>
        [OptionFlag("--ssl-key-password")]
        public string CertificateKeyPassword { get; set; }

        /// <summary>
        /// Path to ssl client cert private key in OpenSSL PEM format
        /// <para><em>AKA: <c>--ssl-key-path</c></em></para>
        /// </summary>
        [PathOptionFlag("--ssl-key-path")]
        public string CertificateKeyPath { get; set; }

        /// <summary>
        /// Link from section header to table of contents
        /// <para><em>AKA: <c>--enable-toc-back-links</c> and <c>--disable-toc-back-links</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--enable-toc-back-links", "--disable-toc-back-links")]
        public bool EnableTableOfContentsBackLinks { get; set; }

        /// <summary>
        /// Set viewport size if you have custom scrollbars or css attribute overflow to emulate window size
        /// <para><em>AKA: <c>--viewport-size</c></em></para>
        /// </summary>
        [OptionFlag("--viewport-size")]
        public string ViewportSize { get; set; }

        /// <summary>
        /// Wait until <c>window.status</c> is equal to this string before rendering page
        /// <para><em>AKA: <c>--window-status</c></em></para>
        /// </summary>
        [OptionFlag("--window-status")]
        public string WaitUntilWindowStatus { get; set; }
    }
}
