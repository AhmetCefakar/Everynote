using Everynote.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Filter
{
	// Controller'lardaki action'lara istekte buluna bilmeyi oturum açmış kullanıcılar için izin veren Attribute tabnımı
	public class UserAuthentication : FilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			// İstekte bulunan kullanıcı login değilse Login sayfasına yönlendirme yapılıyor
			if (CurrentSession.User == null)
			{
				filterContext.Result = new RedirectResult("/User/Login");
			}
		}
	}
}