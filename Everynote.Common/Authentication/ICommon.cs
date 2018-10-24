using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Core.Authentication
{
	public interface ICommon
	{
		/// <summary>
		/// Geriye kullanıcı adını dönen metod
		/// </summary>
		/// <returns></returns>
		string GetCurruntUSerName();
	}
}
