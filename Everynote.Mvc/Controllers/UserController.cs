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
	public class UserController : Controller
	{
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
					if (loginResult.Errors.Find(q => q.Code == Entities.Enums.ErrorMessageCode.UserIsNotActive) != null)
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
			return RedirectToAction("Index", "Home");
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

				SuccessViewModel successViewModel = new SuccessViewModel()
				{
					Title = "Kayıt Başarılı",
					RedirectingUrl = "/User/Login",

				};
				successViewModel.Items.Add("Mail adresinize yollana aktivasyon mail'ini kontrol edip hesabınızı doğrulayınız!");

				return View("Success", successViewModel); // Shared altındaki 'Success' view'ına gider
			}

			return View(model);
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

				ErrorViewModel errorViewModel = new ErrorViewModel
				{
					Title = "Geçersiz İşlem",
					RedirectingTimeout = 4000,
					Items = resultUser.Errors
				};
				return View("Error", errorViewModel); // Shared altındaki 'Error' view'ına gider
			}

			SuccessViewModel successViewModel = new SuccessViewModel()
			{
				Title = "Hesap Aktiflerştirildi!",
				RedirectingUrl = "/User/Login"
			};
			successViewModel.Items.Add("Hesabınız Doğrulandı. Not paylaşabilirsiniz!");

			return View("Success", successViewModel); // Shared altındaki 'Success' view'ına gider
		}
		#endregion
	}
}