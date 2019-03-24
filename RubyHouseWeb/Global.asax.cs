using Autofac;
using Autofac.Integration.Mvc;
using RubyHouseServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RubyHouseWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //builder
            //    .RegisterAssemblyTypes(assembly)
            //    .AssignableTo<IAccountServices>()
            //    .AsImplementedInterfaces()
            //    .InstancePerRequest();
            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();
            //builder.RegisterModule(RubyHouseServices.IServices.IAccountServices);

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
            //builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //builder.RegisterType<ExtensibleActionInvoker>()
            //    .As<IActionInvoker>()
            //    .WithParameter("injectActionMethodParameters", true);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Log4net
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
