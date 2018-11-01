using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Entities.DTO;
using Everynote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
	public class HomeController : Controller
	{
		private readonly NoteManager noteManager = new NoteManager();
		private readonly CategoryManager categoryManager = new CategoryManager();

		// GET: Home
		public ActionResult Index()
		{
			// Veritabanını yoksa oluşturan metod çağırılıyor
			//BusinessLayer.Test test = new BusinessLayer.Test();
			//test.CommetInsertTest();

			var deger = noteManager.GetAllNotesQueryable().Where(q => q.IsDraft == false).OrderByDescending(q => q.CreatedOn);

			return View(deger.ToList());
		}

		// GET: Bu action kendine gelen id değerine göre bu kategori değerine sahip olan notları listelemek için BLL katmanından aldığı modeli
		// 'Home/Index' action'ına yönlendiriyor
		public ActionResult ByCategory(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			
			Category category = categoryManager.Find(q => q.Id == id.Value);

			if (category == null)
			{
				return HttpNotFound();
			}

			return View("Index", category.Notes.Where(q => q.IsDraft == false).OrderByDescending(q => q.CreatedOn).ToList());
		}

		// GET: En çok beğenilenler notların listesini döndüren action
		public ActionResult MostLiked()
		{
			return View("Index", noteManager.GetAllNotes().OrderByDescending(q => q.LikeCount).ToList());
		}

		// GET: About
		public ActionResult About()
		{
			return View();
		}

		// TestNotification
		public ActionResult TestNotify()
		{
			SuccessViewModel model = new SuccessViewModel()
			{
				Header = "Başarılı Mesaj denemesi",
				Title = "Success Test",
				RedirectingTimeout = 4000,
				Items = new List<string> { "Test başarılı 1", "Test başarılı 2" }
			};
			
			return View("Success", model);
		}

	}
}