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
using Everynote.Mvc.Models;

namespace Everynote.Mvc.Controllers
{
	public class NoteController : Controller
	{
		NoteManager noteManager = new NoteManager();
		CategoryManager categoryManager = new CategoryManager();
		LikedManager likedManager = new LikedManager();

		// GET: Notes
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

				return RedirectToAction("Index");
			}
			ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
			return View(note);
		}

		// GET: Notes/Delete/5
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
		public ActionResult DeleteConfirmed(int id)
		{
			Note note = noteManager.Find(q => q.Id == id);
			noteManager.Delete(note);
			return RedirectToAction("Index");
		}

	}
}
