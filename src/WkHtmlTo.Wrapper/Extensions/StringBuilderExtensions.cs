using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WkHtmlTo.Wrapper.Flags;
using WkHtmlTo.Wrapper.Options;

namespace System.Text
{
    internal static class StringBuilderExtensions
    {
        internal static void AppendFormat(this StringBuilder builder, OptionFlag flag, string value)
        {
            if(flag is PathOptionFlag)
            {
                value = PathInfo.GetAbsolutePath(value);
            }

            builder.AppendFormat(CultureInfo.InvariantCulture, " {0}  \"{1}\"", flag.SwitchName, value);
        }

        internal static void AppendFormat(this StringBuilder builder, OptionFlag flag, IEnumerable<string> value)
        {
            foreach (var item in value)
            {
                builder.AppendFormat(flag, item);
            }
        }

        internal static void AppendFormat(this StringBuilder builder, OptionFlag flag, IDictionary<string, string> value)
        {
            foreach (var kvp in value)
            {
                builder.AppendFormat(" {0} \"{1}\" \"{2}\"", flag.SwitchName, kvp.Key, kvp.Value);
            }
        }

        internal static void AppendFormat(this StringBuilder builder, OptionFlag flag, bool value)
        {
            if (value)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", flag.SwitchName);
            }
            else if (flag is ToggleOptionFlag toggleFlag)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", toggleFlag.FalseSwitchName);
            }
        }

        internal static void AppendFormat(this StringBuilder builder, OptionFlag flag, object value)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, " {0}  \"{1}\"", flag.SwitchName, value);
        }

        internal static void AppendFormat(this StringBuilder builder, IOptions value)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", value.ToSwitchCommand());
        }
    }
}
