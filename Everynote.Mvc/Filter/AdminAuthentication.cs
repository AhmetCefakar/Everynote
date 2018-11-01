using Everynote.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Filter
{
	// Admin kullanıcısının erişebileceği sayfaları kontrol etmek için yazılan Attribute
	public class AdminAuthentication : FilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (CurrentSession.User != null && CurrentSession.User.IsAdmin == false)
			{
				filterContext.Result = new RedirectResult("/Home/AccessDenide"); // Todo: Yetkisiz işlem sayfasına yönlendirme yapılacak, sayfanın oluşturulup bağlanması gerekiyor
			}
		}
	}
}