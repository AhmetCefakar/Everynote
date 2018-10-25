using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    public class Note : EverynoteEntityBase
    {
        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(5000)]
        public string Text { get; set; }

        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        
        public int CategoryId { get; set; }
		// [ScaffoldColumn(false)]
		//public int OwnerId { get; set; } // User tablosuyla ilişkili
		
		// Bir kategorinin birden çok notu vardır
		public virtual Category Category { get; set; }
        public virtual User Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}
