using Everynote.Core.Abstract;
using Everynote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer.Abstract
{
	/// <summary>
	/// Bu sınıf, katmanlı mimariyi bozmadan UI katmanına DAL katmanını refere etmeden Repository işlemlerine erişmeyi sağlıyor
	/// Bu sınıf ile Repository deseni ile oluşturulmuş olan Database işlemleri hem BLL katmanında hem UI katmanında kullanılabilir
	/// Not: Bu sınıf UI kısmı .Net teknolojilerini destekler diğer platformlara destek için BLL kullanan Service katmanı yazmak gerekir
	/// </summary>
	/// <typeparam name="T">An Entity Type</typeparam>
	public abstract class ManagerBase<T> : IRepository<T> where T: class
	{
		private Repository<T> repository = new Repository<T>();

		public virtual int Delete(T obj)
		{
			return repository.Delete(obj);
		}

		public virtual T Find(Expression<Func<T, bool>> where)
		{
			return repository.Find(where);
		}

		public virtual int Insert(T obj)
		{
			return repository.Insert(obj);
		}

		public virtual List<T> List()
		{
			return repository.List();
		}

		public List<T> List(Expression<Func<T, bool>> where)
		{
			return repository.List(where);
		}

		public virtual IQueryable<T> ListQueryable()
		{
			return repository.ListQueryable();
		}

		public virtual int Save()
		{
			return repository.Save();
		}

		public virtual int Update(T obj)
		{
			return repository.Update(obj);
		}
	}
}
