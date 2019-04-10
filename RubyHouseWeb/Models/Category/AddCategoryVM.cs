using EntityFrameWorkModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RubyHouseWeb.Models
{
    public class AddCategoryVM
    {
        public string CategoryName { get; set; }
        public int levelCategory { get; set; }
        public long? DisplayOrder { get; set; }
        public List<Category> categories { get; set; }
    }
}