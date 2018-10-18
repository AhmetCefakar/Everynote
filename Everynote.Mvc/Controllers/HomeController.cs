using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Entities.DTO;
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

		#region User Process
		#region Login
		// GET: Login, Login sayfası çağırıldığında çalışan action
		public ActionResult Login()
		{
			return View();
		}

		// POST: Login, Login sayfası post olduğunda çalışan action
		[HttpPost]
		public ActionResult Login(LoginDTO model)
		{
			if (ModelState.IsValid)
			{
				EverynoteUserManager userManager = new EverynoteUserManager();
				BusinessLayerResult<User> loginResult = userManager.LoginUser(model);

				if (loginResult.Errors.Count > 0)
				{
					// Eğer kullanıcı aktif değilse ekranda kullanıcıya özel aktive etme butonu gösterilecek
					if (loginResult.Errors.Find(q => q.Code == Entities.Messages.ErrorMessageCode.UserIsNotActive) != null)
					{
						ViewBag.ActiveLink = "http://www.google.com";
					}

					loginResult.Errors.ForEach(q => ModelState.AddModelError("", q.Message)); // BLL'den gelen hatalar ModelState'e ekleniyor 
					return View(model);
				}

				Session["login"] = loginResult.Result;
				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		// GET: Loginout, Login'i düşüren action
		public ActionResult Logout()
		{
			Session.Clear();
			return View();
		}
		#endregion

		#region Register
		// GET: Register
		public ActionResult Register()
		{
			return View();
		}

		// POST: Register, Register sayfası post olduğunda çalışan action
		[HttpPost]
		public ActionResult Register(RegisterDTO model)
		{
			// Modelin annotation kurallarana uygunğu kontrol ediliyor
			if (ModelState.IsValid)
			{
				EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
				BusinessLayerResult<User> registerResult = everynoteUserManager.RegisterUser(model);

				if (registerResult.Errors.Count > 0)
				{
					registerResult.Errors.ForEach(q => ModelState.AddModelError("", q.Message)); // BLL'den gelen hatalar ModelState'e ekleniyor 
					return View(model);
				}

				return RedirectToAction("RegisterOk", "Home");
			}

			return View(model);
		}

		public ActionResult RegisterOk()
		{
			return View(); //RedirectToAction("Index", "Home");
		}

		#endregion


		#region UserActivate
		// Kullanıcının kendini aktif etmesini sağlayan action
		public ActionResult UserActivate(Guid id)
		{
			// Todo: Kullanıcı aktivasyonu sağlanacak 
			EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
			BusinessLayerResult<User> resultUser = everynoteUserManager.ActivateUser(id);

			if (resultUser.Errors.Count > 0)
			{
				TempData["Errors"] = resultUser.Errors;
				return RedirectToAction("UserActivateCancel", "Home");
			}

			return RedirectToAction("UserActivateOk", "Home");
		}

		public ActionResult UserActivateOk()
		{
			return View();
		}

		public ActionResult UserActivateCancel()
		{
			List<Entities.Messages.ErrorMessage> errors = null;

			if (TempData["Errors"] != null)
			{
				errors = TempData["Errors"] as List<Entities.Messages.ErrorMessage>;
			}

			return View(errors);
		}
		#endregion


		#endregion





	}
}