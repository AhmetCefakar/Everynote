using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everynote.Entities;
using Everynote.DataAccessLayer.EntityFramework;

namespace Everynote.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> repoNote = new Repository<Note>();

        public List<Note> GetAllNotes()
        {
            return repoNote.List();
        }

        public IQueryable<Note> GetAllNotesQueryable()
        {
            return repoNote.ListQueryable();
        }
    }
}
