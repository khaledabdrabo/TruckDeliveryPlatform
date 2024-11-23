using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace YourNamespace.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            // Set Arabic culture
            var culture = new CultureInfo("ar-SA");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
} 