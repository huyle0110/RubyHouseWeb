using AutoMapper;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.System.DL.Services
{
    public class CatCommodityGroupService :RepositoryBase<CatCommodityGroup, CatCommodityGroupModel>, ICatCommodityGroupService
    {
        public CatCommodityGroupService(IContextBase<CatCommodityGroup> repository, IMapper mapper) : base(repository, mapper)
        {

        }

        List<CatCommodityGroupModel> ICatCommodityGroupService.Get()
        {
            return mapper.Map<List<CatCommodityGroupModel>>(DataContext.Get());
        }
    }
}
