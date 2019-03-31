using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.EF
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected RubyHouseDbContext _db { get; set; }
        protected DbSet<T> _table = null;
        public GenericRepository()
        {
            _db = new RubyHouseDbContext();
            _table = _db.Set<T>();
        }
        public GenericRepository(RubyHouseDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> SelectAll()
        {
            return _table.ToList();
        }
        public int Delete(object id)
        {
            T existing = _table.Find(id);
            try
            {
                _table.Remove(existing);
                return 1;
            } 
            catch
            {
                return 0;
            }
        }

        public int Insert(T obj)
        {
            try
            {
                _table.Add(obj);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public T SelectById(object id)
        {
            try
            {
                return _table.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public int Update(T obj)
        {
            try
            {
                _table.Attach(obj);
                _db.Entry(obj).State = EntityState.Modified;
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
