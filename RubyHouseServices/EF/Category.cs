using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.EF
{
    public class Category
    {
        public long ID { get; set; }
        public string CategoryName { get; set; }

        public string MetaTitle { get; set; }
        public long? ParentID { get; set; }
        public int? DisplayOrder { get; set; }
        public string SeoTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }  


        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
    }
}
