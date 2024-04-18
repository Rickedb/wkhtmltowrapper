using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper.AspNetCore.Options
{
    public interface IRazorViewOptions : IHtmlOptions
    {
        Task RenderViewToHtmlAsync(ActionContext actionContext, ViewDataDictionary viewData, string viewName);
    }
}
