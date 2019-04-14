using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.Services
{
    public class CategoryServices : GenericRepository<Category>, ICategoryServices
    {
        public List<Category> search(SearchCategoryRequestModel model)
        {
            return this.SelectAll().ToList();
        }
    }
}
