﻿using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using EntityFrameWorkModule.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.Services
{
    public class ProductServices : GenericRepository<Product>,IProductServices
    {
        public SearchProductResultModel search(SearchProductRequestModel model)
        {
            var student = this.SelectAll().ToList();
            throw new NotImplementedException();
        }
    }
}
