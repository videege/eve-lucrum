using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Domain
{
    public interface IDataContext
    {
        void SaveChanges();

        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
    }
}
