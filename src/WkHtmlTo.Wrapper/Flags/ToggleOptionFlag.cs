﻿namespace WkHtmlTo.Wrapper.Flags
{
    internal class ToggleOptionFlag : OptionFlag
    {
        public string FalseSwitchName { get; private set; }

        public ToggleOptionFlag(string name, string unsetName) : base(name)
        {
            FalseSwitchName = unsetName;
        }
    }
}
