using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Core.Abstract
{
    /// <summary>
    /// Bu Interface bir den fazla 
    /// </summary>
    /// <typeparam name="T">Generic Type For Entities</typeparam>
    public interface IRepository<T>
    {
        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfının listesini geriye döndüren metod.
        List<T> List();

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfının listesini Expression ifadesine göre filtreleyip geriye döndüren metod.
        // 'IQueryable' ile DB'ye sorgu atılmadan sorgu döndürülebilir
        List<T> List(Expression<Func<T, bool>> where);

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfının querable listesini Expression ifadesine göre filtreleyip geriye döndüren metod.
        IQueryable<T> ListQueryable();

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfını türünden Expression ifadesine göre tek kayıt ya da null değer döndüren metod.
        T Find(Expression<Func<T, bool>> where);

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfına yeni kayıt ekleyen metod.
        int Insert(T obj);

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfının güncelleme işlemini yapan metod(sadece gelen entity modelini DB'ye işler).
        int Update(T obj);

        // Under IRepository -> New'lenebilir bir T tipinden Entity sınıfını DB'den silen metod.
        int Delete(T obj);

        // Under IRepository -> Yapılan işlemleri DB'ye işleyen metod.
        int Save();
    }
}
