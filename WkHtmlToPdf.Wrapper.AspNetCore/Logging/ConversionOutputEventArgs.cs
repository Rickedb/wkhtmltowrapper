namespace WkHtmlToPdf.Wrapper.AspNetCore.Logging
{
    public struct ConversionOutputEventArgs
    {
        public ConversionOutputEvent Event { get; set; }

        public ConversionOutputEventArgs(ConversionOutputEvent ev)
        {
            Event = ev;
        }
    }
}
