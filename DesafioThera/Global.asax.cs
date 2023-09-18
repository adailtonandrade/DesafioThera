using CrossCutting.IoC;
using Ninject;
using Ninject.Web.Mvc;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DesafioThera
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Create a Ninject kernel and load your modules
            IKernel kernel = new StandardKernel(new NinjectConfig());

            // Set the NinjectDependencyResolver as the MVC dependency resolver
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
