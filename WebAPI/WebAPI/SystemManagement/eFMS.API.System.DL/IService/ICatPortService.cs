using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFMS.API.System.DL.IService
{
    public interface ICatPortService : IRepositoryBase<CatPlace, CatPlaceModel>
    {
        HandleState CreatePort(CatPlaceModel model);
        CatPlaceModel First(Guid id);
        HandleState Delete(Guid id);
        HandleState Update(Guid id, CatPlaceModel model);
        List<CatPortModel> Paging(PortIndexCriteria criteria, int page, int size, string orderByProperty, bool isAscendingOrder, out int rowsCount);
    }
}
