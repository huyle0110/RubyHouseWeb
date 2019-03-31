using AutoMapper;
using eFMS.API.Common.Utils;
using eFMS.API.System.DL.Models;
using eFMS.API.System.Models;
using eFMS.API.System.Service.Models;
using System;
using System.Linq;

namespace eFMS.API.System.Infrastructure
{
    public class MappingProfile : Profile
    {
        public  MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<SysUserGroup, SysUserGroupModel>();
            CreateMap<SysUserGroupModel, SysUserGroup>();

            CreateMap<CatPortModel, CatPlaceModel>()
                     .ForMember(dest => dest.AreaId, option => option.MapFrom(f => f.Zone))
                     .ForMember(dest => dest.LocalAreaId, option => option.MapFrom(f => f.LocalZone))
                     .ForMember(dest => dest.NameVn, option => option.MapFrom(f => f.Name));

            CreateMap<CatPlace, CatPortModel>()
                     .ForMember(dest => dest.Name, option => option.MapFrom(f => f.NameVn))
                     .ForMember(dest => dest.Zone, option => option.MapFrom(f => f.AreaId))
                     .ForMember(dest => dest.LocalZone, option => option.MapFrom(f => f.LocalAreaId));

            CreateMap<CatPlaceModel, CatPortModel>()
                     .ForMember(dest => dest.Name, option => option.MapFrom(f => f.NameVn))
                     .ForMember(dest => dest.Zone, option => option.MapFrom(f => f.AreaId))
                     .ForMember(dest => dest.LocalZone, option => option.MapFrom(f => f.LocalAreaId));

            CreateMap<CatCommodity, CatCommodityModel>();
            CreateMap<CatCommodityModel, CatCommodity>();
            //CreateMap<SysUserGroup, SysUserGroupModel>();
            //CreateMap<SysUserGroupModel, SysUserGroup>();
            CreateMap<SysUserGroupEditModel, SysUserGroupModel>();
        }
    }
}
