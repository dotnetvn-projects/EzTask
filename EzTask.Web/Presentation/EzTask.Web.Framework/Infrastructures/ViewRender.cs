using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.Infrastructures
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRender(IRazorViewEngine razorViewEngine, 
            ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, ControllerContext context, object model)
        {            
            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(context, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }
                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };
                var viewContext = new ViewContext(context, viewResult.View, viewDictionary, new TempDataDictionary(Context.Current, _tempDataProvider), sw, new HtmlHelperOptions());
                viewContext.RouteData = Context.Current.GetRouteData();
                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}
