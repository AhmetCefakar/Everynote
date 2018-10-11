using Everynote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.DataAccessLayer.EntityFramework
{
    /// <summary>
    /// Bu sınıf Veritabanı context nesnesinin tüm program boyunca sedece bir defa üretilmesini sağlıyot(Used Singleton Pattern in This Class)
    /// </summary>
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static readonly object _lockSync = new object();

        // 'protected' ile sınıfın dışarıdan rem alanına çıkartılması engellenmiş oldu
        protected RepositoryBase()
        {
            if (context == null)
            {
                // 'lock' nesnesi multi thred uygulamalarda aynı anda sadece bir tane iş parçacığının bu bloğu çalıştıra bileceğini belirtir
                lock (_lockSync)
                {
                    context = new DatabaseContext();
                }
            }
        }

    }
}
