using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using EntityFrameWorkModule.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.Services
{
    public class CategoryServices : GenericRepository<Category>, ICategoryServices
    {
        public SearchCategoryResultModel search(SearchCategoryRequestModel model)
        {
            var result = new SearchCategoryResultModel();
            result.resultList = this.SelectAll().ToList();
            return result;
        }
    }
}
