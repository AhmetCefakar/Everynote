using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities
{
    // Note - User tabloları arasında olan çok-çok ilişkiyi tutan ara tablo
    public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NoteId { get; set; }
        public int LikedUserId { get; set; }

        public Note Note { get; set; }
        public EverynoteUser LikedUser { get; set; }
    }
}
