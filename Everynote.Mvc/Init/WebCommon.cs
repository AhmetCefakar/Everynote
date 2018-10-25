using Everynote.Core.Authentication;
using Everynote.Entities;
using Everynote.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Everynote.Mvc.Init
{
	public class WebCommon : ICommon
	{
		/// <summary>
		/// Login olan kullanıcının adını döndüren method
		/// </summary>
		/// <returns></returns>
		public string GetCurrentUSerName()
		{
			if (CurrentSession.User != null)
				return CurrentSession.User.UserName;
			return "System";

		}
	}
}