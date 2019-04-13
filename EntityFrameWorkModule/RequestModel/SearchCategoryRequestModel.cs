using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.RequestModel
{
    public class SearchCategoryRequestModel
    {
        public string categoryName { get; set; }
        public int Status { get; set; }
        public bool showMenu { get; set; }
    }
}
