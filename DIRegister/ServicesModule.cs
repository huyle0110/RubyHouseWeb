using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DIRegister
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<AccountServices>().As<IAccountServices>().InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.Load("EntityFrameWorkModule"))
                .Where(s => s.Name.EndsWith("Services"))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            base.Load(builder);

        }
    }
}
