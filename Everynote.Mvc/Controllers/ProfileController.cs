using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
    public class ProfileController : Controller
    {
		/// <summary>
		/// Kullanıcının Profil Sayfasını Açan Action
		/// </summary>
		/// <returns></returns>
		public ActionResult ProfileShow()
		{
			User Currentuser = Session["login"] as User;
			EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
			BusinessLayerResult<User> businessLayerResultUser = everynoteUserManager.GetUserById(Currentuser.Id);

			if (businessLayerResultUser.Errors.Count > 0)
			{
				ErrorViewModel errorViewModel = new ErrorViewModel
				{
					Title = "Hata Oluştu",
					RedirectingTimeout = 4000,
					Items = businessLayerResultUser.Errors
				};
				return View("Error", errorViewModel); // Shared altındaki 'Error' view'ına gider
			}

			return View(businessLayerResultUser.Result);
		}

		// Edit sayfasını açan action
		public ActionResult ProfileEdit()
		{
			return View();
		}

		// Edit işlemini yapan action
		[HttpPost]
		public ActionResult ProfileEdit(User user)
		{
			return View();
		}

		// Delete işlemi yapan action
		public ActionResult ProfileDelete()
		{
			return View();
		}
	}
}