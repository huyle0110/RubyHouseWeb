using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.EF
{
    public abstract class RepositoryBase<TContext, TModel> : IRepositoryBase<TContext, TModel> where TContext : class, new()
        where TModel : class, new()
    {
        private readonly IContextBase<TContext> _DataContext;
        private readonly IMapper _mapper;
        public List<DataColumnError> DataColumnErrors { get; set; }

        protected virtual IContextBase<TContext> DataContext
        {
            get
            {
                return _DataContext;
            }
        }
        protected virtual IMapper mapper
        {
            get
            {
                return _mapper;
            }
        }
        public RepositoryBase(IContextBase<TContext> repository, IMapper mapper)
        {
            _DataContext = repository;
            _mapper = mapper;
        }
        public void SetChildren<TChild>(string MasterField, string ChildField, bool AllowDeleteChild)
            where TChild : class, new()
        {
            DataContext.SetChildren<TChild>(MasterField, ChildField, AllowDeleteChild);
        }
        public void SetChildren<TChild>(string MasterField, string ChildField)
           where TChild : class, new()
        {
            DataContext.SetChildren<TChild>(MasterField, ChildField, false);
        }

        public void SetParent<TParent>(string ChildrenField, string ParentField) where TParent : class, new()
        {
            DataContext.SetParent<TParent>(ChildrenField, ParentField);
        }

        public void SetUnique(string[] keyfields)
        {
            DataContext.SetUnique(keyfields);
        }

        public IQueryable<TModel> Get()
        {
            return Get(null, -1, -1);
        }
        public async Task<List<TModel>> GetAsync()
        {
            return await GetAsync(null);
        }

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate)
        {
            return Get(predicate, -1, -1);
        }

        public async Task<List<TModel>> GetAsync(Expression<Func<TModel, bool>> predicate)
        {
            List<TModel> result = new List<TModel>();
            try
            {
                List<TContext> contexts = new List<TContext>();
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    contexts = await _DataContext.GetAsync(query);
                }
                else
                {
                    contexts = await _DataContext.GetAsync();
                }
                contexts.ForEach(u =>
                {
                    result.Add(_mapper.Map<TContext, TModel>(u));
                });
            }
            catch { }
            return result;
        }

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate, int from, int count)
        {
            List<TModel> result = new List<TModel>();
            try
            {
                IQueryable<TContext> q = new List<TContext>().AsQueryable();
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    q = DataContext.Get(query);
                }
                else
                {
                    q = DataContext.Get();
                }
                if (from > -1)
                    q = q.Skip(from);
                if (count > -1)
                    q = q.Take(count);


                q.ToList().ForEach(u =>
                {
                    result.Add(_mapper.Map<TContext, TModel>(u));
                });
            }
            catch
            {
            }

            return result.AsQueryable();
        }
        public async Task<List<TModel>> GetAsync(Expression<Func<TModel, bool>> predicate, int from, int count)
        {
            List<TModel> q = new List<TModel>();
            q = await GetAsync(predicate);

            if (from > -1)
                q = q.Skip(from).ToList();
            if (count > -1)
                q = q.Take(count).ToList();
            return q;
        }

        public TModel First(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                TContext t = _DataContext.First(query);
                return _mapper.Map<TContext, TModel>(t);
            }
            catch
            {
            }
            return null;
        }

        public bool Any(Expression<Func<TModel, bool>> query)
        {
            return Get().Any(query);
        }
        public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> query)
        {
            return await Get().AnyAsync(query);
        }

        public int Count(Expression<Func<TModel, bool>> query)
        {
            return Get(query).Count();
        }

        public IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size)
        {
            int c = 0;
            var result = Paging(predicate, page, size, out c);
            return result;
        }
        public IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size, out int rowsCount)
        {
            return Paging(predicate, page, size, null, false, out rowsCount);
        }
        public async Task<(List<TModel> result, int rowsCount)> PagingAsync(Expression<Func<TModel, bool>> predicate, int page, int size)
        {
            return await PagingAsync(predicate, page, size, null, false);
        }

        public IQueryable<TModel> Paging(int page, int size, out int rowsCount)
        {
            return Paging(null, page, size, null, false, out rowsCount);
        }

        public async Task<(List<TModel> result, int rowsCount)> PagingAsync(int page, int size)
        {
            return await PagingAsync(null, page, size, null, false);
        }

        public IQueryable<TModel> Paging(Expression<Func<TModel, bool>> predicate, int page, int size, Expression<Func<TModel, object>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            //List<TModel> result = new List<TModel>();
            //rowsCount = 0;
            //try
            //{
            //    IQueryable<TContext> q = new List<TContext>().AsQueryable();
            //    Expression<Func<TContext, bool>> query = predicate != null ? ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate) : null;
            //    Expression<Func<TContext, object>> overproperty = orderByProperty != null ? ExpressionEx.ConvertExpression<TContext, TModel, object>(orderByProperty) : null;

            //    if (query != null)
            //    {                  
            //        if(overproperty != null)
            //            q = DataContext.Paging(query, page, size, overproperty, isAscendingOrder, out rowsCount);
            //        else
            //            q = DataContext.Paging(query, page, size, out rowsCount);
            //    }
            //    else
            //    {
            //        if (overproperty != null)
            //            q = DataContext.Paging(page, size, overproperty, isAscendingOrder, out rowsCount);
            //        else
            //            q = DataContext.Paging(query, page, size, out rowsCount);
            //    }              
            //    q.ForEachAsync(u =>
            //    {
            //        result.Add(_mapper.Map<TContext, TModel>(u));
            //    });
            //}
            //catch { }
            //return result.AsQueryable();

            var q = predicate == null ? Get() : Get(predicate);
            return q.Paging(page, size, orderByProperty, isAscendingOrder, out rowsCount);
        }
        public async Task<(List<TModel> result, int rowsCount)> PagingAsync(Expression<Func<TModel, bool>> predicate, int page, int size, Expression<Func<TModel, object>> orderByProperty, bool isAscendingOrder)
        {
            var q = predicate == null ? Get() : Get(predicate);
            return await q.PagingAsync(page, size, orderByProperty, isAscendingOrder);
        }

        public virtual HandleState Add(TModel entity)
        {
            return DataContext.Add(_mapper.Map<TModel, TContext>(entity));
        }

        public virtual Task<HandleState> AddAsync(TModel entity)
        {
            return DataContext.AddAsync(_mapper.Map<TModel, TContext>(entity));
        }

        public virtual HandleState Add(IEnumerable<TModel> entities)
        {
            List<TContext> lEntity = new List<TContext>();
            if (entities != null)
            {
                foreach (var ety in entities)
                {
                    lEntity.Add(_mapper.Map<TModel, TContext>(ety));
                }
            }
            return DataContext.Add(lEntity, true);
        }

        public virtual async Task<HandleState> AddAsync(IEnumerable<TModel> entities)
        {
            List<TContext> lEntity = new List<TContext>();
            if (entities != null)
            {
                foreach (var ety in entities)
                {
                    lEntity.Add(_mapper.Map<TModel, TContext>(ety));
                }
            }
            return await DataContext.AddAsync(lEntity, true);
        }

        public virtual HandleState Update(TModel entity, Expression<Func<TModel, bool>> predicate)
        {
            Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
            var ety = _mapper.Map<TModel, TContext>(entity);
            return DataContext.Update(ety, query, true);
        }
        public virtual async Task<HandleState> UpdateAsync(TModel entity, Expression<Func<TModel, bool>> query)
        {
            return await UpdateAsync(entity, query, true);
        }
        private HandleState Update(TModel entity, Expression<Func<TModel, bool>> predicate, bool allowSubmitChange)
        {
            try
            {
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    return DataContext.Update(_mapper.Map<TModel, TContext>(entity), query, allowSubmitChange);
                }
                else
                {
                    return new HandleState("Predicate is null");
                }
            }
            catch (Exception ex) { return new HandleState(ex); }
        }
        private async Task<HandleState> UpdateAsync(TModel entity, Expression<Func<TModel, bool>> predicate, bool allowSubmitChange)
        {
            try
            {
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    return await DataContext.UpdateAsync(_mapper.Map<TModel, TContext>(entity), query, allowSubmitChange);
                }
                else
                {
                    return new HandleState("Predicate is null");
                }
            }
            catch (Exception ex) { return new HandleState(ex); }
        }
        public virtual HandleState Delete(Expression<Func<TModel, bool>> predicate)
        {
            return Delete(predicate, true);
        }
        public virtual async Task<HandleState> DeleteAsync(Expression<Func<TModel, bool>> query)
        {
            return await DeleteAsync(query, true);
        }
        private HandleState Delete(Expression<Func<TModel, bool>> predicate, bool allowSubmitChange)
        {
            try
            {
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    return DataContext.Delete(query, allowSubmitChange);
                }
                else
                {
                    return new HandleState("Predicate is null");
                }
            }
            catch (Exception ex) { return new HandleState(ex); }
        }
        private async Task<HandleState> DeleteAsync(Expression<Func<TModel, bool>> predicate, bool allowSubmitChange)
        {
            try
            {
                if (predicate != null)
                {
                    Expression<Func<TContext, bool>> query = ExpressionEx.ConvertExpression<TContext, TModel, bool>(predicate);
                    return await DataContext.DeleteAsync(query, allowSubmitChange);
                }
                else
                {
                    return new HandleState("Predicate is null");
                }
            }
            catch (Exception ex) { return new HandleState(ex); }
        }

        //public virtual void Dispose()
        //{
        //    if (DataContext != null) DataContext.DC.Dispose();
        //    GC.SuppressFinalize(this);
        //}
    }
}
