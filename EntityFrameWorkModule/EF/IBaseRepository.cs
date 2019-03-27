using RubyHouseServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.EF
{
    public interface IRepositoryBase<TContext, TModel>
        where TContext : class, new()
        where TModel : class, new()
    {
        //List<DataColumnError> DataColumnErrors { get; }
        void SetChildren<TChild>(string MasterField, string ChildField, bool AllowDeleteChild) where TChild : class, new();
        void SetChildren<TChild>(string MasterField, string ChildField) where TChild : class, new();
        void SetParent<TParent>(string ChildrentField, string ParentField) where TParent : class, new();
        void SetUnique(string[] keyfields);

        IQueryable<TModel> Get();
        Task<List<TModel>> GetAsync();

        IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate);
        Task<List<TModel>> GetAsync(Expression<Func<TModel, bool>> predicate);

        IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate, int from, int count);
        Task<List<TModel>> GetAsync(Expression<Func<TModel, bool>> predicate, int from, int count);

        TModel First(Expression<Func<TModel, bool>> predicate);

        bool Any(Expression<Func<TModel, bool>> query);
        Task<bool> AnyAsync(Expression<Func<TModel, bool>> query);

        int Count(Expression<Func<TModel, bool>> query);

        IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size);
        IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size, out int rowsCount);
        Task<(List<TModel> result, int rowsCount)> PagingAsync(Expression<Func<TModel, bool>> predicate, int page, int size);

        IQueryable<TModel> Paging(int page, int size, out int rowsCount);
        Task<(List<TModel> result, int rowsCount)> PagingAsync(int page, int size);
        IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size, Expression<Func<TModel, object>> orderByProperty, bool isAscendingOrder, out int rowsCount);
        Task<(List<TModel> result, int rowsCount)> PagingAsync(Expression<Func<TModel, bool>> predicate, int page, int size, Expression<Func<TModel, object>> orderByProperty, bool isAscendingOrder);
        ResponseModel Add(TModel entity);
        Task<ResponseModel> AddAsync(TModel entity);
        ResponseModel Add(IEnumerable<TModel> entities);
        Task<ResponseModel> AddAsync(IEnumerable<TModel> entities);
        ResponseModel Update(TModel entity, Expression<Func<TModel, bool>> predicate);
        Task<ResponseModel> UpdateAsync(TModel entity, Expression<Func<TModel, bool>> query);
        ResponseModel Delete(Expression<Func<TModel, bool>> predicate);
        Task<ResponseModel> DeleteAsync(Expression<Func<TModel, bool>> query);
    }
}
