
using ais_web3.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;

namespace ais_web2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.HttpContext.Current.Application["MyAppSession"] = System.Web.HttpContext.Current.Session;
        }
    }
}
