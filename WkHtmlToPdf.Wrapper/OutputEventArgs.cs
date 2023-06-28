namespace WkHtmlToPdf.Wrapper
{
    public struct OutputEventArgs
    {
        public OutputEvent Event { get; set; }

        public OutputEventArgs(OutputEvent ev)
        {
            Event = ev; 
        }
    }
}
