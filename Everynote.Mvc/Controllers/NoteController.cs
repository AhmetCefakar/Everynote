using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Mvc.Filter;
using Everynote.Mvc.Models;

namespace Everynote.Mvc.Controllers
{
	public class NoteController : Controller
	{
		NoteManager noteManager = new NoteManager();
		CategoryManager categoryManager = new CategoryManager();
		LikedManager likedManager = new LikedManager();

		// GET: Notes
		[UserAuthentication]
		public ActionResult Index()
		{
			var notes = noteManager.ListQueryable()
								   .Include("Category") // Sorguya verilen tablo için Join atılmasını sağlar.
								   .Include("Owner")
								   .Where(q => q.Owner.Id == CurrentSession.User.Id)
								   .OrderByDescending(q => q.ModifiedOn);

			return View(notes.ToList());
		}

		// GET: Liked Notes
		[UserAuthentication]
		public ActionResult MyLikedNotes()
		{
			// likedNotes listesinin çekilmesinin sağlayan linq sorgusu
			IOrderedQueryable<Note> notes = likedManager.ListQueryable()
						.Include(q => q.Note)
						.Include(q => q.LikedUser)
						.Where(q => q.LikedUserId == CurrentSession.User.Id)
						.Select(q => q.Note)
						.Include(q => q.Category)
						.Include(q => q.Owner)
						.OrderByDescending(q => q.ModifiedOn);

			return View("Index", notes.ToList());
		}

		// GET: Notes/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Note note = noteManager.Find(q => q.Id == id.Value);
			if (note == null)
			{
				return HttpNotFound();
			}
			return View(note);
		}

		// GET: Notes/Create
		[UserAuthentication]
		public ActionResult Create()
		{
			ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
			return View();
		}

		// POST: Notes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[UserAuthentication]
		public ActionResult Create(Note note)
		{
			ModelState.Remove("CreatedUserName");
			ModelState.Remove("CreatedOn");
			if (ModelState.IsValid)
			{
				note.Owner = CurrentSession.User;
				noteManager.Insert(note);
				return RedirectToAction("Index");
			}

			ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
			return View(note);
		}

		// GET: Notes/Edit/5
		[UserAuthentication]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Note note = noteManager.Find(q => q.Id == id.Value);
			if (note == null)
			{
				return HttpNotFound();
			}
			ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
			return View(note);
		}

		// POST: Notes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[UserAuthentication]
		public ActionResult Edit(Note note)
		{
			ModelState.Remove("CreatedUserName");
			ModelState.Remove("CreatedOn");
			if (ModelState.IsValid)
			{
				Note noteModelFromDB = noteManager.Find(q => q.Id == note.Id);
				noteModelFromDB.IsDraft = note.IsDraft;
				noteModelFromDB.CategoryId = note.CategoryId;
				noteModelFromDB.Text = note.Text;
				noteModelFromDB.Title = note.Title;

				noteManager.Update(noteModelFromDB); // DB'de note değerinin güncellenmesi

				return RedirectToAction("Index");
			}
			ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
			return View(note);
		}

		// GET: Notes/Delete/5
		[UserAuthentication]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Note note = noteManager.Find(q => q.Id == id.Value);
			if (note == null)
			{
				return HttpNotFound();
			}
			return View(note);
		}

		// POST: Notes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[UserAuthentication]
		public ActionResult DeleteConfirmed(int id)
		{
			Note note = noteManager.Find(q => q.Id == id);
			noteManager.Delete(note);
			return RedirectToAction("Index");
		}

		// Geriye, giriş yapmış olan kullanıcının UI kısmında listelenen notlardan beğendiklerinin id listesini döndüren metod
		[HttpPost]
		[UserAuthentication]
		public ActionResult GetLiked(int[] IdArrays)
		{
			if (CurrentSession.User != null)
			{
				List<int> liketNoteIds = likedManager.List(
								q => q.LikedUser.Id == CurrentSession.User.Id &&
								IdArrays.Contains(q.Note.Id)).
								Select(q => q.Note.Id).ToList();

				return Json(new { result = liketNoteIds });
			}
			return Json(new { result = new List<int>() });
		}

		// Like-Dislike işlemlerini yönetmek için kullanılan metod
		[HttpPost]
		[UserAuthentication]
		public ActionResult GetLikeState(int noteId, bool likedState)
		{
			int result = 0;

			if (CurrentSession.User != null)
			{
				#region DB'de like-dislike işlemlerinin işlenmesi
				Liked liked = likedManager.Find(q => q.Note.Id == noteId && q.LikedUser.Id == CurrentSession.User.Id);
				Note note = noteManager.Find(q => q.Id == noteId);

				if ((note != null && liked != null) && likedState == false)
				{
					result = likedManager.Delete(liked);
				}
				else  if ((note != null && liked == null) && likedState == true)
				{
					result = likedManager.Insert(new Liked {
						LikedUser = CurrentSession.User,
						Note = note,
					});
				}
				#endregion

				#region LikeCount değerini ayarlama
				if (note != null && result > 0)
				{
					if (likedState)
					{
						note.LikeCount++;
					}
					else
					{
						note.LikeCount--;
					}
					result = noteManager.Update(note);
					
					return Json(new { hasError = false, errorMessage = string.Empty, likeCount = note.LikeCount });
				}
				#endregion
			}
			return Json(new { hasError = true, errorMessage = "Beğenme işlemide hata oluştu!", likeCount = 0 });
		}

		// Geriye ilgili notun detayını döndüren metod
		public ActionResult GetNoteContent(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Note note = noteManager.Find(q => q.Id == id.Value);
			if (note == null)
			{
				return HttpNotFound();
			}
			
			return PartialView("_PartialNoteContent", note);
		}
		
	}
}
