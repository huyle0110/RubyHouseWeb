using AutoMapper;
using eFMS.API.Common.Globals;
using eFMS.API.Common.Utils;
using eFMS.API.System.DL.Infrastructure;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace eFMS.API.System.DL.Services
{
    public class CatCommodityService : RepositoryBase<CatCommodity, CatCommodityModel>, ICatCommodityService
    {
        private readonly eFMSDataContext context = new eFMSDataContext();
        public CatCommodityService(IContextBase<CatCommodity> repository, IMapper mapper): base(repository, mapper)
        {

        }

        public HandleState Create(CatCommodityModel catCommodityModel)
        {
            var catCommodity = mapper.Map<CatCommodity>(catCommodityModel);
            // Always Active when creating a new commodity
            catCommodity.Inactive = false;

            var resultCreate = DataContext.Add(catCommodity);
            return resultCreate;
        }

        public HandleState Delete(int id)
        { 
            var catCommodityService = DataContext.First(f => f.Id == id);
            if (catCommodityService != null)
            {
                catCommodityService.Inactive = true;
            }
            // Only update status of catCommodity Object
            var updateCommodity = DataContext.Update(catCommodityService, u => u.Id == id);
            return updateCommodity;
        }

        public CatCommodityModel Get(int id)
        {
            var getCommodity = DataContext.First(f => f.Id == id);
            return mapper.Map<CatCommodityModel>(getCommodity);
        }

        public HandleState Update(int id, CatCommodityModel model)
        {
            var updateResult = DataContext.Update(mapper.Map<CatCommodity>(model), u => u.Id == id);
            return updateResult;
        }

        public List<CatCommodityModel> Paging(CatCommodityCriteria criteria, int page , int size, string orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            Expression<Func<CatCommodity, bool>> query = x => (criteria.Id.HasValue? x.Id == criteria.Id : true)
                                        && (x.CommodityNameEn ?? "").Contains(criteria.Commodity_EN ?? "")
                                        && (x.CommodityNameVn ?? "").Contains(criteria.Commodity_VN ?? "")
                                        && (x.Inactive == false);
            var result = new List<CatCommodityModel>();
            if (size > 1)
            {
                if (page < 1)
                    page = 1;
                if (!string.IsNullOrEmpty(orderByProperty) && (isAscendingOrder || !isAscendingOrder))
                {
                    var orderBy = ExpressionExtension.CreateExpression<CatCommodity, object>(orderByProperty);
                    var resultTest = DataContext.Paging(query, page, size, orderBy, isAscendingOrder, out rowsCount).ToList();
                    result = mapper.Map<List<CatCommodityModel>>(DataContext.Paging(query, page, size, orderBy, isAscendingOrder, out rowsCount).ToList());
                }
                else
                {
                    result = mapper.Map<List<CatCommodityModel>>(DataContext.Paging(query, page, size, out rowsCount).ToList());
                }
            }
            else
            {
                var resultGet = DataContext.Get(query);
                rowsCount = resultGet.Count();
                result = mapper.Map<List<CatCommodityModel>>(result.ToList());
            }
            if (result.Count() > 0)
            {
                result.ForEach(item =>
                {
                    var tempCommodityGroup = context.CatCommodityGroup.FirstOrDefault(f => f.Id == item.CommodityGroupId);
                    item.CommodityGroupName = tempCommodityGroup != null ? tempCommodityGroup.GroupNameEn : null;
                });
            }
            return result.OrderBy(o => o.Id).ToList();
        }
    }
}
