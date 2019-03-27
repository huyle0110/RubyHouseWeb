using RubyHouseServices.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.EF
{
    public interface IContextBase<T> : IDisposable
         where T : class, new()
    {
        DbContext DC { get; }
        //void ConfigDataContext<D>(string connectionString);

        void SetChildren<TChild>(string MasterField, string ChildField, bool AllowDeleteChild)
        where TChild : class, new();

        void SetParent<TParent>(string ChildrentField, string ParentField)
        where TParent : class, new();

        void SetUnique(string[] keyfields);
        bool CheckExist(T entitty);

        bool HaveParent(T entitty);

        IQueryable<T> Where(Expression<Func<T, bool>> exp);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp);

        IQueryable<T> Get();
        Task<List<T>> GetAsync();

        IQueryable<T> Get(Expression<Func<T, bool>> query);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> query);

        IQueryable<T> Get(Expression<Func<T, bool>> query, int from, int count);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> query, int from, int count);

        T First(Expression<Func<T, bool>> query);
        Task<T> FirstAsync(Expression<Func<T, bool>> query);

        bool Any(Expression<Func<T, bool>> query);
        Task<bool> AnyAsync(Expression<Func<T, bool>> query);

        int Count(Expression<Func<T, bool>> query);
        Task<int> CountAsync(Expression<Func<T, bool>> query);

        IQueryable<T> Paging(Expression<Func<T, bool>> query, int page, int size, out int rowsCount);
        Task<(List<T> result, int rowsCount)> PagingAsync(Expression<Func<T, bool>> query, int page, int size);

        IQueryable<T> Paging(int page, int size, out int rowsCount);
        Task<(List<T> result, int rowsCount)> PagingAsync(int page, int size);
        IQueryable<T> Paging(int page, int size, Expression<Func<T, object>> orderByProperty, bool isAscendingOrder, out int rowsCount);
        IQueryable<T> Paging(Expression<Func<T, bool>> query, int page, int size, Expression<Func<T, object>> orderByProperty, bool isAscendingOrder, out int rowsCount);
        Task<(List<T> result, int rowsCount)> PagingAsync(Expression<Func<T, bool>> query, int page, int size, Expression<Func<T, object>> orderByProperty, bool isAscendingOrder);
        ResponseModel Add(T entity);
        Task<ResponseModel> AddAsync(T entity);
        ResponseModel Add(T entity, bool allowSubmitChange);

        Task<ResponseModel> AddAsync(T entity, bool allowSubmitChange);

        ResponseModel Add(IEnumerable<T> entities);
        Task<ResponseModel> AddAsync(IEnumerable<T> entities);

        ResponseModel Add(IEnumerable<T> entities, bool allowSubmitChange);
        Task<ResponseModel> AddAsync(IEnumerable<T> entities, bool allowSubmitChange);

        ResponseModel Update(T entity, Expression<Func<T, bool>> query);
        Task<ResponseModel> UpdateAsync(T entity, Expression<Func<T, bool>> query);

        ResponseModel Update(T entity, Expression<Func<T, bool>> query, bool allowSubmitChange);
        Task<ResponseModel> UpdateAsync(T entity, Expression<Func<T, bool>> query, bool allowSubmitChange);
        //HandleState Update(T entity, Expression<Func<T, bool>> query, IEnumerable<string> propertyNames, bool reverse, bool allowSubmitChange);
        ResponseModel Delete(Expression<Func<T, bool>> query);
        Task<ResponseModel> DeleteAsync(Expression<Func<T, bool>> query);
        ResponseModel Delete(Expression<Func<T, bool>> query, bool allowSubmitChange);
        Task<ResponseModel> DeleteAsync(Expression<Func<T, bool>> query, bool allowSubmitChange);
        ResponseModel SubmitChanges();
    }
}
