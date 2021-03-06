﻿using Everynote.BusinessLayer;
using Everynote.BusinessLayer.Result;
using Everynote.Entities;
using Everynote.Entities.Messages;
using Everynote.Mvc.Filter;
using Everynote.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
	[UserAuthentication]
	public class ProfileController : Controller
    {
		/// <summary>
		/// Kullanıcının Profil Sayfasını Açan Action
		/// </summary>
		/// <returns></returns>
		public ActionResult ProfileShow()
		{
			EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
			BusinessLayerResult<User> businessLayerResultUser = everynoteUserManager.GetUserById(CurrentSession.User.Id);

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
			EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
			BusinessLayerResult<User> businessLayerResultUser = everynoteUserManager.GetUserById(CurrentSession.User.Id);

			if (businessLayerResultUser.Errors.Count > 0)
			{
				ErrorViewModel errorViewModel = new ErrorViewModel
				{
					Title = "Hata Oluştu",
					RedirectingTimeout = 4000,
					Items = businessLayerResultUser.Errors,
					RedirectingUrl = "/Profile/ProfileShow"
				};
				return View("Error", errorViewModel); // Shared altındaki 'Error' view'ına gider
			}

			return View(businessLayerResultUser.Result);
		}

		// Edit işlemini yapan action
		[HttpPost]
		public ActionResult ProfileEdit(User user, HttpPostedFileBase ProfileImage)
		{
			ModelState.Remove("CreatedUserName"); // 'EverynoteEntityBase' sınıfından gelen kayıt edenin bilgisini içeren field kontrolü siliniyor

			if (ModelState.IsValid)
			{
				#region Kullanıcı Resminin Değiştirilmesi
				if (ProfileImage != null && (
					ProfileImage.ContentType == "image/jpg" ||
					ProfileImage.ContentType == "image/jpeg" ||
					ProfileImage.ContentType == "image/png"))
				{
					string fileName = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}";

					ProfileImage.SaveAs(Server.MapPath($"~/images/{fileName}"));
					user.ProfileImageFileName = fileName;
				}
				#endregion

				EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
				BusinessLayerResult<User> businessLayerResultUser = everynoteUserManager.UpdateProfile(user);

				if (businessLayerResultUser.Errors.Count > 0)
				{
					ErrorViewModel errorViewModel = new ErrorViewModel
					{
						Title = "Hata Oluştu",
						RedirectingTimeout = 4000,
						Items = businessLayerResultUser.Errors,
						RedirectingUrl = "/Profile/ProfileEdit"
					};
					return View("Error", errorViewModel); // Shared altındaki 'Error' view'ına gider
				}

				// Profil değiştirildiği için session güncelleniyor.
				CurrentSession.Set<User>("login", businessLayerResultUser.Result);

				return RedirectToAction("ProfileEdit", "Profile");
			}

			return View(user); // Action'na yollanan model hatalı ise, ilgili model aynı sayfaya geri yollanıyor
		}

		// Delete işlemi yapan action
		public ActionResult ProfileDelete()
		{
			EverynoteUserManager everynoteUserManager = new EverynoteUserManager();
			BusinessLayerResult<User> businessLayerResultUser = everynoteUserManager.RemoveUserById(CurrentSession.User.Id);

			if (businessLayerResultUser.Errors.Count > 0)
			{
				ErrorViewModel errorViewModel = new ErrorViewModel
				{
					Title = "Profil silinemedi",
					RedirectingTimeout = 4000,
					Items = businessLayerResultUser.Errors,
					RedirectingUrl = "/Profile/ProfileShow"
				};

				return View("Error", errorViewModel); // Shared altındaki Error sayfasına gider
			}

			Session.Clear();

			return RedirectToAction("Index", "Home");
		}
	}
}