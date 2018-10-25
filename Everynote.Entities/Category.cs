using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    [Table("Category")]
    public class Category : EverynoteEntityBase
    {
        public Category()
        {
            Notes = new List<Note>();
        }

        [DisplayName("Kategori"), Required, StringLength(50)]
        public string Title { get; set; }

        [DisplayName("Açıklama"), StringLength(150)]
        public string Description { get; set; }

        // Bir kategorinin birden çok notu vardır
        public virtual List<Note> Notes { get; set; }
    }
}
