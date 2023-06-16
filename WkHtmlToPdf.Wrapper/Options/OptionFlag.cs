using System;

namespace WkHtmlToPdf.Wrapper.Options
{
    internal class OptionFlag : Attribute
    {
        public string Name { get; private set; }

        public OptionFlag(string name)
        {
            Name = name;
        }
    }
}
