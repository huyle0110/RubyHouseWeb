using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModule.Model;

namespace EntityFrameWorkModule.ResultModel
{
    public class SearchProductResultModel
    {
        public SearchProductResultModel()
        {
            this.resultList = new List<Product>();
        }
        public int totalRecord { get; set; }
        public List<Product> resultList { get; set; }
    }

    public class SearchCategoryResultModel
    {
        public SearchCategoryResultModel()
        {
            this.resultList = new List<Category>();
        }
        public int totalRecord { get; set; }
        public List<Category> resultList { get; set; }
    }
}
