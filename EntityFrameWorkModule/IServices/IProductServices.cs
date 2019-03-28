using EntityFrameWorkModule.RequestModel;
using EntityFrameWorkModule.ResultModel;

namespace EntityFrameWorkModule.IServices
{
    public interface IProductServices
    {
        SearchProductResultModel search(SearchProductRequestModel model);
    }
}
