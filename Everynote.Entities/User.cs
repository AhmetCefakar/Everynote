using Everynote.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    public class User : EverynoteEntityBase
    {
        [DisplayName("Ad") ,StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Soyad"), StringLength(25)]
        public string Surname { get; set; }

		public Gender Gender { get; set; }
		
		[Required, DisplayName("Kullanıcı Adı"), StringLength(25)]
        public string UserName { get; set; }

        [Required, DisplayName("Şifre"), StringLength(100)]
        public string Password { get; set; }

        [Required, DisplayName("Mail Adresi"), StringLength(100)]
        public string Email { get; set; }

		[StringLength(50)]
		public string ProfileImageFileName { get; set; }
		
		public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        [Required]
		[ScaffoldColumn(false)] // 'ScaffoldColumn' kullanımı ile bu model UI tarafından otomatik üretim işlemlerinde kullanılırsa göz ardı edilme durumunu ayarlıyor.
		public Guid ActivateGuid { get; set; }


        // Bir kategorinin birden çok notu vardır
        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; } // çok-çok ilişkiyi sağlar

    }
}
