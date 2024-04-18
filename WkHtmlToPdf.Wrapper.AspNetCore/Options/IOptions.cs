using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Options
{
    public interface IOptions
    {
        string ToSwitchCommand();
    }

    public interface IPdfOptions : IOptions
    {
        string OutputPath { get; set; }
    }

    public interface IFileOrUrlOptions : IPdfOptions
    {
        string HtmlFilePathOrUrl { get; set; }
    }

    public interface IHtmlOptions : IPdfOptions
    {
        string Html { get; set; }
    }

    public interface IRazorViewOptions : IHtmlOptions
    {
        Task RenderViewToHtmlAsync(ActionContext actionContext, ViewDataDictionary viewData, string viewName);
    }
    public interface IComponentOptions : IHtmlOptions
    {
        Task RenderComponentToHtmlAsync(PageContext pageContext, ViewDataDictionary viewData, string pageName);
    }

    public interface IRazorPageOptions : IHtmlOptions
    {
        Task RenderPageToHtmlAsync(PageContext pageContext, ViewDataDictionary viewData, string pageName);
    }
}
