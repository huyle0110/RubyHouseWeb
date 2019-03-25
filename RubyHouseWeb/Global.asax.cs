using Autofac;
using Autofac.Integration.Mvc;
using DIRegister;
using RubyHouseServices.IServices;
using RubyHouseWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Log4net
            log4net.Config.XmlConfigurator.Configure();

            RegisterDependencies();
        }
        public class WebDependencyRegistration : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                if (builder == null)
                {
                    throw new ArgumentNullException(nameof(builder),
                            "Argument builder can not be null.");
                }
                //The line below tells autofac, when a controller is initialized, pass into its constructor, the implementations of the required interfaces
                builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());
                base.Load(builder);
            }
        }
        private void RegisterDependencies()
        {
            //Logic Layer Dependency
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new WebDependencyRegistration());
            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();
            //set Autofac as default Dependency Resolver for application
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}
