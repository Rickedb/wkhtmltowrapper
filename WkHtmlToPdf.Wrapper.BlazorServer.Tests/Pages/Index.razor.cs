using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.BlazorServer;

namespace WkHtmlToPdf.Wrapper.BlazorServer.Tests.Pages
{
    public partial class Index
    {
        [Inject]
        public PdfComponentRenderer Renderer { get; set; }

        public Index()
        {

        }

        public async Task RunAsync()
        {
            
            await Renderer.GenerateAndDownloadAsync<Counter>();
        }
    }
}
