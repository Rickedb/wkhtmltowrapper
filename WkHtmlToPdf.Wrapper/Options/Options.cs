using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WkHtmlToPdf.Wrapper.Options
{
    public abstract class Options : IOptions
    {
        public virtual string ToSwitchCommand()
        {
            var builder = new StringBuilder();

            var fields = GetType().GetProperties();
            foreach (var fi in fields)
            {
                object value = fi.GetValue(this, null);
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
               
                if (value is Dictionary<string, string> dictionary)
                {
                    foreach (var d in dictionary)
                    {
                        builder.AppendFormat(" {0} {1} {2}", flag.SwitchName, d.Key, d.Value);
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
                else
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, " {0}  \"{1}\"", flag.SwitchName, value);
                }
            }

            return builder.ToString().Trim();
        }
    }
}
