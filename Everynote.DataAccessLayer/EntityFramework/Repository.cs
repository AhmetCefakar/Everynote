﻿using Everynote.Core.Abstract;
using Everynote.Core.Authentication;
using Everynote.DataAccessLayer;
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

        /// <summary>
        /// New'lenebilir bir T tipinden Entity sınıfının listesini Queryable olarak geriye döndüren metod.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

			if (obj is EverynoteEntityBase)
			{
				EverynoteEntityBase baseEntity = obj as EverynoteEntityBase;
				baseEntity.CreatedOn = DateTime.Now;
				baseEntity.CreatedUserName = App.Common.GetCurrentUSerName();
			}

            return Save();
        }

        public int Update(T obj)
        {
			if (obj is EverynoteEntityBase)
			{
				EverynoteEntityBase baseEntity = obj as EverynoteEntityBase;
				baseEntity.ModifiedOn = DateTime.Now;
				baseEntity.ModifiedUserName = App.Common.GetCurrentUSerName();
			}

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
