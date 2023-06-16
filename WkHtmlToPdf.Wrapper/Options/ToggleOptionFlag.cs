using System;

namespace WkHtmlToPdf.Wrapper.Options
{
    internal class ToggleOptionFlag : Attribute
    {
        public string SetName { get; private set; }
        public string UnsetName { get; private set; }

        public ToggleOptionFlag(string setName, string unsetName)
        {
            SetName = setName;
            UnsetName = unsetName;
        }
    }
}
