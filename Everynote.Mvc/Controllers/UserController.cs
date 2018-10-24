using Everynote.BusinessLayer;
using Everynote.BusinessLayer.Result;
using Everynote.Entities;
using Everynote.Entities.DTO;
using Everynote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
	public class UserController : Controller
	{
		private readonly EverynoteUserManager userManager = new EverynoteUserManager();

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
				BusinessLayerResult<User> registerResult = userManager.RegisterUser(model);

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
			BusinessLayerResult<User> resultUser = userManager.ActivateUser(id);

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

		#region Admin İşlemleri
		// GET: Users
		public ActionResult Index()
		{
			return View(userManager.List());
		}

		// GET: Users/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = userManager.Find(q => q.Id == id.Value);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// GET: Users/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Users/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(User user)
		{
			ModelState.Remove("CreatedUserName");
			ModelState.Remove("CreatedOn");
			if (ModelState.IsValid)
			{
				BusinessLayerResult<User> businessLayerResult = userManager.Insert(user);
				if (businessLayerResult.Errors.Count > 0)
				{
					businessLayerResult.Errors.ForEach(q => ModelState.AddModelError("", q.Message));
					return View(user); // Aynı sayfaya hata bilgileri ile modelin geri yollanması
				}

				return RedirectToAction("Index");
			}

			return View(user);
		}

		// GET: Users/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = userManager.Find(q => q.Id == id.Value);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,Surname,Gender,UserName,Password,Email,ProfileImageFileName,IsActive,IsAdmin")] User user)
		{
			if (ModelState.IsValid)
			{
				// Todo: Varolan kullanıcı bilgileri ile kullanıcı düzenlenemeyecek şekilde yazılacak!
				BusinessLayerResult<User> businessLayerResult = userManager.Update(user);
				if (businessLayerResult.Errors.Count > 0)
				{
					businessLayerResult.Errors.ForEach(q => ModelState.AddModelError("", q.Message));
					return View(user); // Aynı sayfaya hata bilgileri ile modelin geri yollanması
				}
			
				return RedirectToAction("Index");
			}
			return View(user); // Aynı sayfaya güncelleme işlemi sonrası modelin geri yollanması
		}

		// GET: Users/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = userManager.Find(q => q.Id == id.Value);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			// Todo: Silme işleminde kayıt silinmeden pasife çekilecek şekilde ayarlanmalıdır
			User user = userManager.Find(q => q.Id == id);
			userManager.Delete(user);
			return RedirectToAction("Index");
		}
		
		#endregion
	}
}