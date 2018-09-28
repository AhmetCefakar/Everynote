﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everynote.Entities;

namespace Everynote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        // EF'ye tabloların sınıflarının tanıtılması
        public DbSet<EverynoteUser> EverynoteUser { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Liked> Liked { get; set; }

    }

    /// <summary>
    /// Veritabanı ilk oluşturulunca başlangıç verilerini ekleyecek class
    /// </summary>
    class DatabaseInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            #region EverynoteUser Table
            List<EverynoteUser> userList = new List<EverynoteUser> {
                new EverynoteUser{
                    Name = "Ahmet",
                    Surname = "Cefakar",
                    Email = "ahmetcefakar@hotmail.com",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = true,
                    UserName = "ahmet",
                    Password = "abc",
                    CreatedOn = DateTime.Now,
                    CreatedUserName = "ahmet"
                },
                new EverynoteUser
                {
                    Name = "Elif",
                    Surname = "Doruk",
                    Email = "elifdoruk@hotmail.com",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = "elif",
                    Password = "abc",
                    CreatedOn = DateTime.Now,
                    CreatedUserName = "elif",
                }
            };
            context.EverynoteUser.AddRange(userList);
            context.SaveChanges();
            #endregion
            
            // Adding fake Categories
            for (int i = 0; i < 20; i++)
            {
                Category category = new Category
                {
                    Title = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(),
                    CreatedUserName = "admin"
                };
                context.Category.Add(category);

                // Adding notes
                for (int j = 0; j < FakeData.NumberData.GetNumber(2, 9); j++)
                {
                    Note note = new Note
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 30)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 5)),
                        Category = category,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 5),
                        Owner = (j % 2 == 0) ? userList[0] : userList[1],
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        CreatedUserName = (j % 2 == 0) ? userList[0].UserName : userList[1].UserName,
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now)
                    };
                    category.Notes.Add(note);

                    // Adding comments
                    for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                    {
                        Comment comment = new Comment
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Note = note,
                            Owner = (k % 2 == 0) ? userList[0] : userList[1],
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                            CreatedUserName = (k % 2 == 0) ? userList[0].UserName : userList[1].UserName
                        };

                        note.Comments.Add(comment);
                    }

                    // Adding fake likes
                    for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                    {
                        Liked liked = new Liked
                        {
                            LikedUser = (k % 2 == 0) ? userList[0] : userList[1]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
            base.Seed(context);
        }
    }
}