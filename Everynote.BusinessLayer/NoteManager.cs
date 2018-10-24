using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everynote.Entities;
using Everynote.DataAccessLayer.EntityFramework;
using Everynote.BusinessLayer.Abstract;

namespace Everynote.BusinessLayer
{
    public class NoteManager : ManagerBase<Note>
    {
        public List<Note> GetAllNotes()
        {
            return List();
        }

        public IQueryable<Note> GetAllNotesQueryable()
        {
            return ListQueryable();
        }
    }
}
