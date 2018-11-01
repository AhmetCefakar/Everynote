using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Mvc.Filter;
using Everynote.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
    public class CommentController : Controller
    {
		NoteManager noteManager = new NoteManager();
		CommentManager commentManager = new CommentManager();

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Include ile verisi çekilen ilgili sınıfın bağlu olduğu tablolardaki verilerde çekilir
			Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(q => q.Id == id.Value);

			if (note == null)
			{
				return HttpNotFound();
			}

            return PartialView("_PartialComments", note.Comments);
        }
		
		// Post: Comment edit işlemini yapan metod
		[HttpPost]
		[UserAuthentication]
		public ActionResult Edit(int? id, string text)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Comment comment = commentManager.Find(q => q.Id == id.Value);

			if (comment == null)
			{
				return new HttpNotFoundResult();
			}
			comment.Text = text;

			if (commentManager.Update(comment) > 0)
			{
				return Json(new { result = true }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { result = false }, JsonRequestBehavior.AllowGet);
		}

		// Post: Comment edit işlemini yapan metod
		[HttpGet]
		[UserAuthentication]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Comment comment = commentManager.Find(q => q.Id == id.Value);

			if (comment == null)
			{
				return new HttpNotFoundResult();
			}

			if (commentManager.Delete(comment) > 0)
			{
				return Json(new { result = true }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { result = false }, JsonRequestBehavior.AllowGet);
		}

		// Post: Comment edit işlemini yapan metod
		[HttpPost]
		[UserAuthentication]
		public ActionResult Insert(string text, int noteId)
		{
			ModelState.Remove("CreatedOn");
			ModelState.Remove("CreatedUserName");
			if (ModelState.IsValid)
			{
				Note note = noteManager.Find(q => q.Id == noteId);

				if (note == null)
				{
					return new HttpNotFoundResult();
				}
				
				if (commentManager.Insert(new Comment { Text = text, Note = note, Owner = CurrentSession.User }) > 0)
				{
					return Json(new { result = true }, JsonRequestBehavior.AllowGet);
				}
			}
			return Json(new { result = false }, JsonRequestBehavior.AllowGet);
		}
	}
}