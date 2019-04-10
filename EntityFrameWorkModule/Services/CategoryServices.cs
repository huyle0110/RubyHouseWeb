using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.Services
{
    public class CategoryServices : GenericRepository<Category>, ICategoryServices
    {
        
    }
}
