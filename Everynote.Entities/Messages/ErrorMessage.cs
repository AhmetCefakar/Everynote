using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities.Messages
{
	/// <summary>
	/// Hata tutmak için kullanılan sınıf
	/// </summary>
	public class ErrorMessage
	{
		public ErrorMessageCode Code { get; set; }
		public string Message { get; set; }
	}
}
