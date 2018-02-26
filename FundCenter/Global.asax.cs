using Autofac;
using Autofac.Integration.Mvc;
using FundCenter.Filters;
using FundCenter.Logics;
using FundCenter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FundCenter
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<CacheStore>().As<ICache>();
            builder.RegisterType<AccountLogic>().As<IAccountLogic>();
            builder.RegisterType<FundsLogic>().As<IFundsLogic>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.Register(c => new AuthorizationFilter(c.Resolve<ICache>())).AsAuthorizationFilterFor<Controller>().InstancePerRequest();
            builder.Register(c => new FundCenterActionFilter()).AsActionFilterFor<Controller>().InstancePerRequest();
            builder.Register(c => new ExceptionHandlingAttribute()).AsExceptionFilterFor<Controller>().InstancePerRequest();

            builder.RegisterFilterProvider();
            // Build the container.
            var container = builder.Build();

            // Configure MVC with the dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Configure MVC move the version header
            MvcHandler.DisableMvcResponseHeader = true;

        }
    }
}