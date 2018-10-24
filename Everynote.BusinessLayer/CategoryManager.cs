using Everynote.BusinessLayer.Abstract;
using Everynote.DataAccessLayer.EntityFramework;
using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
		// Todo: Silme işlemlerinde veritabanından kayıtlar silinmemeli, sadece silindi kolon değeri true yapılmalıdır
		public override int Delete(Category obj)
		{
			return base.Delete(obj);
		}

		// Kodları sil, artık gereksizler. BLL katmanıında DAL katmanındaki Repository yapısına generic olarak erişimi sağlayacak bir tasarım yapıldı.
		//private Repository<Category> repoCategory = new Repository<Category>();
		//// Category sınıfından bir liste döndüren metod
		//public List<Category> GetAllCategories()
		//{
		//    return repoCategory.List();
		//}

		//// Category sınıfından bir liste döndüren metod
		//public Category GetCategoryById(int id)
		//{
		//    return repoCategory.Find(q => q.Id == id);
		//}

	}
}
