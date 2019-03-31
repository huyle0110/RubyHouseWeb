using eFMS.API.Common.Globals;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using System.Collections.Generic;

namespace eFMS.API.System.DL.IService
{
    public interface ICatCommodityService: IRepositoryBase<CatCommodity, CatCommodityModel>
    {
        HandleState Create(CatCommodityModel catCommodityModel);
        CatCommodityModel Get(int id);
        HandleState Delete(int id);
        HandleState Update(int id, CatCommodityModel model);
        List<CatCommodityModel> Paging(CatCommodityCriteria catCommodityCriteria, int page, int size, string orderByProperty, bool isAscendingOrder, out int rowsCount);
    }
}
