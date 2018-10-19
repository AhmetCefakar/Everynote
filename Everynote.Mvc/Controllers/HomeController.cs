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
		// GET: Home
		public ActionResult Index()
		{
			// Veritabanını yoksa oluşturan metod çağırılıyor
			//BusinessLayer.Test test = new BusinessLayer.Test();
			//test.CommetInsertTest();

			NoteManager noteManager = new NoteManager();
			return View(noteManager.GetAllNotesQueryable().OrderByDescending(q => q.CreatedOn).ToList());
		}

		// GET: Bu action kendine gelen id değerine göre bu kategori değerine sahip olan notları listelemek için BLL katmanından aldığı modeli
		// 'Home/Index' action'ına yönlendiriyor
		public ActionResult ByCategory(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}

			CategoryManager categoryManager = new CategoryManager();
			Category category = categoryManager.GetCategoryById(id.Value);

			if (category == null)
			{
				return HttpNotFound();
			}

			return View("Index", category.Notes.OrderByDescending(q => q.CreatedOn).ToList());
		}

		// GET: En çok beğenilenler notların listesini döndüren action
		public ActionResult MostLiked()
		{
			NoteManager noteManager = new NoteManager();

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