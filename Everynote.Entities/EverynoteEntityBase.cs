using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    // Bu sınıf Generic olarak tanımlandı Id değeri duruma göre int, Guid, string olarak tanımlanabilir
    public class EverynoteEntityBase
    {
        // Her tablonun bir Id değeri olur
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Her tabloda kaydeden-kayıt tarihi ve değiştiren-değiştirme tarihi alanları olur
        [Required, ScaffoldColumn(false)]
        public string CreatedUserName { get; set; }

        [Required, ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

		[ScaffoldColumn(false)] // 'ScaffoldColumn' ile otomatik UI kısmına model oluşturulurken ilgili property'nin kullanılıp kullanılmaması ayarlanıyor
		public string ModifiedUserName { get; set; }

		[ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }
    }
}
