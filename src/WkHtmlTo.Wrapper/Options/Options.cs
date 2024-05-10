using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// The base class of any available wkhtmlto option or argument
    /// </summary>
    public abstract class Options : IOptions
    {
        /// <summary>
        /// Convert the configured options to switch command arguments to use at wkhtmlto executable
        /// </summary>
        /// <returns>The arguments in a string representation</returns>
        public virtual string ToSwitchCommand()
        {
            var builder = new StringBuilder();
            var fields = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fi in fields)
            {
                var value = fi.GetValue(this, null);
                if (value == null)
                    continue;

                if (value is IOptions innerOption)
                {
                    builder.AppendFormat(innerOption);
                    continue;
                }

                var flag = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (flag == null)
                    continue;

                if (value is IDictionary<string, string> dictionary)
                {
                    builder.AppendFormat(flag, dictionary);
                }
                else if (value is bool boolean)
                {
                    builder.AppendFormat(flag, boolean);
                }
                else if (value is string str)
                {
                    builder.AppendFormat(flag, str);
                }
                else if (value is IEnumerable<string> enumerable)
                {
                    builder.AppendFormat(flag, enumerable);
                }
                else
                {
                    builder.AppendFormat(flag, value);
                }
            }

            return builder.ToString().Trim();
        }
    }
}
