using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.EF
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(object id);
        int Insert(T obj);
        int Update(T obj);
        int Delete(object id);
        void Save();
    }
}
