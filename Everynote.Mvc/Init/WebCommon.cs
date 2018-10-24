using Everynote.Core.Authentication;
using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Everynote.Mvc.Init
{
	public class WebCommon : ICommon
	{
		public string GetCurruntUSerName()
		{
			if (HttpContext.Current.Session["login"] != null)
			{
				User user = HttpContext.Current.Session["login"] as User;
				return user.UserName;
			}
			else
			{
				return "System";
			}
		}
	}
}