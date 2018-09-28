using Everynote.DataAccessLayer.EntityFramework;
using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer
{
    public class CategoryManager
    {
        private Repository<Category> repoCategory = new Repository<Category>();

        // Category sınıfından bir liste döndüren metod
        public List<Category> GetAllCategories()
        {
            return repoCategory.List();
        }

        // Category sınıfından bir liste döndüren metod
        public Category GetCategoryById(int id)
        {
            return repoCategory.Find(q => q.Id == id);
        }

    }
}
