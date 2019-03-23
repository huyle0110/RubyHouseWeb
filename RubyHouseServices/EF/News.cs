using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.EF
{
    public class News
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }

        public string ImagePath { get; set; }
        public long CategoryId { get; set; }

        public bool IsHot { get; set; }
        public long ViewCount { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
    }
}
