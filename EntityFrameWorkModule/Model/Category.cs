using EntityFrameWorkModule.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.Model
{
    public class Category
    {
        public long ID { get; set; }
        public string CategoryName { get; set; }
        public levelCategory? levelCategory { get; set; }
        public long? parentCategory { get; set; }

        public string MetaTitle { get; set; }
        public int? DisplayOrder { get; set; } // Khong hiển thị thì là null, hiển thị theo vị trí từ 1
        public string SeoTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
    }
}
