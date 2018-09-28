using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everynote.DataAccessLayer.EntityFramework;
using Everynote.Entities;

namespace Everynote.BusinessLayer
{
    // Bu sınıf veritabanını en başta yoksa oluşturulması için kullanılıyor
    public class Test
    {
        Repository<Category> repoCategory = new Repository<Category>();
        Repository<EverynoteUser> repoUser = new Repository<EverynoteUser>();
        Repository<Note> repoNote = new Repository<Note>();
        Repository<Comment> repoComment = new Repository<Comment>();

        public Test()
        {
            // Repository class'ına erişip Category sınıfından listenin çekilmasi
            var myCateggoryList = repoCategory.List();
        }

        public void CommetInsertTest()
        {
            EverynoteUser everynoteUser = repoUser.Find(q => q.Id == 1);
            Note note = repoNote.Find(q => q.Id == 2);

            Comment comment = new Comment
            {
                Text = "This is a test comment!",
                CreatedOn = DateTime.Now,
                CreatedUserName = "admin",
                Owner = everynoteUser,
                Note = note
            };

            repoComment.Insert(comment);


        }

    }
}
