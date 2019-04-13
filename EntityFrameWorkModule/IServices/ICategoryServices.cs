using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.IServices
{
    public interface ICategoryServices : IGenericRepository<Category>
    {
        List<Category> search(SearchCategoryRequestModel model);
    }
}
