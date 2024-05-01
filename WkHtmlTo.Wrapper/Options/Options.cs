using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    public abstract class Options : IOptions
    {
        private static Lazy<string> _lazyAssemblyPath = new Lazy<string>(GetCurrentAssemblyPath);

        public virtual string ToSwitchCommand()
        {
            var builder = new StringBuilder();

            var fields = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fi in fields)
            {
                var value = fi.GetValue(this, null);
                if (value == null)
                    continue;

                if (value is IOptions innerOption)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", innerOption.ToSwitchCommand());
                    continue;
                }

                var flag = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (flag == null)
                    continue;

                if (value is IDictionary<string, string> dictionary)
                {
                    foreach (var d in dictionary)
                    {
                        builder.AppendFormat(" {0} \"{1}\" \"{2}\"", flag.SwitchName, d.Key, d.Value);
                    }
                }
                else if (value is bool boolean)
                {
                    if (boolean)
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", flag.SwitchName);
                    }
                    else if (flag is ToggleOptionFlag toggleFlag)
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, " {0}", toggleFlag.FalseSwitchName);
                    }
                }
                else if (value is string str)
                {
                    if (flag is PathOptionFlag && !PathInfo.IsPathFullyQualified(str))
                    {
                        str = Path.Combine(_lazyAssemblyPath.Value, str);
                    }

                    builder.AppendFormat(CultureInfo.InvariantCulture, " {0}  \"{1}\"", flag.SwitchName, str);
                }
                else
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, " {0}  \"{1}\"", flag.SwitchName, value);
                }
            }

            return builder.ToString().Trim();
        }

        private static string GetCurrentAssemblyPath()
            => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
