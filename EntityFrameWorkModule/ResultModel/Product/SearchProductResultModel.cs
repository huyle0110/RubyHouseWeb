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
        public int totalRecord { get; set; }
        public List<Product> resultList { get; set; }
    }
}
