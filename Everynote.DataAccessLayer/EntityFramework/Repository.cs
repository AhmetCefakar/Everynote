using Everynote.DataAccessLayer;
using Everynote.DataAccessLayer.Abstract;
using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.DataAccessLayer.EntityFramework
{
    // Generic(Genel) mimari kullanılarak oluşturulan Repository sınıfı
    public class Repository<T> : RepositoryBase, IRepository<T> where T : class
    {
        //private readonly DatabaseContext _db;
        private DbSet<T> _objectSet;
            
        // Todo: Repository altında -> Repositoy Constructor
        public Repository()
        {
            //_db = RepositoryBase.CreateContext(); // Singleton ile db context üretmeyi sağlar
            _objectSet = context.Set<T>();
        }
        
        /// <summary>
        ///  New'lenebilir bir T tipinden Entity sınıfının listesini geriye döndüren metod.
        /// </summary>
        /// <returns>T obj List</returns>
        public List<T> List()
        {
           return _objectSet.ToList();
        }
        
        /// <summary>
        /// New'lenebilir bir T tipinden Entity sınıfının listesini Expression ifadesine göre filtreleyip geriye döndüren metod.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            return Save();
        }

        public int Update(T obj)
        {
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
