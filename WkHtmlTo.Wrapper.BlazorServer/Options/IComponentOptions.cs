using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper.BlazorServer.Options
{
    public interface IComponentOptions : IHtmlOptions
    {
        Task RenderHtmlFromComponentAsync<TComponent>(HtmlRenderer renderer) where TComponent : IComponent;
    }
}
