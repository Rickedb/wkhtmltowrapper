using System;

namespace WkHtmlToPdf.Wrapper.Options
{
    internal class OptionFlag : Attribute
    {
        public string SwitchName { get; private set; }

        public OptionFlag(string name)
        {
            SwitchName = name;
        }
    }
}
