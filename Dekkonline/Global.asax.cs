using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace DekkOnline
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {


            //Saving a backup to a file in the /backup folder
            Application.Add("loggSti", Server.MapPath("~/Admin/log/log.txt"));
            Application.Add("loginError", Server.MapPath("~/Admin/log/loginerror.txt"));
            // Code that runs on application startup
           
            RegisterRoutes();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterRoutes()
        {
            
            RouteTable.Routes.Add(new Route("{resource}.ico/{*pathInfo}", new StopRoutingHandler()));
            RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            RouteTable.Routes.Add(new Route("{resource}.ashx/{*pathInfo}", new StopRoutingHandler()));
            //RouteTable.Routes.MapPageRoute("Default", "{title}", "~/Default.aspx");
            RouteTable.Routes.MapPageRoute("Tyres", "Tyres/{section}/{size}/{brand}", "~/Tyres.aspx");
            RouteTable.Routes.MapPageRoute("Dekk", "Dekk", "~/Tyres.aspx");
            RouteTable.Routes.MapPageRoute("News", "news/{id}", "~/newsN.aspx");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //RouteTable.Routes.MapPageRoute("DMS", "eDMS/{Action}","~/Citroen/Default.aspx");
        }

    }
}