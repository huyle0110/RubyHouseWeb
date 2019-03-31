using EntityFrameWorkModule.EF;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using EntityFrameWorkModule.ResultModel;
using System.Collections.Generic;

namespace EntityFrameWorkModule.IServices
{
    public interface IProductServices: IGenericRepository<Product>
    {
        SearchProductResultModel search(SearchProductRequestModel model);
    }
}
