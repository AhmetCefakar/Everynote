using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    public class Comment : EverynoteEntityBase
    {
        [Required, StringLength(300)]
        public string Text { get; set; }

        //public int NoteId { get; set; }
        //public int OwnerId { get; set; }

        // Bir kategorinin birden çok notu vardır
        public Note Note { get; set; }
        public User Owner { get; set; }
    }
}
