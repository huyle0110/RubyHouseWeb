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
    public class CatPortService : RepositoryBase<CatPlace, CatPlaceModel>, ICatPortService
    {
        private readonly eFMSDataContext context = new eFMSDataContext();
        public CatPortService(IContextBase<CatPlace> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public CatPlaceModel First(Guid id)
        {
            var getResult = DataContext.First(x => x.Id == id);
            return mapper.Map<CatPlaceModel>(getResult);
        }

        public HandleState Delete(Guid id)
        {
            var portIndex = DataContext.First(f => f.Id == id);
            if (portIndex != null)
                portIndex.Inactive = true;
            var resultUpdate = DataContext.Update(portIndex, u => u.Id == id);
            return resultUpdate;
        }

        public HandleState Update(Guid id, CatPlaceModel model)
        {
            CatPlace catPlace = mapper.Map<CatPlace>(model);
            var resultUpdate = DataContext.Update(catPlace , u => u.Id == id);
            return resultUpdate;
        }

        public HandleState CreatePort(CatPlaceModel model)
        {
            model.Id = Guid.NewGuid();
            model.PlaceTypeId = PlaceType.Port.ToString();
            model.Inactive = false;
            return DataContext.Add(mapper.Map<CatPlace>(model));
        }

        public List<CatPortModel> Paging(PortIndexCriteria criteria, int page, int size, string orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            var countrySearch = new List<short>();
            if (!string.IsNullOrEmpty(criteria.Country))
            {
                // Get Country Name from Country Context to get CountryID
                countrySearch = (from c in context.CatCountry
                                 where c.NameVn.Contains(criteria.Country)
                                 select c.Id).ToList();
            }
            Expression<Func<CatPlace, bool>> query = x => (x.Code ?? "").Contains(criteria.Code ?? "")
                                        && (x.NameVn ?? "").Contains(criteria.Name ?? "")
                                        && (countrySearch.Contains(x.CountryId.Value) || string.IsNullOrEmpty(criteria.Country))
                                        && (x.AreaId ?? "").Contains(criteria.Zone ?? "")
                                        && (x.LocalAreaId ?? "").Contains(criteria.LocalZone ?? "")
                                        && (x.ModeOfTransport.Contains(criteria.Mode) || string.IsNullOrEmpty(criteria.Mode))
                                        && (x.Inactive == false);
            var result = new List<CatPortModel>();
            if (size > 1)
            {
                if (page < 1)
                    page = 1;
                if (!string.IsNullOrEmpty(orderByProperty) && (isAscendingOrder || !isAscendingOrder))
                {
                    var orderBy = ExpressionExtension.CreateExpression<CatPlace, object>(orderByProperty);
                    result = mapper.Map<List<CatPortModel>>(DataContext.Paging(query, page, size, orderBy, isAscendingOrder, out rowsCount).ToList());
                }
                else
                {
                    result = mapper.Map<List<CatPortModel>>(DataContext.Paging(query, page, size, out rowsCount).ToList());
                }
            }
            else
            {
                var resultGet = DataContext.Get(query);
                rowsCount = resultGet.Count();
                result = mapper.Map<List<CatPortModel>>(result.ToList());
            }
            result.ForEach(item =>
            {
                var country = context.CatCountry.FirstOrDefault(f => f.Id == item.CountryId);
                item.Country = country != null ? country.NameEn : null;
            });
            return result.OrderBy(o => o.Code).ToList();
        }
    }
}
