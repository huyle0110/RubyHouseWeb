using ITL.NetCore.Connection.EF;
using Microsoft.Extensions.DependencyInjection;
using eFMS.API.System.Service.Contexts;
using Microsoft.Extensions.Localization;
using LocalizationCultureCore.StringLocalizer;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Services;
using eFMS.API.System.Service.Models;

namespace eFMS.API.System.Infrastructure
{
    public static class ServiceRegister
    {

        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IStringLocalizer, JsonStringLocalizer>();
            services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddScoped(typeof(IContextBase<>), typeof(Base<>));

            services.AddTransient<IUserGroupService, UserGroupService>();
            //ICatPortService
            services.AddTransient<ICatPortService, CatPortService>();
            services.AddTransient<IContextBase<CatPlace>, Base<CatPlace>>();

            services.AddTransient<ICatCommodityService, CatCommodityService>();
            services.AddTransient<IContextBase<CatCommodity>, Base<CatCommodity>>();
            services.AddTransient<ICatBranchService, CatBranchService>();

            services.AddTransient<ICatCountryService, CatCountryService>();
            services.AddTransient<ICatAreaService, CatAreaService>();
            services.AddTransient<ICatCommodityGroupService, CatCommodityGroupService>();
        }
    }
}
